using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ZoneEditor : Field<float>
	{
		[SerializeField]
		private ZoneEditorControl _zoneEditorControl;

		[SerializeField]
		private RectTransform _horizontalLine;

		[SerializeField]
		private float _minValue;

		[SerializeField]
		private float _maxValue;

		public ZoneEditorControl ZoneEditorControl
		{
			[CompilerGenerated]
			get
			{
				return _zoneEditorControl;
			}
		}

		private void Awake()
		{
			_zoneEditorControl.DragHandler = OnDragHandler;
			_zoneEditorControl.gameObject.SetActive(value: false);
		}

		private void OnDragHandler(float y)
		{
			base.Value = y / (float)Screen.height;
		}

		protected override void SetValue(float value)
		{
			if (!(value < _minValue) && !(value > _maxValue))
			{
				base.SetValue(value);
				RectTransform horizontalLine = _horizontalLine;
				Vector2 anchorMin = _horizontalLine.anchorMin;
				horizontalLine.anchorMin = new Vector2(anchorMin.x, value);
				_zoneEditorControl.SetYPos(value * (float)Screen.height);
				_zoneEditorControl.gameObject.SetActive(value: true);
			}
		}
	}
}
