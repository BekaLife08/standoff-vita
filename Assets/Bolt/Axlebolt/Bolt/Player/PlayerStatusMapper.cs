using System;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Player
{
	public class PlayerStatusMapper : MessageMapper<Axlebolt.Bolt.Protobuf.PlayerStatus, PlayerStatus>
	{
		public static readonly PlayerStatusMapper Instance = new PlayerStatusMapper();

		public override PlayerStatus ToOriginal(Axlebolt.Bolt.Protobuf.PlayerStatus proto)
		{
			if (proto == null)
			{
				throw new ArgumentNullException("proto");
			}
			return new PlayerStatus
			{
				PlayInGame = PlayInGameMapper.Instance.ToOriginal(proto.PlayInGame),
				OnlineStatus = EnumMapper<Axlebolt.Bolt.Protobuf.OnlineStatus, OnlineStatus>.ToOriginal(proto.OnlineStatus)
			};
		}
	}
}
