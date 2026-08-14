using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI.GameStats;
using Axlebolt.Standoff.Game.UI.Winners;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Player;
using ExitGames.Client.Photon;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseRoundEndingGameState : TimeBasedGameState
	{
		private class MvpComparer : IComparer<PhotonPlayer>
		{
			public int Compare(PhotonPlayer x, PhotonPlayer y)
			{
				if (x.GetRoundKills() == y.GetRoundKills())
				{
					return (x.GetRoundAssists() != y.GetRoundAssists()) ? x.GetRoundAssists().CompareTo(y.GetRoundAssists()) : x.ID.CompareTo(y.ID);
				}
				return x.GetRoundKills().CompareTo(y.GetRoundKills());
			}
		}

		private DefuseController _defuseController;

		private TeamGameStatsView _teamGameStatsView;

		private WinnersView _winnersView;

		[CompilerGenerated]
		private static Func<PhotonPlayer, double> _003C_003Ef__mg_0024cache0;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 101;
			}
		}

		public DefuseRoundEndingGameState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_defuseController = (DefuseController)gameController;
			_teamGameStatsView = gameController.GameView.GetView<TeamGameStatsView>();
			_winnersView = gameController.GameView.GetView<WinnersView>();
		}

		public override Hashtable Init()
		{
			Hashtable hashtable = base.Init();
			if (_defuseController.IsBombPlantedOnTime())
			{
				if (ScenePhotonBehavior<BombManager>.Instance.IsDefused)
				{
					WinTeam(hashtable, Team.Ct, ScenePhotonBehavior<BombManager>.Instance.FinalSapper, DefuseMvp.DefusingBomb);
					return hashtable;
				}
				if (!PhotonNetwork.playerList.IsAllDead(Team.Ct))
				{
					WinTeam(hashtable, Team.Tr, ScenePhotonBehavior<BombManager>.Instance.FinalBomber, DefuseMvp.PlantingBomb);
					return hashtable;
				}
				PhotonPlayer photonPlayer = (from player in PhotonNetwork.playerList
					where player.GetTeam() == Team.Ct
					select player).OrderByDescending(PhotonPlayerExtensions.GetDeathTime).FirstOrDefault();
				if (photonPlayer == null)
				{
					WinTeam(hashtable, Team.Tr, null, DefuseMvp.None);
					return hashtable;
				}
				if (ScenePhotonBehavior<BombManager>.Instance.IsDetonated() && photonPlayer.GetDeathTime() > ScenePhotonBehavior<BombManager>.Instance.DetonatedTime)
				{
					WinTeam(hashtable, Team.Tr, ScenePhotonBehavior<BombManager>.Instance.FinalBomber, DefuseMvp.PlantingBomb);
					return hashtable;
				}
				WinTeam(hashtable, Team.Tr, GetMvpPlayer(Team.Tr), DefuseMvp.MostEliminations);
				return hashtable;
			}
			if (PhotonNetwork.playerList.IsAllDead(Team.Ct))
			{
				WinTeam(hashtable, Team.Tr, GetMvpPlayer(Team.Tr), DefuseMvp.MostEliminations);
				return hashtable;
			}
			WinTeam(hashtable, Team.Ct, GetMvpPlayer(Team.Ct), DefuseMvp.MostEliminations);
			return hashtable;
		}

		private PhotonPlayer GetMvpPlayer(Team team)
		{
			return (from player in PhotonNetwork.playerList
				where player.GetTeam() == team
				select player).OrderByDescending((PhotonPlayer player) => player, new MvpComparer()).FirstOrDefault();
		}

		private void WinTeam(Hashtable hashtable, Team team, PhotonPlayer mvpPlayer, DefuseMvp mvp)
		{
			hashtable.AddScore(team);
			hashtable.AddConsecutiveLosses(team.GetOtherTeam());
			hashtable.SetWinTeam(new WinTeam(team, mvpPlayer, (byte)mvp));
			mvpPlayer?.AddMvp();
		}

		public override void Enter()
		{
			WinTeam winTeam = PhotonNetwork.room.GetWinTeam();
			_winnersView.Show(winTeam.Team, winTeam.MvpPlayer, MvpCodeToMessage(winTeam.MvpPlayer, (DefuseMvp)winTeam.MvpCode));
			_defuseController.RoundFinished(winTeam.Team, winTeam.MvpPlayer);
			_teamGameStatsView.SetTimeVisible(visible: false);
		}

		private string MvpCodeToMessage(PhotonPlayer player, DefuseMvp mvp)
		{
			if (player == null)
			{
				return string.Empty;
			}
			switch (mvp)
			{
			case DefuseMvp.DefusingBomb:
				return string.Format(ScriptLocalization.Defuse.MvpDefusingBomb, player.NickName);
			case DefuseMvp.PlantingBomb:
				return string.Format(ScriptLocalization.Defuse.MvpPlantingBomb, player.NickName);
			case DefuseMvp.MostEliminations:
				return string.Format(ScriptLocalization.Defuse.MvpMostEliminations, player.NickName);
			case DefuseMvp.None:
				return string.Empty;
			default:
				throw new ArgumentOutOfRangeException("mvp", mvp, null);
			}
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}

		public override void Complete()
		{
		}

		public override bool CanSpawnPlayer()
		{
			return false;
		}
	}
}
