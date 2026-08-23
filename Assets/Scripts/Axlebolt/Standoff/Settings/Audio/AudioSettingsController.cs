using Axlebolt.Standoff.UI;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Audio
{
	public class AudioSettingsController : SettingsTabController<AudioSettingsController>
	{
		[SerializeField]
		private ExtendedSlider _volumeField;

		private AudioSettings _model;

		public override void OnOpen()
		{
			base.OnOpen();
			_model = AudioSettingsManager.Instance.Model;
			_volumeField.Slider.minValue = 0f;
			_volumeField.Slider.maxValue = 1f;
			_volumeField.Value = _model.Volume;
			_volumeField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
		}

		protected override IEnumerator ApplyInternal()
		{
			_model.Volume = _volumeField.Value;
			yield return AudioSettingsManager.Instance.Save(_model);
		}

		public override void ResetDefaults()
		{
			AudioSettings audioSettings = AudioSettingsManager.Instance.DefaultValue();
			_volumeField.Value = audioSettings.Volume;
		}

		public override void OnClose()
		{
			_volumeField.ValueChangedHandler = null;
		}
	}
}
