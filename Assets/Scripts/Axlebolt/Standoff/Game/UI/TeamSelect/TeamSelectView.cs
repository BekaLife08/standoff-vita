using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.TeamSelect
{
	public class TeamSelectView : View, IPlayerPropSensitiveView
	{
		private static readonly Log Log = Log.Create(typeof(TeamSelectView));

		private static readonly string[] PlayerProps = new string[1]
		{
			"team"
		};

		[SerializeField]
		[NotNull]
		private Image _background;

		[SerializeField]
		private Color _teamActiveColor;

		[SerializeField]
		private Color _teamInactiveColor;

		[SerializeField]
		private Color _ctBorderActiveColor;

		[SerializeField]
		private Color _ctBorderInActiveColor;

		[SerializeField]
		private Color _trBorderActiveColor;

		[SerializeField]
		private Color _trBorderInactiveColor;

		[NotNull]
		[SerializeField]
		private Button _trButton;

		[NotNull]
		[SerializeField]
		private Image _trBorder;

		[NotNull]
		[SerializeField]
		private TeamSelectPlayersView _trPlayers;

		[NotNull]
		[SerializeField]
		private Button _trSelectButton;

		[SerializeField]
		[NotNull]
		private Button _ctButton;

		[SerializeField]
		[NotNull]
		private Image _ctBorder;

		[SerializeField]
		[NotNull]
		private TeamSelectPlayersView _ctPlayers;

		[SerializeField]
		[NotNull]
		private Button _ctSelectButton;

		private GameController _gameController;

		private Team _selectedTeam;

		public Sprite LevelImage
		{
			set
			{
				_background.sprite = value;
			}
		}

		public string[] SensitivePlayerProperties
		{
			[CompilerGenerated]
			get
			{
				return PlayerProps;
			}
		}

		public void Init([NotNull] GameController gameController)
		{
			if (gameController == null)
			{
				throw new ArgumentNullException("gameController");
			}
			_gameController = gameController;
			_ctButton.GetComponent<Image>();
			_trButton.GetComponent<Image>();
			_trButton.onClick.AddListener(OnClickTrButton);
			_ctButton.onClick.AddListener(OnClickCtButton);
			_trSelectButton.onClick.AddListener(OnSelectTrButton);
			_ctSelectButton.onClick.AddListener(OnSelectCtButton);
		}

		public void OnClickTrButton()
		{
			if (PhotonNetwork.player.GetTeam() != Team.Tr && _gameController.CanChangeTeamTo(Team.Tr))
			{
				_selectedTeam = Team.Tr;
				SelectTrTeam();
				Refresh();
			}
		}

		private void OnClickCtButton()
		{
			if (PhotonNetwork.player.GetTeam() != Team.Ct && _gameController.CanChangeTeamTo(Team.Ct))
			{
				_selectedTeam = Team.Ct;
				SelectCtTeam();
				Refresh();
			}
		}

		private void OnSelectTrButton()
		{
			_gameController.ChangeTeamTo(Team.Tr);
		}

		public void OnSelectCtButton()
		{
			_gameController.ChangeTeamTo(Team.Ct);
		}

		private void SelectTrTeam()
		{
			SetTrActive(active: true);
			SetCtActive(active: false);
		}

		public void SelectCtTeam()
		{
			SetTrActive(active: false);
			SetCtActive(active: true);
		}

		public void SelectNonTeam()
		{
			SetTrActive(active: false);
			SetCtActive(active: false);
		}

		private void SetTrActive(bool active)
		{
			_trBorder.color = ((!active) ? _trBorderInactiveColor : _trBorderActiveColor);
			_trButton.GetComponent<Image>().color = ((!active) ? _teamInactiveColor : _teamActiveColor);
			_trSelectButton.gameObject.SetActive(active);
		}

		public void SetCtActive(bool active)
		{
			_ctBorder.color = ((!active) ? _ctBorderInActiveColor : _ctBorderActiveColor);
			_ctButton.GetComponent<Image>().color = ((!active) ? _teamInactiveColor : _teamActiveColor);
			_ctSelectButton.gameObject.SetActive(active);
		}

		public override void Show()
		{
			base.Show();
			_selectedTeam = PhotonNetwork.player.GetTeam();
			switch (_selectedTeam)
			{
			case Team.Tr:
				SelectTrTeam();
				break;
			case Team.Ct:
				SelectCtTeam();
				break;
			default:
				SelectNonTeam();
				break;
			}
			Refresh();
		}

		public void Refresh()
		{
			if (!base.IsVisible)
			{
				Log.Error("Can't refresh inactive team select view");
				return;
			}
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			PhotonPlayer[] array = playerList.GetByTeam(Team.Ct, excludeCurrentPlayer: true);
			PhotonPlayer[] array2 = playerList.GetByTeam(Team.Tr, excludeCurrentPlayer: true);
			switch (_selectedTeam)
			{
			case Team.Ct:
				Array.Resize(ref array, array.Length + 1);
				array[array.Length - 1] = PhotonNetwork.player;
				break;
			case Team.Tr:
				Array.Resize(ref array2, array2.Length + 1);
				array2[array2.Length - 1] = PhotonNetwork.player;
				break;
			}
			_ctPlayers.Refresh(array);
			_trPlayers.Refresh(array2);
		}
	}
}
