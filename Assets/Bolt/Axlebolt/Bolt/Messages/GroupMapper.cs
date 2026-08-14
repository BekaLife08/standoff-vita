using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Messages
{
	public class GroupMapper : MessageMapper<Group, BoltGroup>
	{
		public static readonly GroupMapper Instance = new GroupMapper();

		public override BoltGroup ToOriginal(Group proto)
		{
			return new BoltGroup
			{
				Id = proto.Id,
				Name = proto.Name,
				AvatarId = proto.AvatarId,
				Players = PlayerMapper.Instance.ToOriginalList(proto.Players)
			};
		}
	}
}
