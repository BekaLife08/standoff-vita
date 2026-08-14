using System;

namespace Heyzap
{
	[Serializable]
	public class VirtualCurrencyResponse
	{
		public string LatestTransactionID;

		public string CurrencyID;

		public string CurrencyName;

		public float DeltaOfCurrency;
	}
}
