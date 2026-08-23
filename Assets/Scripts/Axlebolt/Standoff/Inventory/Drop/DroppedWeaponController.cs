using System;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Main.Inventory;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Drop
{
	[RequireComponent(typeof(WeaponMap))]
	[RequireComponent(typeof(MeshLodGroup))]
	public class DroppedWeaponController : MonoBehaviour
	{
		private BoxCollider _collider;

		private Rigidbody _rigidbody;

		private WeaponMap _weaponMap;

		private WeaponDropParameters _dropParameters;

		public WeaponId WeaponId { get; private set; }

		public int DropId { get; private set; }

		public InventoryItemId SkinId { get; private set; }

		public Transform Transform { get; private set; }

		public MeshLodGroup LodGroup { get; private set; }

		public WeaponDropData DropData { get; private set; }

		public byte SlotIndex { get; private set; }

		public void PreInitialize(WeaponId weaponId, [NotNull] WeaponDropParameters dropParameters)
		{
			if (dropParameters == null)
			{
				throw new ArgumentNullException("dropParameters");
			}
			WeaponId = weaponId;
			_dropParameters = dropParameters;
			SlotIndex = WeaponId.GetSlotIndex();
			_weaponMap = this.GetRequireComponent<WeaponMap>();
			_weaponMap.Initialize();
			LodGroup = this.GetRequireComponent<MeshLodGroup>();
			LodGroup.Initialize();
			LodGroup.SetLayer(1);
			LodGroup.CombineMesh();
			base.gameObject.layer = 10;
			_collider = base.gameObject.AddComponent<BoxCollider>();
			_collider.material = dropParameters.Material;
			_rigidbody = base.gameObject.AddComponent<Rigidbody>();
			_rigidbody.isKinematic = false;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			Transform = base.transform;
		}

		public void Initialize(int dropId, InventoryItemId skinId)
		{
			DropId = dropId;
			SkinId = skinId;
		}

		public void OnReturnToPool()
		{
		}

		public void Drop(WeaponDropData dropData)
		{
			DropData = dropData;
			base.transform.position = dropData.Transform.pos;
			base.transform.eulerAngles = dropData.Transform.rot;
			base.transform.localScale = dropData.Transform.scale;
			Vector3 vector = new Vector3(dropData.Direction.x, dropData.Direction.y, dropData.Direction.z);
			vector = _dropParameters.Force * vector.normalized;
			_rigidbody.AddForce(vector);
		}
	}
}
