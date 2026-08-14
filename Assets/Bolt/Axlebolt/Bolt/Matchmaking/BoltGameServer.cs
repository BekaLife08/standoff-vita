namespace Axlebolt.Bolt.Matchmaking
{
	public class BoltGameServer
	{
		public string Id { get; internal set; }

		public string IP { get; internal set; }

		public int Port { get; internal set; }

		public BoltGameServer(string id, string ip, int port)
		{
			Id = id;
			IP = ip;
			Port = port;
		}
	}
}
