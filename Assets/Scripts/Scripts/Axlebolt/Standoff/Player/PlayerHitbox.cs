using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.HitHandling;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerHitbox : MonoBehaviour
	{
		private PlayerHitboxConfig.HitboxConfig _hitboxConfig;

		private Collider _trigger;

		public PlayerHitController PlayerHitController
		{
			get;
			set;
		}

		public BipedMap.Bip Bone
		{
			[CompilerGenerated]
			get
			{
				return _hitboxConfig.bone;
			}
		}

		public void Initialize([NotNull] PlayerHitboxConfig.HitboxConfig config, int layer)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			_hitboxConfig = config;
			PlayerHitboxConfigurator.ApplyHitboxConfig(base.gameObject, _hitboxConfig, layer);
			base.gameObject.tag = SurfaceTypeUtility.GetTag(SurfaceType.Character);
			_trigger = GetComponent<Collider>();
			base.gameObject.layer = LayerMask.NameToLayer("PlayerHitbox");
		}

		public void Enable()
		{
			_trigger.enabled = true;
		}

		public void Disable()
		{
			_trigger.enabled = false;
		}

		public void Hit(HitData hitData)
		{
			PlayerHitController.Hit(hitData);
		}
	}
}
