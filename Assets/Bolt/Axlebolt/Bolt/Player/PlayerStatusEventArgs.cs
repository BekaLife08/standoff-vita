using Axlebolt.Bolt.Friends;

namespace Axlebolt.Bolt.Player
{
	public class PlayerStatusEventArgs
	{
		public PlayerStatus NewPlayerStatus { get; set; }

		public BoltFriend Friend { get; set; }

		protected bool Equals(PlayerStatusEventArgs other)
		{
			return string.Equals(Friend.Id, other.Friend.Id) && object.Equals(NewPlayerStatus, other.NewPlayerStatus);
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
			return obj.GetType() == GetType() && Equals((PlayerStatusEventArgs)obj);
		}

		public override int GetHashCode()
		{
			return (((Friend.Id != null) ? Friend.Id.GetHashCode() : 0) * 397) ^ ((NewPlayerStatus != null) ? NewPlayerStatus.GetHashCode() : 0);
		}
	}
}
