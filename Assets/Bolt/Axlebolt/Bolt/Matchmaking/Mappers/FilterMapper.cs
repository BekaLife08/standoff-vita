using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Matchmaking.Mappers
{
	public class FilterMapper : MessageMapper<Filter, BoltFilter>
	{
		public static readonly FilterMapper Instance = new FilterMapper();

		public override BoltFilter ToOriginal(Filter proto)
		{
			return new BoltFilter
			{
				Name = proto.Name,
				IntValue = proto.IntValue,
				FloatValue = proto.FloatValue,
				StringValue = proto.StringValue,
				Compare = EnumMapper<Comparison, BoltFilter.Comparison>.ToOriginal(proto.Comparison)
			};
		}

		public override Filter ToProto(BoltFilter original)
		{
			return new Filter
			{
				Name = original.Name,
				IntValue = original.IntValue,
				FloatValue = original.FloatValue,
				StringValue = original.StringValue,
				Comparison = EnumMapper<Comparison, BoltFilter.Comparison>.ToProto(original.Compare)
			};
		}
	}
}
