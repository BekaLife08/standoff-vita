using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Controls
{
	public class ControlsCustomizationField : Field<float>
	{
		[SerializeField]
		private Slider _slider;

		[SerializeField]
		private bool _isAbove;

		[SerializeField]
		private float _offset;

		private RectTransform _rectTransform;

		private RectTransform _fullTransform;

		private RectTransform _bindTransform;

		private void Awake()
		{
			_rectTransform = (RectTransform)base.transform;
			_fullTransform = (RectTransform)base.transform.parent;
			_slider.onValueChanged.AddListener(delegate(float value)
			{
				base.SetValue(value);
			});
			Hide();
		}

		protected override void SetValue(float value)
		{
			_slider.value = value;
		}

		public void BindTo([NotNull] RectTransform rectTransform)
		{
			if (rectTransform == null)
			{
				throw new ArgumentNullException("rectTransform");
			}
			_bindTransform = rectTransform;
			Show();
			UpdatePosition();
		}

		public void Unbind()
		{
			_bindTransform = null;
			Hide();
		}

		public void UpdatePosition()
		{
			if (!(_bindTransform == null))
			{
				int num = _isAbove ? 1 : (-1);
				float num2 = (float)(num * Screen.height) / _fullTransform.rect.height;
				Vector3 b = new Vector3(0f, num2 * (_bindTransform.rect.height / 2f + _rectTransform.rect.height / 2f + _offset), 0f);
				_rectTransform.position = _bindTransform.position + b;
			}
		}
	}
}
