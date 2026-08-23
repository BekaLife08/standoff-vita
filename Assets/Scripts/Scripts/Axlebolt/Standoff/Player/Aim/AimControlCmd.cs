using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Aim
{
	public class AimControlCmd : MessageBase
	{
		public AimType AimType;

		public Vector3 aimEulerAngles;

		public Vector2 DeltaAimAngles;

		public override void Deserialize(NetworkReader reader)
		{
			AimType = (AimType)reader.ReadByte();
			aimEulerAngles = reader.ReadVector3();
			DeltaAimAngles = reader.ReadVector2();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((byte)AimType);
			writer.Write(aimEulerAngles);
			writer.Write(DeltaAimAngles);
		}
	}
}
