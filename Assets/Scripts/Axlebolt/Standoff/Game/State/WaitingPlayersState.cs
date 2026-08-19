using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using ExitGames.Client.Photon;
using I2.Loc;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public class WaitingPlayersState : IGameState
	{
		private StatisticsView _statisticsView;

		private GameStateView _gameStateView;

		public byte Id
		{
			[CompilerGenerated]
			get
			{
				return 10;
			}
		}

		public void Setup(GameController gameController)
		{
			_gameStateView = gameController.GameView.GetView<GameStateView>();
			_statisticsView = gameController.GameView.GetView<StatisticsView>();
		}

		public Hashtable Init()
		{
			return new Hashtable();
		}

		public void Enter()
		{
			if (PhotonNetwork.offlineMode)
			{
				Axlebolt.Standoff.Bots.BotManager.SpawnBots();
			}
			_gameStateView.Show();
			_gameStateView.Text = ScriptLocalization.GameState.WaitingPlayers;
			_statisticsView.GameDescription = ScriptLocalization.GameState.WaitingPlayers;
		}

		public YieldInstruction Instruction()
		{
			return new WaitForSeconds(0.5f);
		}

		public void Update()
		{
		}

		public bool CheckExit()
		{
			if (PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.player.GetTeam() != Team.None;
			}
			int ctPlayersCount = PhotonNetwork.playerList.GetCtPlayersCount();
			int trPlayersCount = PhotonNetwork.playerList.GetTrPlayersCount();
			return ctPlayersCount >= 1 && trPlayersCount >= 1;
		}

		public void Exit()
		{
			_gameStateView.Hide();
			_statisticsView.GameDescription = string.Empty;
		}

		public void Complete()
		{
		}

		public bool CanSpawnPlayer()
		{
			return true;
		}
	}
}
