using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Matchmaking.Mappers
{
	public class GameServerMapper : MessageMapper<GameServer, BoltGameServer>
	{
		public static readonly GameServerMapper Instance = new GameServerMapper();

		public override BoltGameServer ToOriginal(GameServer proto)
		{
			return (proto == null) ? null : new BoltGameServer(proto.Id, proto.Ip, proto.Port);
		}
	}
}
