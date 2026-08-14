using Axlebolt.Standoff.Common;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Game
{
	public class GameSettingsManager
	{
		private static GameSettingsManager _instance;

		private readonly string _key = "GameSettings";

		private readonly IFileStorage _storage;

		public static GameSettingsManager Instance
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

		public GameSettings Model
		{
			get;
			private set;
		}

		private GameSettingsManager(IFileStorage settingsStorage)
		{
			_storage = settingsStorage;
			_key = _key + "_" + SystemInfo.deviceName;
			Model = _storage.Load(_key, DefaultValue());
		}

		public static void Init()
		{
			Init(new PrefsStorage());
		}

		public static void Init(IFileStorage storage)
		{
			_instance = new GameSettingsManager(storage);
		}

		public GameSettings DefaultValue()
		{
			return new GameSettings();
		}

		public IEnumerator Save(GameSettings model)
		{
			Model = model;
			yield return _storage.Save(_key, Model);
		}
	}
}
