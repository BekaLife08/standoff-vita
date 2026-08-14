using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Settings.Audio;
using Axlebolt.Standoff.Settings.Controls;
using Axlebolt.Standoff.Settings.Game;
using Axlebolt.Standoff.Settings.Video;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsManager
	{
		private static bool _initialized;

		public static void Init()
		{
			Init(new PrefsStorage());
		}

		public static void InitIfNeed()
		{
			if (!_initialized)
			{
				Init();
			}
		}

		public static void Init(IFileStorage fileStorage)
		{
			VideoSettingsManager.Init(fileStorage);
			ControlsSettingsManager.Init(fileStorage);
			AudioSettingsManager.Init(fileStorage);
			GameSettingsManager.Init(fileStorage);
			_initialized = true;
		}
	}
}
