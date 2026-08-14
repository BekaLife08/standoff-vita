using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class LodUtility
	{
		public static bool IsLod(Transform transform, int layer, int level)
		{
			if (!(transform.name == "L" + layer + "_LOD" + level) && layer == 0)
			{
				return transform.name == "LOD" + level;
			}
			return true;
		}

		public static bool IsLod(Transform transform)
		{
			return transform.name.StartsWith("L");
		}

		public static Transform GetLod(Transform parent, int layer, int level, bool combined = false)
		{
			string text = "L" + layer + "_LOD" + level;
			if (combined)
			{
				text += "_C";
			}
			Transform transform = parent.Find(text);
			if (transform == null && layer == 0)
			{
				text = "LOD" + level;
				if (combined)
				{
					text += "_C";
				}
				transform = parent.Find(text);
			}
			return transform;
		}
	}
}
