using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class VectorAngle
	{
		public static float DeltaAngle(float ang1, float ang2)
		{
			float num = ang1 - (ang2 + 360f);
			float num2 = ang1 - ang2;
			float num3 = ang1 - (ang2 - 360f);
			if (Mathf.Abs(num) < Mathf.Abs(num2) && Mathf.Abs(num) < Mathf.Abs(num3))
			{
				return num;
			}
			if (Mathf.Abs(num2) < Mathf.Abs(num) && Mathf.Abs(num2) < Mathf.Abs(num3))
			{
				return num2;
			}
			if (Mathf.Abs(num3) < Mathf.Abs(num) && Mathf.Abs(num3) < Mathf.Abs(num2))
			{
				return num3;
			}
			return 0f;
		}

		public static Vector3 DeltaAngle3(Vector3 a, Vector3 b)
		{
			Vector3 zero = Vector3.zero;
			zero.x = DeltaAngle(a.x, b.x);
			zero.y = DeltaAngle(a.y, b.y);
			zero.z = DeltaAngle(a.z, b.z);
			return zero;
		}

		public static Vector3 LerpEulerAngle(Vector3 a, Vector3 b, float coeff)
		{
			Vector3 zero = Vector3.zero;
			zero.x = Mathf.LerpAngle(a.x, b.x, coeff);
			zero.y = Mathf.LerpAngle(a.y, b.y, coeff);
			zero.z = Mathf.LerpAngle(a.z, b.z, coeff);
			return zero;
		}

		public static TransformTR LerpTR(TransformTR a, TransformTR b, float coeff)
		{
			TransformTR transformTR = new TransformTR();
			transformTR.rot = LerpEulerAngle(a.rot, b.rot, coeff);
			transformTR.pos = Vector3.Lerp(a.pos, b.pos, coeff);
			return transformTR;
		}

		public static float AngleDirected(Vector3 a, Vector3 b, Vector3 relative)
		{
			Vector3 from = Vector3.Cross(a, b);
			return Vector3.Angle(a, b) * (float)((Vector3.Angle(from, relative) > 90f) ? 1 : (-1));
		}

		public static float AngleDirected(Vector2 a, Vector2 b)
		{
			Vector3 lhs = new Vector3(a.x, 0f, a.y);
			Vector3 rhs = new Vector3(b.x, 0f, b.y);
			Vector3 from = Vector3.Cross(lhs, rhs);
			return Vector2.Angle(a, b) * (float)((Vector3.Angle(from, Vector3.up) > 90f) ? 1 : (-1));
		}

		public static void ApplyTransformLocalTR(Transform targetTransform, TransformTRS source)
		{
			targetTransform.localPosition = source.pos;
			targetTransform.localEulerAngles = source.rot;
		}

		public static void ApplyLocalTRTransform(Transform targetTransform, TransformTRS source)
		{
			source.pos = targetTransform.localPosition;
			source.rot = targetTransform.localEulerAngles;
		}
	}
}
