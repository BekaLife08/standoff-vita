using UnityEngine.Networking;

namespace Axlebolt.Networking
{
	public class ObjectSnapshot : MessageBase
	{
		public float time;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
		}

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
		}
	}
}
