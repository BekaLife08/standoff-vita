using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class WeaponTakeData : MessageBase
	{
		public int DropId;

		public int ViewId;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)DropId);
			writer.WritePackedUInt32((uint)ViewId);
		}

		public override void Deserialize(NetworkReader reader)
		{
			DropId = (int)reader.ReadPackedUInt32();
			ViewId = (int)reader.ReadPackedUInt32();
		}
	}
}
