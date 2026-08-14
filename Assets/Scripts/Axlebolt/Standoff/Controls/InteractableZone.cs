using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class InteractableZone : MonoBehaviour
	{
		public int layer;

		public bool blockTouch = true;

		public bool areaActiveOnly = true;

		public RectTransform interactiveZone;

		[HideInInspector]
		public int activeTouchDataId = -1;

		protected TouchData touchData;

		protected RectTransform interactiveArea;

		public Image img;

		public bool IsActive
		{
			get;
			set;
		} = true;


		protected virtual void Awake()
		{
			if (interactiveZone == null)
			{
				UnityEngine.Debug.LogError("No Rect Transform component found");
			}
		}

		internal void UpdateTouchData(TouchData touchData)
		{
			this.touchData = touchData;
			if (touchData.touchPhase == TouchPhase.Canceled || touchData.touchPhase == TouchPhase.Ended)
			{
				RemoveActiveTouchData();
			}
			else if (areaActiveOnly && !IsCoveredByTouchArea(touchData))
			{
				RemoveActiveTouchData();
			}
			else
			{
				OnTouchStayActive(touchData);
			}
		}

		internal void RemoveActiveTouchData()
		{
			if (touchData != null)
			{
				OnTouchEnd(touchData);
				touchData = null;
				activeTouchDataId = -1;
			}
		}

		internal bool IsCoveredByTouchArea(TouchData touchData)
		{
			Vector3[] array = new Vector3[4];
			interactiveZone.GetWorldCorners(array);
			Vector3 vector = touchData.interactiveArea.InverseTransformPoint(array[0]);
			Vector3 vector2 = touchData.interactiveArea.InverseTransformPoint(array[2]);
			Vector3 positionConverted = touchData.positionConverted;
			if (vector.x <= positionConverted.x && positionConverted.x <= vector2.x && vector.y <= positionConverted.y && positionConverted.y <= vector2.y)
			{
				return true;
			}
			return false;
		}

		internal bool HasActiveTouch()
		{
			if (touchData == null)
			{
				return false;
			}
			return true;
		}

		internal void SetTouchData(TouchData touchData)
		{
			this.touchData = touchData;
			touchData.isAffected = blockTouch;
			interactiveArea = touchData.interactiveArea;
			activeTouchDataId = touchData.id;
			OnTouchDown(this.touchData);
		}

		public virtual void OnTouchDown(TouchData touchData)
		{
		}

		public virtual void OnTouchStayActive(TouchData touchData)
		{
		}

		public virtual void OnTouchEnd(TouchData touchData)
		{
		}
	}
}
