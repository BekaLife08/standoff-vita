using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class ItemSelectArea : InteractableZone
	{
		[Serializable]
		public class Sector
		{
			public int SlotIndex;

			public int StartAngle;

			public int EndAngle;
		}

		public delegate void OnItemSelect(int itemIndex);

		public delegate void OnTouchEvent();

		public float TriggerRadius;

		private Vector3 _touchPoint;

		private int _lastSelectedItem = -1;

		public List<Sector> sectors = new List<Sector>();

		public event OnItemSelect onItemSelect = delegate
		{
		};

		public event OnItemSelect onItemSelected = delegate
		{
		};

		public event OnTouchEvent onTouchDownEvent = delegate
		{
		};

		public void SetItem()
		{
		}

		public void AddItem(int startAngle, int endAngle)
		{
			Sector sector = new Sector();
			sector.StartAngle = startAngle;
			sector.EndAngle = endAngle;
			sectors.Add(sector);
		}

		public void Test(int k)
		{
		}

		private void HandleTouch(bool isTouchEnd, TouchData touchData)
		{
			if (isTouchEnd)
			{
				_lastSelectedItem = -1;
			}
			if (Vector3.Distance(_touchPoint, touchData.positionConverted) > TriggerRadius)
			{
				float clockwiseAngle = GetClockwiseAngle(new Vector3(0f, 1f, 0f), touchData.positionConverted - _touchPoint);
				foreach (Sector sector in sectors)
				{
					int slotIndex = sector.SlotIndex;
					if (isInSector(sector, clockwiseAngle) && slotIndex != _lastSelectedItem)
					{
						if (!isTouchEnd)
						{
							this.onItemSelect(slotIndex);
						}
						else
						{
							this.onItemSelected(slotIndex);
						}
						_lastSelectedItem = slotIndex;
						break;
					}
				}
			}
			else if (_lastSelectedItem != 0)
			{
				if (!isTouchEnd)
				{
					this.onItemSelect(0);
				}
				else
				{
					this.onItemSelected(0);
				}
				_lastSelectedItem = 0;
			}
		}

		private bool isInSector(Sector sector, float angle)
		{
			float num = sector.StartAngle;
			float num2 = sector.EndAngle;
			if (num2 < num)
			{
				if (num < angle || angle < num2)
				{
					return true;
				}
				return false;
			}
			return num < angle && angle < num2;
		}

		public override void OnTouchDown(TouchData touchData)
		{
			_touchPoint = touchData.positionConverted;
			_lastSelectedItem = -1;
			this.onTouchDownEvent();
			base.OnTouchDown(touchData);
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			HandleTouch(false, touchData);
			base.OnTouchStayActive(touchData);
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			HandleTouch(true, touchData);
			_lastSelectedItem = -1;
			base.OnTouchEnd(touchData);
		}

		private float GetClockwiseAngle(Vector3 start, Vector3 end)
		{
			float num = Vector3.Angle(start, end);
			Vector3 to = new Vector3(1f, 0f, 0f);
			if (Vector3.Angle(end, to) > 90f)
			{
				num *= -1f;
			}
			return num;
		}
	}
}
