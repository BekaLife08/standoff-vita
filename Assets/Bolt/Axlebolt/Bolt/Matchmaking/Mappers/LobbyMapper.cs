using System.Collections.Concurrent;
using System.Collections.Generic;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Matchmaking.Mappers
{
	public class LobbyMapper : MessageMapper<Lobby, BoltLobby>
	{
		public static readonly LobbyMapper Instance = new LobbyMapper();

		public override BoltLobby ToOriginal(Lobby proto)
		{
			ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>(proto.Data);
			List<BoltFriend> lobbyMembers = FriendMapper.Instance.ToOriginalList(proto.Members);
			List<BoltFriend> lobbyInvites = FriendMapper.Instance.ToOriginalList(proto.Invites);
			BoltGameServer gameServer = GameServerMapper.Instance.ToOriginal(proto.GameServer);
			return new BoltLobby(proto.Id, proto.OwnerPlayerId, proto.Name, EnumMapper<Protobuf.LobbyType, BoltLobby.LobbyType>.ToOriginal(proto.LobbyType), proto.Joinable, proto.MaxMembers, data, lobbyMembers, lobbyInvites, gameServer, PhotonGameMapper.Instance.ToOriginal(proto.PhotonGame));
		}
	}
}
