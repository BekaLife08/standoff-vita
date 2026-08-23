using Axlebolt.Standoff.Common;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Audio
{
	public class AudioSettingsManager
	{
		private static AudioSettingsManager _instance;

		private readonly string _key = "AudioSettings";

		private readonly IFileStorage _storage;

		public static AudioSettingsManager Instance
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

		public AudioSettings Model
		{
			get;
			private set;
		}

		private AudioSettingsManager(IFileStorage settingsStorage)
		{
			_storage = settingsStorage;
			_key = _key + "_" + SystemInfo.deviceName;
			Model = _storage.Load(_key, DefaultValue());
			Apply(Model);
		}

		public static void Init()
		{
			Init(new PrefsStorage());
		}

		public static void Init(IFileStorage storage)
		{
			_instance = new AudioSettingsManager(storage);
		}

		public AudioSettings DefaultValue()
		{
			return new AudioSettings();
		}

		public IEnumerator Save(AudioSettings model)
		{
			Model = model;
			Apply(Model);
			yield return _storage.Save(_key, Model);
		}

		private void Apply(AudioSettings audioSettings)
		{
			AudioListener.volume = audioSettings.Volume;
		}
	}
}
