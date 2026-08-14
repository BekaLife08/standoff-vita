using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Player
{
	public class PlayInGameMapper : MessageMapper<Axlebolt.Bolt.Protobuf.PlayInGame, PlayInGame>
	{
		public static readonly PlayInGameMapper Instance = new PlayInGameMapper();

		public override PlayInGame ToOriginal(Axlebolt.Bolt.Protobuf.PlayInGame proto)
		{
			if (proto == null)
			{
				return null;
			}
			BoltPhotonGame boltPhotonGame = PhotonGameMapper.Instance.ToOriginal(proto.PhotonGame);
			return new PlayInGame(proto.GameCode, proto.GameVersion, proto.LobbyId, boltPhotonGame);
		}
	}
}
