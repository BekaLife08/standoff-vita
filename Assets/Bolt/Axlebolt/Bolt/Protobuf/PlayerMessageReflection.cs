using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class PlayerMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor
		{
			get
			{
				return descriptor;
			}
		}

		static PlayerMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChRwbGF5ZXJfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQaFGNv" + "bW1vbl9tZXNzYWdlLnByb3RvIncKClBsYXlJbkdhbWUSEAoIZ2FtZUNvZGUY" + "ASABKAkSEwoLZ2FtZVZlcnNpb24YAiABKAkSDwoHbG9iYnlJZBgDIAEoCRIx" + "CgpwaG90b25HYW1lGAQgASgLMh0uY29tLmF4bGVib2x0LmJvbHQuUGhvdG9u" + "R2FtZSKKAQoMUGxheWVyU3RhdHVzEhAKCHBsYXllcklkGAEgASgJEjEKCnBs" + "YXlJbkdhbWUYAiABKAsyHS5jb20uYXhsZWJvbHQuYm9sdC5QbGF5SW5HYW1l" + "EjUKDG9ubGluZVN0YXR1cxgDIAEoDjIfLmNvbS5heGxlYm9sdC5ib2x0Lk9u" + "bGluZVN0YXR1cyKgAQoGUGxheWVyEgoKAmlkGAEgASgJEgsKA3VpZBgCIAEo" + "CRIMCgRuYW1lGAMgASgJEhAKCGF2YXRhcklkGAQgASgJEhIKCnRpbWVJbkdh" + "bWUYBSABKAUSNQoMcGxheWVyU3RhdHVzGAYgASgLMh8uY29tLmF4bGVib2x0" + "LmJvbHQuUGxheWVyU3RhdHVzEhIKCmxvZ291dERhdGUYByABKAMqTwoIQXV0" + "aFR5cGUSCAoEVEVTVBAAEgkKBUdVRVNUEAESDwoLR09PR0xFX1BMQVkQAhIP" + "CgtHQU1FX0NFTlRFUhADEgwKCEZBQ0VCT09LEAQqkQEKDE9ubGluZVN0YXR1" + "cxIQCgxTdGF0ZU9mZmxpbmUQABIPCgtTdGF0ZU9ubGluZRABEg0KCVN0YXRl" + "QnVzeRACEg0KCVN0YXRlQXdheRADEg8KC1N0YXRlU25vb3plEAQSFwoTU3Rh" + "dGVMb29raW5nVG9UcmFkZRAFEhYKElN0YXRlTG9va2luZ1RvUGxheRAGQjUK" + "GmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Q" + "cm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { CommonMessageReflection.Descriptor }, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(AuthType),
				typeof(OnlineStatus)
			}, new GeneratedClrTypeInfo[3]
			{
				new GeneratedClrTypeInfo(typeof(PlayInGame), PlayInGame.Parser, new string[4] { "GameCode", "GameVersion", "LobbyId", "PhotonGame" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PlayerStatus), PlayerStatus.Parser, new string[3] { "PlayerId", "PlayInGame", "OnlineStatus" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Player), Player.Parser, new string[7] { "Id", "Uid", "Name", "AvatarId", "TimeInGame", "PlayerStatus", "LogoutDate" }, null, null, null)
			}));
		}
	}
}
