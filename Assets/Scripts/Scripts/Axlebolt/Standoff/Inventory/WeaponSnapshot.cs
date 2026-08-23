using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public abstract class WeaponSnapshot : MessageBase
	{
		public float Time;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Time);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Time = reader.ReadSingle();
		}
	}
}
