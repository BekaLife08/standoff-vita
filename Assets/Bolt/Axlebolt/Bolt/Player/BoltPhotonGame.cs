using System.Collections.Generic;

namespace Axlebolt.Bolt.Player
{
	public class BoltPhotonGame
	{
		public string Region { get; internal set; }

		public string RoomId { get; internal set; }

		public string AppVersion { get; internal set; }

		public Dictionary<string, string> CustomProperties { get; internal set; }

		public BoltPhotonGame(string region, string roomId, string appVersion)
		{
			Region = region;
			RoomId = roomId;
			AppVersion = appVersion;
			CustomProperties = new Dictionary<string, string>();
		}
	}
}
