using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class TransformTR : MessageBase
	{
		public Vector3 pos;

		public Vector3 rot;

		public TransformTR()
		{
			pos = (rot = Vector3.zero);
		}

		public TransformTR Clone()
		{
			return (TransformTR)MemberwiseClone();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(pos);
			writer.Write(rot);
		}

		public override void Deserialize(NetworkReader reader)
		{
			pos = reader.ReadVector3();
			rot = reader.ReadVector3();
		}
	}
}
