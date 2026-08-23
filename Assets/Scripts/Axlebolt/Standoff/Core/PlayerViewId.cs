using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Core
{
	public class PlayerViewId : MessageBase
	{
		public int Id
		{
			get;
			set;
		}

		public int ViewId
		{
			get;
			set;
		}

		public bool IsMe
		{
			[CompilerGenerated]
			get
			{
				return Id == PhotonNetwork.player.ID;
			}
		}

		public bool IsInactive => PhotonPlayer.Find(Id)?.IsInactive ?? true;

		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", "Id", Id, "ViewId", ViewId);
		}

		protected bool Equals(PlayerViewId other)
		{
			return Id == other.Id && ViewId == other.ViewId;
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
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((PlayerViewId)obj);
		}

		public override int GetHashCode()
		{
			return (Id * 397) ^ ViewId;
		}

		public override void Serialize(NetworkWriter writer)
		{
		}

		public override void Deserialize(NetworkReader reader)
		{
		}
	}
}
