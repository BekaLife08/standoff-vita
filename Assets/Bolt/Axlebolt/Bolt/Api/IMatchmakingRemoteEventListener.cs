using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[EventListener("MatchmakingRemoteEventListener")]
	public interface IMatchmakingRemoteEventListener
	{
		[Event("onLobbyDataChanged")]
		void OnLobbyDataChanged(Dictionary data);

		[Event("onNewPlayerJoinedLobby")]
		void OnNewPlayerJoinedLobby(PlayerFriend newPlayer);

		[Event("onLobbyJoinableChanged")]
		void OnLobbyJoinableChanged(bool joinable);

		[Event("onLobbyPhotonGameChanged")]
		void OnLobbyPhotonGameChanged(PhotonGame photonGame);

		[Event("onLobbyChatMessage")]
		void OnLobbyChatMessage(string playerId, string message);

		[Event("onNewPlayerInvitedToLobby")]
		void OnNewPlayerInvitedToLobby(string inviteSenderId, PlayerFriend newPlayer);

		[Event("onLobbyTypeChanged")]
		void OnLobbyTypeChanged(LobbyType lobbyType);

		[Event("onLobbyNameChanged")]
		void OnLobbyNameChanged(string name);

		[Event("onLobbyOwnerChanged")]
		void OnLobbyOwnerChanged(string lobbyOwnerId);

		[Event("onPlayerKickedFromLobby")]
		void OnPlayerKickedFromLobby(string kickInitiatorId, string kickedPlayerId);

		[Event("onLobbyMaxMembersChanged")]
		void OnLobbyMaxMembersChanged(byte maxMembers);

		[Event("onReceivedInviteToLobby")]
		void OnReceivedInviteToLobby(PlayerFriend inviteSender, string lobbyId);

		[Event("onPlayerLeftLobby")]
		void OnPlayerLeftLobby(string leftPlayerId);

		[Event("onLobbyGameServerChanged")]
		void OnLobbyGameServerChanged(GameServer gameServer);

		[Event("onRevokeInviteToLobby")]
		void OnRevokeInviteToLobby(string inviteSenderId, string invitedPlayerId);

		[Event("onRefuseInviteToLobby")]
		void OnRefuseInviteToLobby(string inviteSenderId, string invitedPlayerId);
	}
}
