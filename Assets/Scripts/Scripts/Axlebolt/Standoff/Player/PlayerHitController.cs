using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Occlusion;
using Axlebolt.Standoff.Player.Ragdoll;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player
{
	public class PlayerHitController : MonoBehaviour, IHitController
	{
		private PlayerController _playerController;

		private PlayerRagdollController _playerRagdollController;

		private PlayerHitboxConfig _config;

		private readonly Dictionary<BipedMap.Bip, PlayerHitbox> _playerHitboxes = new Dictionary<BipedMap.Bip, PlayerHitbox>();

		private PlayerOcclusionController _playerOcclusionController;

		private bool _setOcclusionInvisible;

		public Action<HitData> OnHit
		{
			get;
			set;
		}

		public CapsuleCollider Trigger
		{
			get;
			set;
		}

		public void PreInitialize(PlayerHitboxConfig hitboxConfig)
		{
			_config = hitboxConfig;
			_playerController = GetComponent<PlayerController>();
			_playerRagdollController = GetComponent<PlayerRagdollController>();
			_playerController.CharacterSkinSetEvent += OnCharacterSkinSet;
			_playerOcclusionController = _playerController.PlayerOcclusionController;
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = Tags.PlayerHitTrigger;
			Trigger = gameObject.AddComponent<CapsuleCollider>();
			Trigger.center = hitboxConfig.trigger.center;
			Trigger.direction = hitboxConfig.trigger.direction;
			Trigger.height = hitboxConfig.trigger.height;
			Trigger.radius = hitboxConfig.trigger.radius;
			Trigger.isTrigger = true;
			gameObject.layer = LayerMask.NameToLayer("PlayerHitbox");
			gameObject.tag = Tags.PlayerHitTrigger;
			foreach (PlayerHitboxConfig.HitboxConfig hitbox in _config.hitboxes)
			{
				_playerHitboxes[hitbox.bone] = ConfigureBone(_playerController.BipedMap.GetBone(hitbox.bone).gameObject, hitbox);
			}
		}

		private void OnCharacterSkinSet(BipedMap bipedMap)
		{
			_playerHitboxes.Clear();
			foreach (PlayerHitboxConfig.HitboxConfig hitbox in _config.hitboxes)
			{
				_playerHitboxes[hitbox.bone] = ConfigureBone(bipedMap.GetBone(hitbox.bone).gameObject, hitbox);
			}
		}

		private PlayerHitbox ConfigureBone(GameObject boneGO, PlayerHitboxConfig.HitboxConfig hitboxConfig)
		{
			PlayerHitbox playerHitbox = boneGO.AddComponent<PlayerHitbox>();
			playerHitbox.PlayerHitController = this;
			playerHitbox.Initialize(hitboxConfig, _config.layer);
			return playerHitbox;
		}

		public void EnableBoneHit()
		{
			_setOcclusionInvisible = false;
			if (!_playerOcclusionController.IsVisible)
			{
				_playerOcclusionController.SetVisible(isVisible: true);
				_setOcclusionInvisible = true;
			}
			Trigger.enabled = false;
			foreach (KeyValuePair<BipedMap.Bip, PlayerHitbox> playerHitbox in _playerHitboxes)
			{
				playerHitbox.Value.Enable();
			}
		}

		public void DisableBoneHit()
		{
			if (_setOcclusionInvisible && _playerOcclusionController.IsVisible)
			{
				_playerOcclusionController.SetVisible(isVisible: false);
				_setOcclusionInvisible = false;
			}
			Trigger.enabled = true;
			foreach (KeyValuePair<BipedMap.Bip, PlayerHitbox> playerHitbox in _playerHitboxes)
			{
				playerHitbox.Value.Disable();
			}
		}

		public void Hit(HitData hitData)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			hitData.Serialize(networkWriter);
			byte[] array = networkWriter.AsArray();
			_playerController.PhotonView.RPC("HitViaServer", PhotonTargets.AllBufferedViaServer, array);
		}

		[PunRPC]
		private void HitViaServer(byte[] bytes, PhotonMessageInfo msgInfo)
		{
			PhotonPlayer victim = _playerController.PhotonView.owner;
			PhotonPlayer sender = msgInfo.sender;
			HitData hitData = DeserializeHitData(bytes);
			Vector3 point = hitData.Hit.Point;
			if (!victim.IsLocal && point != Vector3.zero)
			{
				Singleton<CharacterImpactsEmitter>.Instance.Emit(point, hitData.Direction, sender.IsLocal);
			}
			Singleton<HitManager>.Instance.Hit(sender, victim, msgInfo.timestamp, hitData, delegate(bool isDeath)
			{
				if (isDeath)
				{
					if (!_playerOcclusionController.IsVisible)
					{
						_playerOcclusionController.SetVisible(isVisible: true);
					}
					CreateRagdoll(hitData);
					if (victim.IsLocal)
					{
						_playerController.KillPlayer();
					}
				}
				else
				{
					OnHit?.Invoke(hitData);
				}
			});
		}

		public void HitImmediate(HitData hitData)
		{
			CreateRagdoll(hitData);
			Singleton<PlayerManager>.Instance.ReturnToPool(base.gameObject);
		}

		private void CreateRagdoll(HitData hitData)
		{
			Singleton<RagdollManager>.Instance.Simulate(_playerRagdollController, hitData);
		}

		private static HitData DeserializeHitData(byte[] bytes)
		{
			NetworkReader reader = new NetworkReader(bytes);
			HitData hitData = new HitData();
			hitData.Deserialize(reader);
			return hitData;
		}
	}
}
