using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class TouchHandler : MonoBehaviour
	{
		public RectTransform interactiveArea;

		public List<InteractableZone> interactableZoneList = new List<InteractableZone>();

		private List<TouchData> touchDataList = new List<TouchData>();

		private List<TouchData> newTouchDataList = new List<TouchData>();

		private TouchData mouseTouch;

		private int nextTouchId;

		public void RegisterInteractableZone(InteractableZone interactabeZone)
		{
			interactableZoneList.Add(interactabeZone);
			interactableZoneList.Sort(CompareTo);
		}

		public void Initialize()
		{
			if (interactiveArea == null)
			{
				UnityEngine.Debug.LogError("No InteractiveArea assigned");
			}
			else
			{
				interactableZoneList.Sort(CompareTo);
			}
		}

		private int GetTouchIndex(int fingerIndex)
		{
			for (int i = 0; i < touchDataList.Count; i++)
			{
				if (touchDataList[i].fingerId == fingerIndex)
				{
					return i;
				}
			}
			return -1;
		}

		private TouchData GetTouchData(int touchId)
		{
			foreach (TouchData touchData in touchDataList)
			{
				if (touchData.id == touchId)
				{
					return touchData;
				}
			}
			return null;
		}

		private int CompareTo(InteractableZone zoneA, InteractableZone zoneB)
		{
			if (zoneA.layer < zoneB.layer)
			{
				return 1;
			}
			if (zoneA.layer > zoneB.layer)
			{
				return -1;
			}
			return 0;
		}

		private void UpdateTouch(TouchData touchData, Touch touch)
		{
			touchData.position = touch.position;
			touchData.touchPhase = touch.phase;
		}

		private void AddTouch(Touch touch, List<TouchData> touchDataList)
		{
			TouchData touchData = new TouchData();
			touchData.fingerId = touch.fingerId;
			touchData.id = nextTouchId;
			nextTouchId++;
			touchData.time = Time.time;
			touchData.position = touch.position;
			touchData.touchPhase = touch.phase;
			touchData.interactiveArea = interactiveArea;
			touchDataList.Add(touchData);
		}

		public TouchData CreateMouseTouch()
		{
			TouchData touchData = new TouchData();
			touchData.id = nextTouchId;
			nextTouchId++;
			touchData.time = Time.time;
			touchData.position = UnityEngine.Input.mousePosition;
			touchData.touchPhase = TouchPhase.Began;
			touchData.interactiveArea = interactiveArea;
			return touchData;
		}

		private void HandleTouches()
		{
			List<TouchData> list = new List<TouchData>();
			newTouchDataList.Clear();
			for (int i = 0; i < UnityEngine.Input.touchCount; i++)
			{
				Touch touch = UnityEngine.Input.GetTouch(i);
				int touchIndex = GetTouchIndex(touch.fingerId);
				if (touchIndex != -1)
				{
					UpdateTouch(touchDataList[touchIndex], touch);
					list.Add(touchDataList[touchIndex]);
				}
				else
				{
					AddTouch(touch, list);
					newTouchDataList.Add(list[list.Count - 1]);
				}
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
			{
				if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
				{
					if (Input.GetMouseButtonDown(0))
					{
						mouseTouch = CreateMouseTouch();
						list.Add(mouseTouch);
						newTouchDataList.Add(list[list.Count - 1]);
					}
					else if ((Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) && mouseTouch != null)
					{
						mouseTouch.position = UnityEngine.Input.mousePosition;
						mouseTouch.touchPhase = ((!Input.GetMouseButtonUp(0)) ? TouchPhase.Moved : TouchPhase.Ended);
						list.Add(mouseTouch);
					}
				}
				else
				{
					mouseTouch = null;
				}
				if (mouseTouch == null)
				{
				}
			}
			touchDataList = list;
		}

		private void HandleInteractableZones()
		{
			foreach (InteractableZone interactableZone in interactableZoneList)
			{
				if (interactableZone.IsActive && interactableZone.HasActiveTouch())
				{
					TouchData touchData = GetTouchData(interactableZone.activeTouchDataId);
					if (touchData != null)
					{
						interactableZone.UpdateTouchData(touchData);
					}
					else
					{
						interactableZone.RemoveActiveTouchData();
					}
				}
			}
			foreach (InteractableZone interactableZone2 in interactableZoneList)
			{
				if (interactableZone2.IsActive && !interactableZone2.HasActiveTouch())
				{
					foreach (TouchData newTouchData in newTouchDataList)
					{
						if (!newTouchData.isAffected && interactableZone2.IsCoveredByTouchArea(newTouchData))
						{
							interactableZone2.SetTouchData(newTouchData);
							break;
						}
					}
				}
			}
		}

		public void Handle()
		{
			HandleTouches();
			HandleInteractableZones();
		}
	}
}
