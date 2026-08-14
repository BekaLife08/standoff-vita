using System.Collections.Concurrent;
using System.Collections.Generic;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf.Collections;

namespace Axlebolt.Bolt.Player
{
	public class PlayerMapper : MessageMapper<Axlebolt.Bolt.Protobuf.Player, BoltPlayer>
	{
		public static readonly PlayerMapper Instance = new PlayerMapper();

		public T ToOriginal<T>(Axlebolt.Bolt.Protobuf.Player proto, T boltPlayer) where T : BoltPlayer
		{
			boltPlayer.Id = proto.Id;
			boltPlayer.Uid = proto.Uid;
			boltPlayer.Name = proto.Name;
			boltPlayer.Avatar = null;
			boltPlayer.AvatarId = proto.AvatarId;
			boltPlayer.TimeInGame = proto.TimeInGame;
			if (proto.PlayerStatus != null)
			{
				boltPlayer.PlayerStatus = PlayerStatusMapper.Instance.ToOriginal(proto.PlayerStatus);
			}
			return boltPlayer;
		}

		public override BoltPlayer ToOriginal(Axlebolt.Bolt.Protobuf.Player proto)
		{
			return ToOriginal(proto, new BoltPlayer());
		}

		public ConcurrentDictionary<string, BoltPlayer> ToOriginalMap(MapField<string, Axlebolt.Bolt.Protobuf.Player> protoMap)
		{
			ConcurrentDictionary<string, BoltPlayer> concurrentDictionary = new ConcurrentDictionary<string, BoltPlayer>();
			if (protoMap == null)
			{
				return concurrentDictionary;
			}
			foreach (KeyValuePair<string, Axlebolt.Bolt.Protobuf.Player> item in protoMap)
			{
				concurrentDictionary[item.Key] = ToOriginal(item.Value);
			}
			return concurrentDictionary;
		}
	}
}
