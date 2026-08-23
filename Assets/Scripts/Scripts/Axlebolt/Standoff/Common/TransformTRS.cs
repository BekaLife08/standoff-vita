using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class TransformTRS : MessageBase
	{
		public Vector3 pos;

		public Vector3 rot;

		public Vector3 scale;

		public TransformTRS()
		{
			pos = (rot = (scale = Vector3.zero));
		}

		public static TransformTRS FromTransform(Transform transform)
		{
			TransformTRS transformTRS = new TransformTRS();
			transformTRS.pos = transform.position;
			transformTRS.rot = transform.eulerAngles;
			transformTRS.scale = transform.localScale;
			return transformTRS;
		}

		public TransformTRS Clone()
		{
			return (TransformTRS)MemberwiseClone();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(pos);
			writer.Write(rot);
			writer.Write(scale);
		}

		public override void Deserialize(NetworkReader reader)
		{
			pos = reader.ReadVector3();
			rot = reader.ReadVector3();
			scale = reader.ReadVector3();
		}
	}
}
