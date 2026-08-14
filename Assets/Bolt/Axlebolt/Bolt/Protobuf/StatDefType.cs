using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum StatDefType
	{
		[OriginalName("INT")]
		Int = 0,
		[OriginalName("FLOAT")]
		Float = 1,
		[OriginalName("AVGRATE")]
		Avgrate = 2
	}
}
