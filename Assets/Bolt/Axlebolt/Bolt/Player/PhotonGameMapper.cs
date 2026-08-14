using System.Collections.Generic;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Player
{
	public class PhotonGameMapper : MessageMapper<PhotonGame, BoltPhotonGame>
	{
		public static readonly PhotonGameMapper Instance = new PhotonGameMapper();

		public override BoltPhotonGame ToOriginal(PhotonGame proto)
		{
			if (proto == null)
			{
				return null;
			}
			Dictionary<string, string> customProperties = new Dictionary<string, string>(proto.CustomProperties);
			return new BoltPhotonGame(proto.Region, proto.RoomId, proto.AppVersion)
			{
				CustomProperties = customProperties
			};
		}

		public override PhotonGame ToProto(BoltPhotonGame original)
		{
			return new PhotonGame
			{
				Region = original.Region,
				RoomId = original.RoomId,
				AppVersion = original.AppVersion
			};
		}
	}
}
