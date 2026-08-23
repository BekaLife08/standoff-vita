using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Animation;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponPool : SimplePool<WeaponId, WeaponController, WeaponPool.PrefabAttr, WeaponPool.InstanceAttr>
	{
		public class PrefabAttr
		{
			public byte Id;

			public WeaponParameters Parameters;

			public WeaponAnimationParameters AnimationParameters;

			public GameObject Model;

			public WeaponMaterial FpsMaterial;

			public WeaponMaterial TpsMaterial;
		}

		public class InstanceAttr
		{
			public byte Id;

			public MeshLodGroup LodGroup;
		}

		private static readonly Log Log = Log.Create(typeof(WeaponPool));

		private readonly Transform _parent;

		private readonly Action<WeaponController> _layerChangedHandler;

		public WeaponParameters[] Weapons
		{
			get;
		}

		public WeaponPool([NotNull] WeaponParameters[] weapons, int poolSize, [NotNull] Transform parent, [NotNull] Action<WeaponController> layerChangedHandler)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (layerChangedHandler == null)
			{
				throw new ArgumentNullException("layerChangedHandler");
			}
			Weapons = weapons;
			_parent = parent;
			_layerChangedHandler = layerChangedHandler;
			Init(poolSize);
		}

		public WeaponParameters GetParameters(WeaponId weaponId)
		{
			return Prefabs[weaponId].Parameters;
		}

		protected override Dictionary<WeaponId, PrefabAttr> InitPrefabs()
		{
			Dictionary<WeaponId, PrefabAttr> dictionary = new Dictionary<WeaponId, PrefabAttr>();
			WeaponParameters[] weapons = Weapons;
			foreach (WeaponParameters weaponParameters in weapons)
			{
				GameObject gameObject = WeaponUtility.LoadModel(weaponParameters.Id);
				if (gameObject.GetComponent<MeshLodGroup>() == null)
				{
					Log.Error($"MeshLodGroup component not found {weaponParameters.name}");
					throw new InvalidOperationException();
				}
				WeaponAnimationParameters animationParameters = WeaponUtility.LoadAnimationParameters(weaponParameters.Id);
				PrefabAttr prefabAttr = new PrefabAttr();
				prefabAttr.Id = weaponParameters.NumId;
				prefabAttr.Parameters = weaponParameters;
				prefabAttr.AnimationParameters = animationParameters;
				prefabAttr.Model = gameObject;
				PrefabAttr value = prefabAttr;
				dictionary[weaponParameters.Id] = value;
			}
			return dictionary;
		}

		protected override Pair Create(WeaponId key, PrefabAttr character)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(character.Model);
			gameObject.transform.SetParent(_parent);
			MeshLodGroup component = gameObject.GetComponent<MeshLodGroup>();
			gameObject.SetActive(value: false);
			InstanceAttr instanceAttr = new InstanceAttr();
			instanceAttr.Id = character.Id;
			instanceAttr.LodGroup = component;
			InstanceAttr attr = instanceAttr;
			WeaponController weaponController = AddController(character.Parameters, gameObject);
			weaponController.PreInitialize(character.Parameters, character.AnimationParameters);
			component.LayerChanged = delegate
			{
				_layerChangedHandler(weaponController);
			};
			Pair pair = new Pair();
			pair.Instance = weaponController;
			pair.Attr = attr;
			return pair;
		}

		private static WeaponController AddController(WeaponParameters weapon, GameObject prefab)
		{
			return (WeaponController)prefab.AddComponent(WeaponTypeMap.GetControllerType(weapon));
		}

		protected override void LoadMaterial(PrefabAttr prefabAttr)
		{
		}

		protected override void UpdateMaterial(WeaponController instance, InstanceAttr attr, PrefabAttr prefabAttr)
		{
		}

		public override void ReturnToPool(WeaponController weapon)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"ReturnToPool {weapon.WeaponId}");
			}
			weapon.OnReturnToPool();
			weapon.gameObject.SetActive(value: false);
			weapon.transform.SetParent(_parent);
			base.ReturnToPool(weapon);
		}

		public void RefreshLods()
		{
			foreach (KeyValuePair<WeaponController, InstanceAttr> instanceAttr in InstanceAttrs)
			{
				instanceAttr.Value.LodGroup.RefreshLod();
			}
		}
	}
}
