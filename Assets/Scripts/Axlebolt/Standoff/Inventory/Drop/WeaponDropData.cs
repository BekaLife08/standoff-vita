using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Main.Inventory;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class WeaponDropData : MessageBase
	{
		public int DropId;

		public WeaponId WeaponId;

		public InventoryItemId SkinId;

		public TransformTRS Transform;

		public Vector3 Direction;

		public int ViewId;

		public short MagazineCapacity;

		public short Capacity;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)DropId);
			writer.Write((int)WeaponId);
			writer.Write((int)SkinId);
			GeneratedNetworkCode._WriteTransformTRS_None(writer, Transform);
			writer.Write(Direction);
			writer.WritePackedUInt32((uint)ViewId);
			writer.WritePackedUInt32((uint)MagazineCapacity);
			writer.WritePackedUInt32((uint)Capacity);
		}

		public override void Deserialize(NetworkReader reader)
		{
			DropId = (int)reader.ReadPackedUInt32();
			WeaponId = (WeaponId)reader.ReadInt32();
			SkinId = (InventoryItemId)reader.ReadInt32();
			Transform = GeneratedNetworkCode._ReadTransformTRS_None(reader);
			Direction = reader.ReadVector3();
			ViewId = (int)reader.ReadPackedUInt32();
			MagazineCapacity = (short)reader.ReadPackedUInt32();
			Capacity = (short)reader.ReadPackedUInt32();
		}
	}
}
