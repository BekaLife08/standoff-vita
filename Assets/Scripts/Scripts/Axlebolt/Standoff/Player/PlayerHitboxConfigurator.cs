using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerHitboxConfigurator : MonoBehaviour
	{
		[HideInInspector]
		public BipedMap bipedMap;

		public PlayerHitboxConfig hitboxConfigs;

		public int layerIndex;

		public CapsuleCollider triggerCollider;

		private void SaveBoneConfig(BipedMap.Bip bone)
		{
			Collider component = bipedMap.GetBone(bone).gameObject.GetComponent<Collider>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("No Collider Attached To " + bone.ToString());
				return;
			}
			PlayerHitboxConfig.HitboxConfig hitboxConfig = new PlayerHitboxConfig.HitboxConfig();
			hitboxConfig.bone = bone;
			if (component is BoxCollider)
			{
				BoxCollider boxCollider = (BoxCollider)component;
				hitboxConfig.center = boxCollider.center;
				hitboxConfig.size = boxCollider.size;
				hitboxConfig.hitboxType = PlayerHitboxConfig.HitboxType.Box;
			}
			if (component is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)component;
				hitboxConfig.center = capsuleCollider.center;
				hitboxConfig.direction = capsuleCollider.direction;
				hitboxConfig.height = capsuleCollider.height;
				hitboxConfig.radius = capsuleCollider.radius;
				hitboxConfig.hitboxType = PlayerHitboxConfig.HitboxType.Capsule;
			}
			hitboxConfigs.hitboxes.Add(hitboxConfig);
		}

		public void SaveConfig()
		{
			bipedMap = GetComponent<BipedMap>();
			hitboxConfigs.hitboxes.Clear();
			SaveBoneConfig(BipedMap.Bip.Head);
			SaveBoneConfig(BipedMap.Bip.Hip);
			SaveBoneConfig(BipedMap.Bip.LeftCalf);
			SaveBoneConfig(BipedMap.Bip.LeftThigh);
			SaveBoneConfig(BipedMap.Bip.LeftFoot);
			SaveBoneConfig(BipedMap.Bip.RightCalf);
			SaveBoneConfig(BipedMap.Bip.RightThigh);
			SaveBoneConfig(BipedMap.Bip.RightFoot);
			SaveBoneConfig(BipedMap.Bip.Spine1);
			SaveBoneConfig(BipedMap.Bip.Spine2);
			SaveBoneConfig(BipedMap.Bip.Neck);
			SaveBoneConfig(BipedMap.Bip.LeftShoulder);
			SaveBoneConfig(BipedMap.Bip.LeftUpperarm);
			SaveBoneConfig(BipedMap.Bip.LeftForearm);
			SaveBoneConfig(BipedMap.Bip.LeftHand);
			SaveBoneConfig(BipedMap.Bip.RightShoulder);
			SaveBoneConfig(BipedMap.Bip.RightUpperarm);
			SaveBoneConfig(BipedMap.Bip.RightForearm);
			SaveBoneConfig(BipedMap.Bip.RightHand);
			SaveBoneConfig(BipedMap.Bip.Head);
			hitboxConfigs.layer = layerIndex;
			hitboxConfigs.trigger.center = triggerCollider.center;
			hitboxConfigs.trigger.direction = triggerCollider.direction;
			hitboxConfigs.trigger.radius = triggerCollider.radius;
			hitboxConfigs.trigger.height = triggerCollider.height;
		}

		public void RestoreData()
		{
			bipedMap = GetComponent<BipedMap>();
			if (hitboxConfigs == null)
			{
				UnityEngine.Debug.LogError("Hitbox Config Not Set");
				return;
			}
			foreach (PlayerHitboxConfig.HitboxConfig hitbox in hitboxConfigs.hitboxes)
			{
				GameObject gameObject = bipedMap.GetBone(hitbox.bone).gameObject;
				ApplyHitboxConfig(gameObject, hitbox, layerIndex);
			}
			triggerCollider.center = hitboxConfigs.trigger.center;
			triggerCollider.direction = hitboxConfigs.trigger.direction;
			triggerCollider.radius = hitboxConfigs.trigger.radius;
			triggerCollider.height = hitboxConfigs.trigger.height;
		}

		public static void ApplyHitboxConfig(GameObject bone, PlayerHitboxConfig.HitboxConfig config, int layerIndex)
		{
			Collider[] components = bone.GetComponents<Collider>();
			Collider[] array = components;
			foreach (Collider obj in array)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			Collider collider = null;
			if (config.hitboxType == PlayerHitboxConfig.HitboxType.Box)
			{
				BoxCollider boxCollider = bone.AddComponent<BoxCollider>();
				boxCollider.center = config.center;
				boxCollider.size = config.size;
				collider = boxCollider;
			}
			if (config.hitboxType == PlayerHitboxConfig.HitboxType.Capsule)
			{
				CapsuleCollider capsuleCollider = bone.AddComponent<CapsuleCollider>();
				capsuleCollider.center = config.center;
				capsuleCollider.direction = config.direction;
				capsuleCollider.radius = config.radius;
				capsuleCollider.height = config.height;
				collider = capsuleCollider;
			}
			collider.enabled = false;
			collider.isTrigger = true;
			collider.gameObject.layer = layerIndex;
		}
	}
}
