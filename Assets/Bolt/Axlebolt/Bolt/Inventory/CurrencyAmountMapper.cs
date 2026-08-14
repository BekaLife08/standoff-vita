using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Inventory
{
	public class CurrencyAmountMapper : MessageMapper<CurrencyAmount, BoltCurrencyAmount>
	{
		public static readonly CurrencyAmountMapper Instancce = new CurrencyAmountMapper();

		public override BoltCurrencyAmount ToOriginal(CurrencyAmount proto)
		{
			return new BoltCurrencyAmount
			{
				CurrencyId = proto.CurrencyId,
				Value = proto.Value
			};
		}

		public override CurrencyAmount ToProto(BoltCurrencyAmount bca)
		{
			return new CurrencyAmount
			{
				CurrencyId = bca.CurrencyId,
				Value = bca.Value
			};
		}
	}
}
