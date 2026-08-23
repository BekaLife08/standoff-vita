using System;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Unity
{
	[Serializable]
	public class BoltNotification
	{
		public const int EventFriendshipRequest = 1;

		public const int EventLobbyInvite = 3;

		public const int EventFriendMessage = 5;

		public const int EventGroupMessage = 6;

		public int boltType;

		public string boltFriendId;

		public string boltLobbyId;

		public string boltMessage;

		public string FriendId
		{
			[CompilerGenerated]
			get
			{
				return boltFriendId;
			}
		}

		public string LobbyId
		{
			[CompilerGenerated]
			get
			{
				return boltLobbyId;
			}
		}

		public string BoltMessage
		{
			[CompilerGenerated]
			get
			{
				return boltMessage;
			}
		}

		public bool IsLobbyInvite()
		{
			return boltType == 3;
		}

		public bool IsFriendshipRequest()
		{
			return boltType == 1;
		}

		public bool IsFriendMessage()
		{
			return boltType == 5;
		}
	}
}
