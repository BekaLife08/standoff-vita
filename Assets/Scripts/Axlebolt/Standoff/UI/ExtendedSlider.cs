using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ExtendedSlider : Field<float>
	{
		[SerializeField]
		private InputField _numText;

		[SerializeField]
		private Slider _slider;

		public Slider Slider
		{
			[CompilerGenerated]
			get
			{
				return _slider;
			}
		}

		private void Awake()
		{
			_numText.text = string.Empty;
			_numText.onEndEdit.AddListener(OnNumTextChanged);
			_slider.value = _slider.minValue;
			_slider.onValueChanged.AddListener(OnSliderChanged);
		}

		protected override void SetValue(float value)
		{
			_slider.value = value;
		}

		private void OnSliderChanged(float value)
		{
			_numText.text = value.ToString("0.00");
			base.SetValue(value);
		}

		private void OnNumTextChanged(string stringValue)
		{
			if (float.TryParse(stringValue, out float result))
			{
				_slider.value = result;
			}
			else
			{
				_numText.text = result.ToString("0.00");
			}
		}
	}
}
