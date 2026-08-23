using Axlebolt.Standoff.Main.Inventory;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponIdentity : MessageBase
	{
		public WeaponId WeaponId;

		public int InstanceId;

		public InventoryItemId SkinId;

		public WeaponIdentity(WeaponId weaponId, int instanceId, InventoryItemId skinId)
		{
			WeaponId = weaponId;
			InstanceId = instanceId;
			SkinId = skinId;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((int)WeaponId);
			writer.WritePackedUInt32((uint)InstanceId);
			writer.Write((int)SkinId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			WeaponId = (WeaponId)reader.ReadInt32();
			InstanceId = (int)reader.ReadPackedUInt32();
			SkinId = (InventoryItemId)reader.ReadInt32();
		}
	}
}
