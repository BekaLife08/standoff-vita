using System.Collections.Generic;
using Axlebolt.Bolt.Player;

namespace Axlebolt.Bolt.Messages
{
	public class BoltGroup
	{
		public string Id { get; internal set; }

		public string Name { get; internal set; }

		public string AvatarId { get; internal set; }

		public List<BoltPlayer> Players { get; internal set; }
	}
}
