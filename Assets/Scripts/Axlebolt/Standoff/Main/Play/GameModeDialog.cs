using System;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Play
{
	public class GameModeDialog : View
	{
		[SerializeField]
		private CloseButton _closeButton;

		[SerializeField]
		private Button _deathMatchButton;

		[SerializeField]
		private Button _defusebutton;

		public Action DeathMatchHandler { get; set; }

		public Action DefuseHandler { get; set; }

		private void Awake()
		{
			_closeButton.CloseHandler = Hide;
			_deathMatchButton.onClick.AddListener(delegate
			{
				Hide();
				Action deathMatchHandler = DeathMatchHandler;
				if (deathMatchHandler != null)
				{
					deathMatchHandler();
				}
			});
			_defusebutton.onClick.AddListener(delegate
			{
				Hide();
				Action defuseHandler = DefuseHandler;
				if (defuseHandler != null)
				{
					defuseHandler();
				}
			});
		}
	}
}
