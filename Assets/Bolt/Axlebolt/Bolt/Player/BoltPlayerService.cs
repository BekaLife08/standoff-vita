using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Player
{
	public class BoltPlayerService : BoltService<BoltPlayerService>
	{
		private readonly PlayerRemoteService _playerRemoteService;

		public BoltPlayer Player { get; private set; }

		internal BoltPlayerService()
		{
			_playerRemoteService = new PlayerRemoteService(BoltApi.Instance.ClientService);
		}

		public Task SetName(string name)
		{
			return BoltApi.Instance.Async(delegate
			{
				_playerRemoteService.SetPlayerName(name);
				Player.Name = name;
			});
		}

		public Task SetAvatar(byte[] avatar)
		{
			if (avatar == null)
			{
				throw new ArgumentNullException("avatar");
			}
			return BoltApi.Instance.Async(delegate
			{
				string avatarId = _playerRemoteService.SetPlayerAvatar(avatar);
				Player.AvatarId = avatarId;
				Player.Avatar = avatar;
				BoltAvatars.Instance.SaveToCache(avatarId, avatar);
			});
		}

		public Task SetFirebaseToken(string token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			return BoltApi.Instance.Async(delegate
			{
				_playerRemoteService.SetPlayerFirebaseToken(token);
			});
		}

		public Task SetOnlineStatus()
		{
			return BoltApi.Instance.Async(SetOnlineStatusInternal);
		}

		public Task SetAwayStatus()
		{
			return BoltApi.Instance.Async(SetAwayStatusInternal);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void SetAwayStatusInternal()
		{
			Player.PlayerStatus.OnlineStatus = OnlineStatus.StateAway;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void SetOnlineStatusInternal()
		{
			Player.PlayerStatus.OnlineStatus = OnlineStatus.StateOnline;
		}

		internal override void Load()
		{
			Axlebolt.Bolt.Protobuf.Player currentPlayer = new Protobuf.Player { AvatarId = "", Id = Guid.NewGuid().ToString(), Name = "Vladimir", PlayerStatus = new Protobuf.PlayerStatus(), TimeInGame = 100000, Uid = "Guest" };
			Player = PlayerMapper.Instance.ToOriginal(currentPlayer);
			BoltAvatars.Instance.FillCached(Player);
		}
	}
}
