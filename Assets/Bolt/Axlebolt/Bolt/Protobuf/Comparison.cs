using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum Comparison
	{
		[OriginalName("EQUAL_TO_OR_LESS_THAN")]
		EqualToOrLessThan = 0,
		[OriginalName("LESS_THAN")]
		LessThan = 1,
		[OriginalName("EQUAL")]
		Equal = 2,
		[OriginalName("GREATER_THAN")]
		GreaterThan = 3,
		[OriginalName("EQUAL_TO_OR_GREATER_THAN")]
		EqualToOrGreaterThan = 4,
		[OriginalName("NOT_EQUAL")]
		NotEqual = 5
	}
}
