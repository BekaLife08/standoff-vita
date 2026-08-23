using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ControlsCustomization : EventTrigger
	{
		private Graphic[] _graphics;

		private Vector3 _diff;

		public Action<ControlsCustomization> SelectHandler
		{
			get;
			set;
		}

		public Action<ControlsCustomization> DragHandler
		{
			get;
			set;
		}

		public Func<ControlsCustomization, float, float> PosXValidator
		{
			get;
			set;
		}

		public Func<ControlsCustomization, float, float> PosYValidator
		{
			get;
			set;
		}

		public ControlType ControlType
		{
			get;
			set;
		}

		public RectTransform RectTransform
		{
			get;
			private set;
		}

		private void Awake()
		{
			RectTransform = (RectTransform)base.transform;
			_graphics = GetComponentsInChildren<Graphic>();
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			Vector3 position = RectTransform.position;
			Vector2 position2 = eventData.position;
			float x = position2.x;
			Vector2 position3 = eventData.position;
			_diff = position - new Vector3(x, position3.y);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
			{
				worldPoint += _diff;
				worldPoint.x = PosXValidator(this, worldPoint.x);
				worldPoint.y = PosYValidator(this, worldPoint.y);
				RectTransform.position = worldPoint;
				DragHandler?.Invoke(this);
			}
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			_diff = Vector3.zero;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			SelectHandler?.Invoke(this);
		}

		public void SetAlpha(float alpha)
		{
			Graphic[] graphics = _graphics;
			foreach (Graphic graphic in graphics)
			{
				Graphic graphic2 = graphic;
				Color color = graphic.color;
				float r = color.r;
				Color color2 = graphic.color;
				float g = color2.g;
				Color color3 = graphic.color;
				graphic2.color = new Color(r, g, color3.b, alpha);
			}
		}

		public float GetAlpha()
		{
			Color color = _graphics[0].color;
			return color.a;
		}
	}
}
