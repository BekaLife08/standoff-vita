using Axlebolt.Standoff.UI;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class PauseView : View
	{
		[SerializeField]
		private PauseMenuView _pauseMenuPrefab;

		public PauseMenuView Main
		{
			get;
			private set;
		}

		private void Awake()
		{
			_pauseMenuPrefab.gameObject.SetActive(value: false);
			Main = CreateMenu(ScriptLocalization.PauseMenu.Title);
			Main.gameObject.SetActive(value: true);
		}

		public PauseMenuView CreateMenu(string title)
		{
			PauseMenuView pauseMenuView = Object.Instantiate(_pauseMenuPrefab, _pauseMenuPrefab.transform.parent, worldPositionStays: false);
			pauseMenuView.Title = title;
			return pauseMenuView;
		}
	}
}
