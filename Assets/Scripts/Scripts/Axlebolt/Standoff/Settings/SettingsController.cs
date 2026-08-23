using Axlebolt.Standoff.Settings.Audio;
using Axlebolt.Standoff.Settings.Controls;
using Axlebolt.Standoff.Settings.Game;
using Axlebolt.Standoff.Settings.Video;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsController : TabController<SettingsController>
	{
		[SerializeField]
		private CloseButton _closeButton;

		[SerializeField]
		private Button _resetButton;

		[SerializeField]
		private Button _applyButton;

		[SerializeField]
		private SettingsTabBar _settingsTab;

		[SerializeField]
		private GameSettingsController _gameSettingsController;

		[SerializeField]
		private VideoSettingsController _videoSettingsController;

		[SerializeField]
		private ControlsSettingsController _controlsSettingsController;

		[SerializeField]
		private AudioSettingsController _audioSettingsController;

		public override void Init()
		{
			base.Init();
			_settingsTab.SetGameSettingsController(_gameSettingsController);
			_settingsTab.SetVideoSettingsController(_videoSettingsController);
			_settingsTab.SetControlSettingsController(_controlsSettingsController);
			_settingsTab.SetAudioSettingsController(_audioSettingsController);
			_controlsSettingsController.CustomizationController.OpenStateChangedEvent += delegate(bool isOpen)
			{
				_closeButton.gameObject.SetActive(!isOpen);
			};
			_closeButton.CloseHandler = TryClose;
			_resetButton.onClick.AddListener(ResetDefaults);
			_applyButton.onClick.AddListener(Apply);
			_gameSettingsController.DirtyChangedEvent = SetDirty;
			_videoSettingsController.DirtyChangedEvent = SetDirty;
			_controlsSettingsController.DirtyChangedEvent = SetDirty;
			_audioSettingsController.DirtyChangedEvent = SetDirty;
		}

		public override bool IsWindowLayout()
		{
			return true;
		}

		public override void OnOpen()
		{
			_applyButton.interactable = false;
			_settingsTab.Select(_gameSettingsController);
		}

		public override void OnClose()
		{
			_settingsTab.Unselect();
		}

		private void TryClose()
		{
			_settingsTab.CurrentController.CanClose(delegate(bool canClose)
			{
				if (canClose)
				{
					Close();
				}
			});
		}

		private void SetDirty(bool isDirty)
		{
			_applyButton.interactable = isDirty;
		}

		private void ResetDefaults()
		{
			GetCurrentController().ResetDefaults();
		}

		private void Apply()
		{
			GetCurrentController().Apply();
		}

		private ISettingsTabController GetCurrentController()
		{
			return (ISettingsTabController)_settingsTab.CurrentController;
		}
	}
}
