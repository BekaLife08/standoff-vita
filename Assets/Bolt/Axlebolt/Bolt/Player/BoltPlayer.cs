namespace Axlebolt.Bolt.Player
{
	public class BoltPlayer
	{
		public string Id { get; internal set; }

		public string Uid { get; internal set; }

		public string Name { get; internal set; }

		public string AvatarId { get; internal set; }

		public byte[] Avatar { get; internal set; }

		public int TimeInGame { get; internal set; }

		public PlayerStatus PlayerStatus { get; internal set; }

		public OnlineStatus OnlineStatus
		{
			get
			{
				return PlayerStatus.OnlineStatus;
			}
		}

		public PlayInGame PlayInGame
		{
			get
			{
				return PlayerStatus.PlayInGame;
			}
		}

		public BoltPlayer()
		{
		}

		public BoltPlayer(string id, string uid, string name, string avatarId, byte[] avatar)
		{
			Id = id;
			Uid = uid;
			Name = name;
			AvatarId = avatarId;
			Avatar = avatar;
		}

		protected bool Equals(BoltPlayer other)
		{
			return string.Equals(Id, other.Id);
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
			return Equals((BoltPlayer)obj);
		}

		public override int GetHashCode()
		{
			return (Id != null) ? Id.GetHashCode() : 0;
		}
	}
}
