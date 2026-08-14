using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class LevelLoadingView : View
	{
		private const int SettingsMaxCount = 10;

		[SerializeField]
		private Image _levelImage;

		[SerializeField]
		private Text _levelNameText;

		[SerializeField]
		private Image _levelSchemaImage;

		[SerializeField]
		private Text _gameModeText;

		[SerializeField]
		private Text _gameModeDescriptionText;

		[SerializeField]
		private TextView _gameModeSettingPrefab;

		[SerializeField]
		private Text _loadingText;

		[SerializeField]
		private Image _progressBar;

		public float Progress
		{
			set
			{
				_progressBar.fillAmount = value;
			}
		}

		public LevelDefinition Level
		{
			set
			{
				_levelNameText.text = value.DisplayName;
				_levelSchemaImage.sprite = value.SchemaImage;
				_levelImage.sprite = value.FullScreenImage;
			}
		}

		public GameMode GameMode
		{
			set
			{
				_gameModeText.text = value.LocalizedName;
				_gameModeDescriptionText.text = value.LocalizedDescription;
			}
		}

		public string LoadingText
		{
			set
			{
				_loadingText.text = value;
			}
		}

		private void Awake()
		{
		}

		public override void Show()
		{
			base.Show();
			Progress = 0f;
		}
	}
}
