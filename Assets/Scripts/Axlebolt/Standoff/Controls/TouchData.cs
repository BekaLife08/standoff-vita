using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class TouchData
	{
		public Vector3 position;

		public float time;

		public TouchPhase touchPhase;

		public int fingerId = -1;

		public int id = -1;

		public bool isAffected;

		public RectTransform interactiveArea;

		public Vector3 positionConverted
		{
			get
			{
				float num = position.x / (float)Screen.width;
				float num2 = position.y / (float)Screen.height;
				float num3 = interactiveArea.rect.yMax - interactiveArea.rect.yMin;
				float num4 = interactiveArea.rect.xMax - interactiveArea.rect.xMin;
				Vector3 a = new Vector3(num4 * num, num3 * num2, 0f);
				float num5 = num4;
				Vector2 pivot = interactiveArea.pivot;
				float x = num5 * pivot.x;
				float num6 = num3;
				Vector2 pivot2 = interactiveArea.pivot;
				Vector3 b = new Vector3(x, num6 * pivot2.y, 0f);
				return a - b;
			}
			private set
			{
			}
		}

		public TouchData Clone()
		{
			return (TouchData)MemberwiseClone();
		}
	}
}
