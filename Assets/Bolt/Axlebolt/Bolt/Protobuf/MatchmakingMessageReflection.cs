using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class MatchmakingMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static MatchmakingMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChltYXRjaG1ha2luZ19tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9s" + "dBoUY29tbW9uX21lc3NhZ2UucHJvdG8aFWZyaWVuZHNfbWVzc2FnZS5wcm90" + "byL7AQoRR2FtZVNlcnZlckRldGFpbHMSCgoCaWQYASABKAkSMQoKZ2FtZVNl" + "cnZlchgCIAEoCzIdLmNvbS5heGxlYm9sdC5ib2x0LkdhbWVTZXJ2ZXISCwoD" + "bWFwGAMgASgJEhYKDmN1cnJlbnRQbGF5ZXJzGAQgASgFEhIKCm1heFBsYXll" + "cnMYBSABKAUSEgoKYm90UGxheWVycxgGIAEoBRIXCg9yZXF1aXJlUGFzc3dv" + "cmQYByABKAgSDwoHdmVyc2lvbhgIIAEoCRIaChJzdWNjZXNzZnVsUmVzcG9u" + "c2UYCSABKAgSFAoMZG9Ob3RSZWZyZXNoGAogASgIIrgDCgVMb2JieRIKCgJp" + "ZBgBIAEoCRIVCg1vd25lclBsYXllcklkGAIgASgJEgwKBG5hbWUYAyABKAkS" + "LwoJbG9iYnlUeXBlGAQgASgOMhwuY29tLmF4bGVib2x0LmJvbHQuTG9iYnlU" + "eXBlEhAKCGpvaW5hYmxlGAUgASgIEhIKCm1heE1lbWJlcnMYBiABKAUSMAoE" + "ZGF0YRgHIAMoCzIiLmNvbS5heGxlYm9sdC5ib2x0LkxvYmJ5LkRhdGFFbnRy" + "eRIwCgdtZW1iZXJzGAggAygLMh8uY29tLmF4bGVib2x0LmJvbHQuUGxheWVy" + "RnJpZW5kEjAKB2ludml0ZXMYCSADKAsyHy5jb20uYXhsZWJvbHQuYm9sdC5Q" + "bGF5ZXJGcmllbmQSMQoKZ2FtZVNlcnZlchgKIAEoCzIdLmNvbS5heGxlYm9s" + "dC5ib2x0LkdhbWVTZXJ2ZXISMQoKcGhvdG9uR2FtZRgLIAEoCzIdLmNvbS5h" + "eGxlYm9sdC5ib2x0LlBob3RvbkdhbWUaKwoJRGF0YUVudHJ5EgsKA2tleRgB" + "IAEoCRINCgV2YWx1ZRgCIAEoCToCOAEiZAoLTG9iYnlJbnZpdGUSDwoHbG9i" + "YnlJZBgBIAEoCRI2Cg1pbnZpdGVDcmVhdG9yGAIgASgLMh8uY29tLmF4bGVi" + "b2x0LmJvbHQuUGxheWVyRnJpZW5kEgwKBGRhdGUYAyABKAMqRQoJTG9iYnlU" + "eXBlEgsKB1BSSVZBVEUQABIQCgxGUklFTkRTX09OTFkQARIKCgZQVUJMSUMQ" + "AhINCglJTlZJU0lCTEUQAypFChNMb2JieURpc3RhbmNlRmlsdGVyEgkKBUNM" + "T1NFEAASCwoHREVGQVVMVBABEgcKA0ZBUhACEg0KCVdPUkxEV0lERRADQjUK" + "GmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Q" + "cm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				CommonMessageReflection.Descriptor,
				FriendsMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(LobbyType),
				typeof(LobbyDistanceFilter)
			}, new GeneratedClrTypeInfo[3]
			{
				new GeneratedClrTypeInfo(typeof(GameServerDetails), GameServerDetails.Parser, new string[10] { "Id", "GameServer", "Map", "CurrentPlayers", "MaxPlayers", "BotPlayers", "RequirePassword", "Version", "SuccessfulResponse", "DoNotRefresh" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Lobby), Lobby.Parser, new string[11]
				{
					"Id", "OwnerPlayerId", "Name", "LobbyType", "Joinable", "MaxMembers", "Data", "Members", "Invites", "GameServer",
					"PhotonGame"
				}, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(LobbyInvite), LobbyInvite.Parser, new string[3] { "LobbyId", "InviteCreator", "Date" }, null, null, null)
			}));
		}
	}
}
