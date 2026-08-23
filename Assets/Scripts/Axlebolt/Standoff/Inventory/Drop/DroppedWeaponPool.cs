using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class DroppedWeaponPool : SimplePool<byte, DroppedWeaponController, DroppedWeaponPool.PrefabAttr, DroppedWeaponPool.InstanceAttr>
	{
		public class PrefabAttr
		{
			public byte Id;

			public GameObject Model;

			public WeaponParameters Parameters;

			public WeaponMaterial Material;
		}

		public class InstanceAttr
		{
			public byte Id;

			public MeshLodGroup LodGroup;
		}

		private static readonly Log Log = Log.Create(typeof(WeaponPool));

		private readonly Transform _parent;

		private readonly WeaponDropParameters _dropParameters;

		private readonly Dictionary<WeaponId, WeaponParameters> _weapons;

		public DroppedWeaponPool([NotNull] WeaponParameters[] weapons, int poolSize, [NotNull] WeaponDropParameters dropParameters, [NotNull] Transform parent)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			if (dropParameters == null)
			{
				throw new ArgumentNullException("dropParameters");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			_weapons = weapons.ToDictionary((WeaponParameters weapon) => weapon.Id, (WeaponParameters weapon) => weapon);
			_dropParameters = dropParameters;
			_parent = parent;
			Init(poolSize);
		}

		public WeaponParameters GetWeaponParameters(WeaponId weaponId)
		{
			return _weapons[weaponId];
		}

		protected override Dictionary<byte, PrefabAttr> InitPrefabs()
		{
			Dictionary<byte, PrefabAttr> dictionary = new Dictionary<byte, PrefabAttr>();
			foreach (WeaponParameters value2 in _weapons.Values)
			{
				GameObject gameObject = WeaponUtility.LoadModel(value2.Id);
				if (gameObject.GetComponent<MeshLodGroup>() == null)
				{
					Log.Error(string.Format("MeshLodGroup component not found {0}", value2.name));
					throw new InvalidOperationException();
				}
				PrefabAttr prefabAttr = new PrefabAttr();
				prefabAttr.Id = value2.NumId;
				prefabAttr.Parameters = value2;
				prefabAttr.Model = gameObject;
				PrefabAttr value = prefabAttr;
				dictionary[value2.NumId] = value;
			}
			return dictionary;
		}

		protected override Pair Create(byte key, PrefabAttr attr)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(attr.Model);
			gameObject.transform.SetParent(_parent);
			gameObject.name = "Dropped_" + gameObject.name;
			MeshLodGroup component = gameObject.GetComponent<MeshLodGroup>();
			gameObject.SetActive(false);
			InstanceAttr instanceAttr = new InstanceAttr();
			instanceAttr.Id = attr.Id;
			instanceAttr.LodGroup = component;
			InstanceAttr attr2 = instanceAttr;
			DroppedWeaponController droppedWeaponController = gameObject.AddComponent<DroppedWeaponController>();
			droppedWeaponController.PreInitialize(attr.Parameters.Id, _dropParameters);
			Pair pair = new Pair();
			pair.Instance = droppedWeaponController;
			pair.Attr = attr2;
			return pair;
		}

		protected override void LoadMaterial(PrefabAttr prefabAttr)
		{
			WeaponParameters parameters = prefabAttr.Parameters;
			prefabAttr.Material = WeaponUtility.LoadDiffuseMaterial(parameters.Id);
		}

		protected override void UpdateMaterial(DroppedWeaponController instance, InstanceAttr attr, PrefabAttr prefabAttr)
		{
			MeshRenderer[] meshRenderers = attr.LodGroup.MeshRenderers;
			MeshRenderer[] array = meshRenderers;
			foreach (MeshRenderer meshRenderer in array)
			{
				meshRenderer.sharedMaterials = new Material[1] { prefabAttr.Material.MainMaterial };
			}
		}

		public override void ReturnToPool(DroppedWeaponController weapon)
		{
			weapon.OnReturnToPool();
			weapon.gameObject.SetActive(false);
			weapon.transform.SetParent(_parent);
			base.ReturnToPool(weapon);
		}

		public void RefreshLods()
		{
			foreach (KeyValuePair<DroppedWeaponController, InstanceAttr> instanceAttr in InstanceAttrs)
			{
				instanceAttr.Value.LodGroup.RefreshLod();
			}
		}
	}
}
