using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Messages;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatModel
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly BoltChat _003CChat_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltLobbyInvite _003CLobbyInvite_003Ek__BackingField;

		public string Id
		{
			[CompilerGenerated]
			get
			{
				return (!Chat.IsGroup) ? Chat.Friend.Id : Chat.Group.Id;
			}
		}

		public BoltFriend Friend
		{
			[CompilerGenerated]
			get
			{
				return Chat.Friend;
			}
		}

		public string Message
		{
			[CompilerGenerated]
			get
			{
				return Chat.Message;
			}
		}

		public long Timestamp
		{
			[CompilerGenerated]
			get
			{
				return (!HasLobbyInvite) ? Chat.Timestamp : LobbyInvite.Timestamp;
			}
		}

		public int UnreadMsgsCount
		{
			[CompilerGenerated]
			get
			{
				return Chat.UnreadMsgsCount;
			}
		}

		public bool HasLobbyInvite
		{
			[CompilerGenerated]
			get
			{
				return LobbyInvite != null;
			}
		}

		public BoltChat Chat
		{
			[CompilerGenerated]
			get
			{
				return _003CChat_003Ek__BackingField;
			}
		}

		public BoltLobbyInvite LobbyInvite
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyInvite_003Ek__BackingField;
			}
		}

		public ChatModel([NotNull] BoltChat chat, BoltLobbyInvite lobbyInvite)
		{
			if (chat == null)
			{
				throw new ArgumentNullException("chat");
			}
			_003CChat_003Ek__BackingField = chat;
			_003CLobbyInvite_003Ek__BackingField = lobbyInvite;
		}

		protected bool Equals(ChatModel other)
		{
			return object.Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			return obj.GetType() == GetType() && Equals((ChatModel)obj);
		}

		public override int GetHashCode()
		{
			return (Id != null) ? Id.GetHashCode() : 0;
		}
	}
}
