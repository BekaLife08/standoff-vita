using Axlebolt.Networking;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimSnapshot : ObjectSnapshot
	{
		public MecanimSyncData mecanimSyncData = new MecanimSyncData();

		public override void Deserialize(NetworkReader reader)
		{
			uint position = reader.Position;
			mecanimSyncData.Deserialize(reader);
			mecanimSyncData.time = time;
		}

		public override void Serialize(NetworkWriter writer)
		{
			mecanimSyncData.Serialize(writer);
		}

		public MecanimSnapshot Clone()
		{
			MecanimSnapshot mecanimSnapshot = (MecanimSnapshot)MemberwiseClone();
			mecanimSnapshot.mecanimSyncData = mecanimSyncData.Clone();
			return mecanimSnapshot;
		}
	}
}
