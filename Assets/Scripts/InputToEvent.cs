using UnityEngine;

public class InputToEvent : MonoBehaviour
{
	private GameObject lastGo;

	public static Vector3 inputHitPos;

	public bool DetectPointedAtGameObject;

	private Vector2 pressedPosition = Vector2.zero;

	private Vector2 currentPos = Vector2.zero;

	public bool Dragging;

	private Camera m_Camera;

	public static GameObject goPointedAt
	{
		get;
		private set;
	}

	public Vector2 DragVector => (!Dragging) ? Vector2.zero : (currentPos - pressedPosition);

	private void Start()
	{
		m_Camera = GetComponent<Camera>();
	}

	private void Update()
	{
		if (DetectPointedAtGameObject)
		{
			goPointedAt = RaycastObject(UnityEngine.Input.mousePosition);
		}
		if (UnityEngine.Input.touchCount > 0)
		{
			Touch touch = UnityEngine.Input.GetTouch(0);
			currentPos = touch.position;
			if (touch.phase == TouchPhase.Began)
			{
				Press(touch.position);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				Release(touch.position);
			}
			return;
		}
		currentPos = UnityEngine.Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			Press(UnityEngine.Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			Release(UnityEngine.Input.mousePosition);
		}
		if (Input.GetMouseButtonDown(1))
		{
			pressedPosition = UnityEngine.Input.mousePosition;
			lastGo = RaycastObject(pressedPosition);
			if (lastGo != null)
			{
				lastGo.SendMessage("OnPressRight", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void Press(Vector2 screenPos)
	{
		pressedPosition = screenPos;
		Dragging = true;
		lastGo = RaycastObject(screenPos);
		if (lastGo != null)
		{
			lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Release(Vector2 screenPos)
	{
		if (lastGo != null)
		{
			GameObject x = RaycastObject(screenPos);
			if (x == lastGo)
			{
				lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
			lastGo = null;
		}
		pressedPosition = Vector2.zero;
		Dragging = false;
	}

	private GameObject RaycastObject(Vector2 screenPos)
	{
		if (Physics.Raycast(m_Camera.ScreenPointToRay(screenPos), out RaycastHit hitInfo, 200f))
		{
			inputHitPos = hitInfo.point;
			return hitInfo.collider.gameObject;
		}
		return null;
	}
}
