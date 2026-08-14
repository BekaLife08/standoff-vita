namespace Axlebolt.Bolt.Friends
{
	public class AvatarEventArgs
	{
		public string FriendId { get; set; }

		public string AvatarId { get; set; }

		public AvatarEventArgs(string friendId, string avatarId)
		{
			FriendId = friendId;
			AvatarId = avatarId;
		}

		protected bool Equals(AvatarEventArgs other)
		{
			return string.Equals(FriendId, other.FriendId) && string.Equals(AvatarId, other.AvatarId);
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
			return Equals((AvatarEventArgs)obj);
		}

		public override int GetHashCode()
		{
			return (((FriendId != null) ? FriendId.GetHashCode() : 0) * 397) ^ ((AvatarId != null) ? AvatarId.GetHashCode() : 0);
		}
	}
}
