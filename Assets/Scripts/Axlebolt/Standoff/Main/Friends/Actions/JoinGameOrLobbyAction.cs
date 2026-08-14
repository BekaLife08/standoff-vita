using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Player;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class JoinGameOrLobbyAction : IFriendAction
	{
		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.JoinGameOrLobby;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.JoinGameOrLobby;
			}
		}

		public Task Execute(BoltFriend friend)
		{
			return (friend.PlayInGame.PhotonGame == null) ? LobbyUtility.JoinLobby(friend.PlayInGame.LobbyId) : LobbyUtility.JoinGame(friend.PlayInGame.PhotonGame, friend.PlayerStatus.PlayInGame.GameVersion);
		}

		public bool IsSupported(BoltFriend friend)
		{
			if (friend.Relationship != RelationshipStatus.Friend)
			{
				return false;
			}
			PlayInGame playInGame = friend.PlayInGame;
			if (!string.IsNullOrEmpty((playInGame != null) ? playInGame.LobbyId : null))
			{
				if (BoltService<BoltMatchmakingService>.Instance.IsInLobby())
				{
					return friend.PlayInGame.LobbyId != BoltService<BoltMatchmakingService>.Instance.CurrentLobby.Id;
				}
				return true;
			}
			PlayInGame playInGame2 = friend.PlayInGame;
			if (((playInGame2 != null) ? playInGame2.PhotonGame : null) != null)
			{
				return true;
			}
			return false;
		}
	}
}
