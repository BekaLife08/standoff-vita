using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("MatchmakingRemoteService")]
	public class MatchmakingRemoteService : RpcService
	{
		public MatchmakingRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("getInvitesToLobby")]
		public LobbyInvite[] GetInvitesToLobby(CancellationToken ct = default(CancellationToken))
		{
			return (LobbyInvite[])Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("deleteLobbyData")]
		public void DeleteLobbyData(string[] keys, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { keys }, ct);
		}

		[Rpc("getLobbyPhotonGame")]
		public PhotonGame GetLobbyPhotonGame(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (PhotonGame)Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("requestInternetServerList")]
		public GameServer[] RequestInternetServerList(string map, byte? freePlayerSlots, byte? maxPlayers, bool withPassword, CancellationToken ct = default(CancellationToken))
		{
			return (GameServer[])Invoke(MethodBase.GetCurrentMethod(), new object[4] { map, freePlayerSlots, maxPlayers, withPassword }, ct);
		}

		[Rpc("setLobbyMaxMembers")]
		public void SetLobbyMaxMembers(int maxMembers, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { maxMembers }, ct);
		}

		[Rpc("setLobbyJoinable")]
		public void SetLobbyJoinable(bool joinable, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { joinable }, ct);
		}

		[Rpc("refuseInvitationToLobby")]
		public void RefuseInvitationToLobby(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("invitePlayerToLobby")]
		public void InvitePlayerToLobby(string invitedPlayerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { invitedPlayerId }, ct);
		}

		[Rpc("joinLobby")]
		public Lobby JoinLobby(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (Lobby)Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("setLobbyType")]
		public void SetLobbyType(Protobuf.LobbyType lobbyType, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyType }, ct);
		}

		[Rpc("setLobbyData")]
		public void SetLobbyData(Dictionary data, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { data }, ct);
		}

		[Rpc("setLobbyName")]
		public void SetLobbyName(string name, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { name }, ct);
		}

		[Rpc("createLobby")]
		public Lobby CreateLobby(string name, Protobuf.LobbyType lobbyType, int maxMembers, CancellationToken ct = default(CancellationToken))
		{
			return (Lobby)Invoke(MethodBase.GetCurrentMethod(), new object[3] { name, lobbyType, maxMembers }, ct);
		}

		[Rpc("getGameServerPlayers")]
		public Axlebolt.Bolt.Protobuf.Player[] GetGameServerPlayers(string gameServerId, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { gameServerId }, ct);
		}

		[Rpc("setLobbyPhotonGame")]
		public void SetLobbyPhotonGame(PhotonGame photonGame, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { photonGame }, ct);
		}

		[Rpc("getLobbyGameServer")]
		public GameServerDetails GetLobbyGameServer(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (GameServerDetails)Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("leaveLobby")]
		public void LeaveLobby(CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("sendLobbyChatMsg")]
		public void SendLobbyChatMsg(string message, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { message }, ct);
		}

		[Rpc("revokePlayerInvitationToLobby")]
		public void RevokePlayerInvitationToLobby(string revokedPlayerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { revokedPlayerId }, ct);
		}

		[Rpc("getLobbyMembers")]
		public Axlebolt.Bolt.Protobuf.Player[] GetLobbyMembers(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("setLobbyGameServer")]
		public void SetLobbyGameServer(string gameServerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { gameServerId }, ct);
		}

		[Rpc("requestLobbyList")]
		public Lobby[] RequestLobbyList(LobbyDistanceFilter distanceFilter, Filter[] filters, CancellationToken ct = default(CancellationToken))
		{
			return (Lobby[])Invoke(MethodBase.GetCurrentMethod(), new object[2] { distanceFilter, filters }, ct);
		}

		[Rpc("getLobbyOwner")]
		public Axlebolt.Bolt.Protobuf.Player GetLobbyOwner(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player)Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("getGameServerDetails")]
		public GameServerDetails GetGameServerDetails(string gameServerId, CancellationToken ct = default(CancellationToken))
		{
			return (GameServerDetails)Invoke(MethodBase.GetCurrentMethod(), new object[1] { gameServerId }, ct);
		}

		[Rpc("kickPlayerFromLobby")]
		public void KickPlayerFromLobby(string kickedPlayerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { kickedPlayerId }, ct);
		}

		[Rpc("getLobby")]
		public Lobby GetLobby(string lobbyId, CancellationToken ct = default(CancellationToken))
		{
			return (Lobby)Invoke(MethodBase.GetCurrentMethod(), new object[1] { lobbyId }, ct);
		}

		[Rpc("setLobbyOwner")]
		public void SetLobbyOwner(string playerId, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}
	}
}
