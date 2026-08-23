using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public class BulletCastData : MessageBase
	{
		public Vector3 StartPosition;

		public Vector3 Direction;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(StartPosition);
			writer.Write(Direction);
		}

		public override void Deserialize(NetworkReader reader)
		{
			StartPosition = reader.ReadVector3();
			Direction = reader.ReadVector3();
		}
	}
}
