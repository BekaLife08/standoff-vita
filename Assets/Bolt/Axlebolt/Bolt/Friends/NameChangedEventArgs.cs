using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Friends
{
	public class NameChangedEventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CFriendId_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CNewName_003Ek__BackingField;

		public string FriendId
		{
			[CompilerGenerated]
			get
			{
				return _003CFriendId_003Ek__BackingField;
			}
		}

		public string NewName
		{
			[CompilerGenerated]
			get
			{
				return _003CNewName_003Ek__BackingField;
			}
		}

		public NameChangedEventArgs(string friendId, string newName)
		{
			_003CFriendId_003Ek__BackingField = friendId;
			_003CNewName_003Ek__BackingField = newName;
		}

		protected bool Equals(NameChangedEventArgs other)
		{
			return string.Equals(FriendId, other.FriendId) && string.Equals(NewName, other.NewName);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((NameChangedEventArgs)obj);
		}

		public override int GetHashCode()
		{
			return (((FriendId != null) ? FriendId.GetHashCode() : 0) * 397) ^ ((NewName != null) ? NewName.GetHashCode() : 0);
		}
	}
}
