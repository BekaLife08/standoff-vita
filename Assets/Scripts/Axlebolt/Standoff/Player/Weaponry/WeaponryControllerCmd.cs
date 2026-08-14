using Axlebolt.Standoff.Inventory;
using Unity;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Weaponry
{
	public class WeaponryControllerCmd : MessageBase
	{
		public WeaponControllerCmd WeaponControllerCmd;

		public int SlotIndex = -1;

		public bool ToDrop;

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteWeaponControllerCmd_None(writer, WeaponControllerCmd);
			writer.WritePackedUInt32((uint)SlotIndex);
			writer.Write(ToDrop);
		}

		public override void Deserialize(NetworkReader reader)
		{
			WeaponControllerCmd = GeneratedNetworkCode._ReadWeaponControllerCmd_None(reader);
			SlotIndex = (int)reader.ReadPackedUInt32();
			ToDrop = reader.ReadBoolean();
		}
	}
}
