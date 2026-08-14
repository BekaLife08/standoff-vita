using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("FriendsRemoteService")]
	public class FriendsRemoteService : RpcService
	{
		public FriendsRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("unblockFriend")]
		public RelationshipStatus UnblockFriend(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("ignoreFriendRequest")]
		public RelationshipStatus IgnoreFriendRequest(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("blockFriend")]
		public RelationshipStatus BlockFriend(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("sendFriendRequest")]
		public RelationshipStatus SendFriendRequest(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("getAvatars")]
		public AvatarBinary[] GetAvatars(string[] avatarIds, CancellationToken ct = default(CancellationToken))
		{
			return (AvatarBinary[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { avatarIds }, ct);
		}

		[Rpc("getPlayerById")]
		public Axlebolt.Bolt.Protobuf.Player GetPlayer(string playerId, CancellationToken ct = default(CancellationToken))
		{
			return (Axlebolt.Bolt.Protobuf.Player)Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}

		[Rpc("getPlayerFriendById")]
		public PlayerFriend GetPlayerFriend(string playerId, CancellationToken ct = default(CancellationToken))
		{
			return (PlayerFriend)Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}

		[Rpc("searchPlayers")]
		public PlayerFriend[] SearchPlayers(string value, int page, int size, CancellationToken ct = default(CancellationToken))
		{
			return (PlayerFriend[])Invoke(MethodBase.GetCurrentMethod(), new object[3] { value, page, size }, ct);
		}

		[Rpc("getPlayerFriends")]
		public PlayerFriend[] GetPlayerFriends(RelationshipStatus[] relationshipStatuses, int page, int size, CancellationToken ct = default(CancellationToken))
		{
			return (PlayerFriend[])Invoke(MethodBase.GetCurrentMethod(), new object[3] { relationshipStatuses, page, size }, ct);
		}

		[Rpc("acceptFriendRequest")]
		public RelationshipStatus AcceptFriendRequest(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("revokeFriendRequest")]
		public RelationshipStatus RevokeFriendRequest(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}

		[Rpc("getPlayerFriendsIds")]
		public string[] GetPlayerFriendsIds(RelationshipStatus[] relationshipStatuses, CancellationToken ct = default(CancellationToken))
		{
			return (string[])Invoke(MethodBase.GetCurrentMethod(), new object[1] { relationshipStatuses }, ct);
		}

		[Rpc("getPlayerFriendsCount")]
		public long GetPlayerFriendsCount(RelationshipStatus[] relationshipStatuses, CancellationToken ct = default(CancellationToken))
		{
			return (long)Invoke(MethodBase.GetCurrentMethod(), new object[1] { relationshipStatuses }, ct);
		}

		[Rpc("getPlayersCount")]
		public long GetPlayersCount(string value, CancellationToken ct = default(CancellationToken))
		{
			return (long)Invoke(MethodBase.GetCurrentMethod(), new object[1] { value }, ct);
		}

		[Rpc("getOnlineStatus")]
		public OnlineStatus GetOnlineStatus(string playerId, CancellationToken ct = default(CancellationToken))
		{
			return (OnlineStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { playerId }, ct);
		}

		[Rpc("removeFriend")]
		public RelationshipStatus RemoveFriend(string friendId, CancellationToken ct = default(CancellationToken))
		{
			return (RelationshipStatus)Invoke(MethodBase.GetCurrentMethod(), new object[1] { friendId }, ct);
		}
	}
}
