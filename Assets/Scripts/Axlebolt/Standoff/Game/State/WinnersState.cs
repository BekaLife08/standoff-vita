using Axlebolt.Standoff.Game.UI.Winners;
using ExitGames.Client.Photon;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game.State
{
	public class WinnersState : TimeBasedGameState
	{
		private GameController _gameController;

		private WinnersView _winnersView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 200;
			}
		}

		public WinnersState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_winnersView = gameController.GameView.GetView<WinnersView>();
		}

		public override Hashtable Init()
		{
			Hashtable hashtable = base.Init();
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			PhotonPlayer[] array = playerList.OrderByScore();
			PhotonPlayer photonPlayer = array[0];
			PhotonPlayer secondPlace = (array.Length <= 1) ? null : array[1];
			PhotonPlayer thirdPlace = (array.Length <= 2) ? null : array[2];
			WinPlayers winPlayers = new WinPlayers(photonPlayer, secondPlace, thirdPlace);
			hashtable.SetFinalPlayers(playerList);
			hashtable.SetWinPlayers(winPlayers);
			hashtable.SetFinalWinTeam(FinalWinTeam.Winner(photonPlayer.GetTeam()));
			return hashtable;
		}

		public override void Enter()
		{
			_gameController.DestroyPlayer();
			WinPlayers winPlayers = PhotonNetwork.room.GetWinPlayers();
			_winnersView.Show(winPlayers.FirstPlace, winPlayers.SecondPlace, winPlayers.ThirdPlace);
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
