using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ZoneEditorControl : EventTrigger
	{
		private RectTransform _parentTransform;

		private RectTransform _rectTransform;

		public Action<float> DragHandler
		{
			get;
			set;
		}

		public Action SelectHandler
		{
			get;
			set;
		}

		public RectTransform RectTransform
		{
			[CompilerGenerated]
			get
			{
				return _rectTransform ?? (_rectTransform = (RectTransform)base.transform);
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			Action<float> dragHandler = DragHandler;
			if (dragHandler != null)
			{
				Vector2 position = eventData.position;
				dragHandler(position.y);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			SelectHandler?.Invoke();
		}

		public void SetYPos(float y)
		{
			RectTransform rectTransform = RectTransform;
			Vector3 position = RectTransform.position;
			float x = position.x;
			Vector3 position2 = RectTransform.position;
			rectTransform.position = new Vector3(x, y, position2.z);
		}
	}
}
