using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Matchmaking.Mappers
{
	public class LobbyInviteMapper : MessageMapper<LobbyInvite, BoltLobbyInvite>
	{
		public static LobbyInviteMapper Instance = new LobbyInviteMapper();

		public override BoltLobbyInvite ToOriginal(LobbyInvite proto)
		{
			return new BoltLobbyInvite(proto.LobbyId, FriendMapper.Instance.ToOriginal(proto.InviteCreator), proto.Date);
		}
	}
}
