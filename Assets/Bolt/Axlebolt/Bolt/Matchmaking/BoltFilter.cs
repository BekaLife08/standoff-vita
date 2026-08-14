namespace Axlebolt.Bolt.Matchmaking
{
	public class BoltFilter
	{
		public enum Comparison
		{
			EqualToOrLessThan = 0,
			LessThan = 1,
			Equal = 2,
			GreaterThan = 3,
			EqualToOrGreaterThan = 4,
			NotEqual = 5
		}

		public string Name { get; set; }

		public int IntValue { get; set; }

		public float FloatValue { get; set; }

		public string StringValue { get; set; }

		public Comparison Compare { get; set; }
	}
}
