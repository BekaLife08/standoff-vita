using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Video
{
	public class VideoSettingsManager
	{
		public delegate void EventHandler();

		private static readonly Log Log = Log.Create(typeof(VideoSettingsManager));

		private readonly string _settingsKey = "VideoSettings";

		private static VideoSettingsManager _instance;

		private VideoSettings _videoSettings;

		private readonly IFileStorage _storage;

		public static VideoSettingsManager Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		public QualityLvl ModelDetail
		{
			[CompilerGenerated]
			get
			{
				return _videoSettings.ModelDetail;
			}
		}

		public QualityLvl ShaderDetail
		{
			[CompilerGenerated]
			get
			{
				return _videoSettings.ShaderDetail;
			}
		}

		public QualityLvl TextureDetail
		{
			[CompilerGenerated]
			get
			{
				return _videoSettings.TextureDetail;
			}
		}

		public QualityLvl EffectDetail
		{
			[CompilerGenerated]
			get
			{
				return _videoSettings.EffectDetail;
			}
		}

		public event EventHandler ModelDetailChanged;

		public event EventHandler ShaderDetailChanged;

		public event EventHandler EffectDetailChanged;

		private VideoSettingsManager([NotNull] IFileStorage settingsStorage)
		{
			if (settingsStorage == null)
			{
				throw new ArgumentNullException("settingsStorage");
			}
			_storage = settingsStorage;
			_settingsKey = _settingsKey + "_" + SystemInfo.deviceName;
			_videoSettings = _storage.Load(_settingsKey, DefaultConfiguration());
			Apply(_videoSettings);
		}

		public static void Init()
		{
			Init(new PrefsStorage());
		}

		public static void Init([NotNull] IFileStorage fileStorage)
		{
			if (fileStorage == null)
			{
				throw new ArgumentNullException("fileStorage");
			}
			_instance = new VideoSettingsManager(fileStorage);
		}

		public VideoSettings DefaultConfiguration()
		{
			QualityLvl shaderDetail = QualityLvl.High;
			QualityLvl textureDetail = QualityLvl.High;
			if (SystemInfo.systemMemorySize < 1024)
			{
				shaderDetail = QualityLvl.Medium;
				textureDetail = QualityLvl.Medium;
			}
			VideoSettings result = default(VideoSettings);
			result.ShaderDetail = shaderDetail;
			result.TextureDetail = textureDetail;
			result.ModelDetail = QualityLvl.High;
			result.EffectDetail = QualityLvl.Medium;
			return result;
		}

		public VideoSettings Get()
		{
			return _videoSettings;
		}

		public IEnumerator Save(VideoSettings settings)
		{
			Apply(settings);
			yield return _storage.Save(_settingsKey, _videoSettings);
			yield return Resources.UnloadUnusedAssets();
		}

		private void Apply(VideoSettings data)
		{
			SetModelDetail(data.ModelDetail);
			SetShaderDetail(data.ShaderDetail);
			SetTextureDetail(data.TextureDetail);
			SetEffectDetail(data.EffectDetail);
		}

		private void SetModelDetail(QualityLvl value)
		{
			if (_videoSettings.ModelDetail != value)
			{
				switch (value)
				{
				case QualityLvl.High:
				case QualityLvl.VeryHigh:
					QualitySettings.maximumLODLevel = 0;
					break;
				case QualityLvl.Medium:
					QualitySettings.maximumLODLevel = 1;
					break;
				case QualityLvl.Low:
					QualitySettings.maximumLODLevel = 2;
					break;
				case QualityLvl.Disabled:
				case QualityLvl.VeryLow:
					QualitySettings.maximumLODLevel = 3;
					break;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
				_videoSettings.ModelDetail = value;
				Log.Debug($"ModelDetailChanged to {value}");
				if (this.ModelDetailChanged != null)
				{
					this.ModelDetailChanged();
				}
			}
		}

		private void SetShaderDetail(QualityLvl value)
		{
			if (_videoSettings.ShaderDetail != value)
			{
				_videoSettings.ShaderDetail = value;
				Log.Debug($"ShaderDetailChanged to {value}");
				if (this.ShaderDetailChanged != null)
				{
					this.ShaderDetailChanged();
				}
			}
		}

		private void SetTextureDetail(QualityLvl value)
		{
			int num;
			switch (value)
			{
			case QualityLvl.High:
			case QualityLvl.VeryHigh:
				num = 0;
				break;
			case QualityLvl.Medium:
				num = 1;
				break;
			case QualityLvl.Low:
				num = 2;
				break;
			case QualityLvl.Disabled:
			case QualityLvl.VeryLow:
				num = 3;
				break;
			default:
				throw new ArgumentOutOfRangeException("value", value, null);
			}
			if (QualitySettings.masterTextureLimit != num)
			{
				QualitySettings.masterTextureLimit = num;
				Log.Debug($"TextureDetailChanged to {value}");
			}
			_videoSettings.TextureDetail = value;
		}

		private void SetEffectDetail(QualityLvl value)
		{
			if (_videoSettings.EffectDetail != value)
			{
				_videoSettings.EffectDetail = value;
				Log.Debug($"EffectDetailChanged to {value}");
				if (this.EffectDetailChanged != null)
				{
					this.EffectDetailChanged();
				}
			}
		}
	}
}
