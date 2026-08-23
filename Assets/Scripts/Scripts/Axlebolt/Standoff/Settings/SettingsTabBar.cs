using Axlebolt.Standoff.Settings.Audio;
using Axlebolt.Standoff.Settings.Controls;
using Axlebolt.Standoff.Settings.Game;
using Axlebolt.Standoff.Settings.Video;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsTabBar : TabBar
	{
		[SerializeField]
		private TabButton _gameSettingsButton;

		[SerializeField]
		private TabButton _videoSettingsButton;

		[SerializeField]
		private TabButton _controlSettingsButton;

		[SerializeField]
		private TabButton _audioSettingsButton;

		public void SetGameSettingsController(GameSettingsController controller)
		{
			Add(_gameSettingsButton, controller);
		}

		public void SetVideoSettingsController(VideoSettingsController controller)
		{
			Add(_videoSettingsButton, controller);
		}

		public void SetControlSettingsController(ControlsSettingsController controller)
		{
			Add(_controlSettingsButton, controller);
		}

		public void SetAudioSettingsController(AudioSettingsController controller)
		{
			Add(_audioSettingsButton, controller);
		}
	}
}
