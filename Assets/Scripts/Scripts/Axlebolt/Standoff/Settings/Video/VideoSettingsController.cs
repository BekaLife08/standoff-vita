using I2.Loc;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Video
{
	public class VideoSettingsController : SettingsTabController<VideoSettingsController>
	{
		[SerializeField]
		private QualityLvlRow _qualityLvlTemplate;

		private QualityLvlField _shaderDetailField;

		private QualityLvlField _modelDetailField;

		private QualityLvlField _textureDetailField;

		private QualityLvlField _effectDetailField;

		private VideoSettings _model;

		public override void Init()
		{
			base.Init();
			_model = VideoSettingsManager.Instance.Get();
			_qualityLvlTemplate.Hide();
			_shaderDetailField = CreateNewQualityLvl(ScriptLocalization.VideoSettings.ShaderDetail);
			_shaderDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
			_textureDetailField = CreateNewQualityLvl(ScriptLocalization.VideoSettings.TextureDetail);
			_textureDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
			_modelDetailField = CreateNewQualityLvl(ScriptLocalization.VideoSettings.ModelDetail);
			_modelDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
			_effectDetailField = CreateNewQualityLvl(ScriptLocalization.VideoSettings.EffectDetail);
			_effectDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
		}

		public override void OnOpen()
		{
			base.OnOpen();
			_shaderDetailField.Value = _model.ShaderDetail;
			_shaderDetailField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_textureDetailField.Value = _model.TextureDetail;
			_textureDetailField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_modelDetailField.Value = _model.ModelDetail;
			_modelDetailField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
			_effectDetailField.Value = _model.EffectDetail;
			_effectDetailField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
		}

		public override void OnClose()
		{
			_shaderDetailField.ValueChangedHandler = null;
			_textureDetailField.ValueChangedHandler = null;
			_modelDetailField.ValueChangedHandler = null;
			_effectDetailField.ValueChangedHandler = null;
		}

		private QualityLvlField CreateNewQualityLvl(string label)
		{
			QualityLvlRow qualityLvlRow = Object.Instantiate(_qualityLvlTemplate, _qualityLvlTemplate.transform.parent, worldPositionStays: false);
			qualityLvlRow.LabelText.text = label;
			qualityLvlRow.Show();
			return qualityLvlRow.Field;
		}

		protected override IEnumerator ApplyInternal()
		{
			_model.ShaderDetail = _shaderDetailField.Value;
			_model.TextureDetail = _textureDetailField.Value;
			_model.ModelDetail = _modelDetailField.Value;
			_model.EffectDetail = _effectDetailField.Value;
			return VideoSettingsManager.Instance.Save(_model);
		}

		public override void ResetDefaults()
		{
			VideoSettings videoSettings = VideoSettingsManager.Instance.DefaultConfiguration();
			_shaderDetailField.Value = videoSettings.ShaderDetail;
			_textureDetailField.Value = videoSettings.TextureDetail;
			_modelDetailField.Value = videoSettings.ModelDetail;
			_effectDetailField.Value = videoSettings.EffectDetail;
		}
	}
}
