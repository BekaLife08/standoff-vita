using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class FloatingJoystick : InteractableZone
	{
		public enum TouchState
		{
			TouchFixed,
			NotFixed
		}

		public JoysticType type = JoysticType.FreeTouch;

		public RectTransform touchTriggerArea;

		public RectTransform joystickCurcleRect;

		public RectTransform joystickPointerRect;

		private float radius;

		private Vector3 triggerAreaCenter;

		private float triggerAreaRadius;

		private bool isInitialized;

		private TouchState touchState = TouchState.NotFixed;

		private Vector3 curJoystickPos;

		private Vector3 startJoystickPos;

		private Vector3 curTouchPosition;

		public Vector3 JoystickAxis
		{
			get
			{
				if (radius <= 1E-05f)
				{
					return Vector3.zero;
				}
				Vector3 a = curTouchPosition - curJoystickPos;
				float num = Mathf.Clamp01(a.magnitude / radius);
				a.Normalize();
				if (num < 0.3f)
				{
					num = 0f;
				}
				return a * num;
			}
		}

		public void Initialize()
		{
			float num = joystickCurcleRect.rect.yMax - joystickCurcleRect.rect.yMin;
			float num2 = joystickCurcleRect.rect.xMax - joystickCurcleRect.rect.xMin;
			radius = Mathf.Min(num, num2) * 0.5f;
			Vector3[] array = new Vector3[4];
			touchTriggerArea.GetWorldCorners(array);
			triggerAreaCenter = interactiveArea.InverseTransformPoint((array[0] + array[2]) * 0.5f);
			triggerAreaRadius = Mathf.Min(touchTriggerArea.rect.xMax - touchTriggerArea.rect.xMin, touchTriggerArea.rect.yMax - touchTriggerArea.rect.yMin) * 0.5f;
			if (type == JoysticType.Fixed)
			{
				float num3 = num2;
				Vector2 pivot = joystickCurcleRect.pivot;
				float x = num3 * pivot.x;
				float num4 = num;
				Vector2 pivot2 = joystickCurcleRect.pivot;
				Vector3 b = new Vector3(x, num4 * pivot2.y, 0f);
				Vector3 position = joystickCurcleRect.TransformPoint(new Vector3(num2 * 0.5f, num * 0.5f, 0f) - b);
				startJoystickPos = interactiveArea.InverseTransformPoint(position);
				joystickCurcleRect.gameObject.SetActive(value: true);
			}
			isInitialized = true;
		}

		private void SetJoystickCurclePosition(Vector3 position)
		{
			Vector3 localPosition = joystickCurcleRect.parent.InverseTransformPoint(interactiveArea.TransformPoint(position));
			joystickCurcleRect.localPosition = localPosition;
		}

		private void SetJoystickPointerPosition(Vector3 position)
		{
			Vector3 localPosition = joystickPointerRect.parent.InverseTransformPoint(interactiveArea.TransformPoint(position));
			joystickPointerRect.localPosition = localPosition;
		}

		private bool IsInTriggerArea(Vector3 point)
		{
			Vector3[] array = new Vector3[4];
			touchTriggerArea.GetWorldCorners(array);
			Vector3 vector = interactiveArea.InverseTransformPoint(array[0]);
			Vector3 vector2 = interactiveArea.InverseTransformPoint(array[2]);
			return vector.x <= point.x && point.x <= vector2.x && vector.y <= point.y && point.y <= vector2.y;
		}

		public override void OnTouchDown(TouchData touchData)
		{
			if (!isInitialized)
			{
				Initialize();
			}
			Vector3 positionConverted = touchData.positionConverted;
			if (IsInTriggerArea(positionConverted))
			{
				if (type == JoysticType.Floating || type == JoysticType.FreeTouch)
				{
					curJoystickPos = positionConverted;
				}
				if (type == JoysticType.Fixed)
				{
					curJoystickPos = startJoystickPos;
				}
				touchState = TouchState.TouchFixed;
				if (type != 0)
				{
					joystickCurcleRect.gameObject.SetActive(value: true);
				}
				joystickPointerRect.gameObject.SetActive(value: true);
				OnTouchStayActive(touchData);
			}
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			if (touchState != 0)
			{
				return;
			}
			Vector3 positionConverted = touchData.positionConverted;
			Vector3 vector = positionConverted - curJoystickPos;
			Vector3 vector2 = triggerAreaCenter - positionConverted;
			if (type == JoysticType.Floating)
			{
				if (vector2.magnitude < triggerAreaRadius + radius)
				{
					if (vector.magnitude > radius)
					{
						curJoystickPos = positionConverted - vector.normalized * radius;
						Vector3 vector3 = curJoystickPos - triggerAreaCenter;
						if (vector3.magnitude > triggerAreaRadius)
						{
							curJoystickPos = triggerAreaCenter + vector3.normalized * triggerAreaRadius;
						}
					}
				}
				else
				{
					curJoystickPos = triggerAreaCenter - vector2.normalized * triggerAreaRadius;
				}
			}
			if (type == JoysticType.FreeTouch)
			{
			}
			if (type == JoysticType.Fixed)
			{
				curJoystickPos = startJoystickPos;
			}
			curTouchPosition = positionConverted;
			SetJoystickPointerPosition(JoystickAxis * radius + curJoystickPos);
			SetJoystickCurclePosition(curJoystickPos);
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			if (touchState == TouchState.TouchFixed)
			{
				touchState = TouchState.NotFixed;
				if (type != 0)
				{
					joystickCurcleRect.gameObject.SetActive(value: false);
				}
				joystickPointerRect.gameObject.SetActive(value: false);
				curTouchPosition = (curJoystickPos = Vector3.zero);
			}
		}

		public void SetJoystickType(JoysticType joysticType)
		{
			type = joysticType;
			isInitialized = false;
		}
	}
}
