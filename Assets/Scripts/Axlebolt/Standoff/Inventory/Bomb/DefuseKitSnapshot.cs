using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class DefuseKitSnapshot : WeaponSnapshot
	{
		public DefuseKitState State;

		public float Progress;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((int)State);
			writer.Write(Progress);
		}

		public override void Deserialize(NetworkReader reader)
		{
			State = (DefuseKitState)reader.ReadInt32();
			Progress = reader.ReadSingle();
		}
	}
}
