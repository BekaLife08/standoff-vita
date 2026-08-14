using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum MessageStatus
	{
		[OriginalName("Sent")]
		Sent = 0,
		[OriginalName("Received")]
		Received = 1,
		[OriginalName("Read")]
		Read = 2,
		[OriginalName("SentFailed")]
		SentFailed = 3
	}
}
