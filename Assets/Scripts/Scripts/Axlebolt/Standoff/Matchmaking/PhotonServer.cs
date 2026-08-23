namespace Axlebolt.Standoff.Matchmaking
{
	public class PhotonServer
	{
		public string Ip
		{
			get;
		}

		public string Location
		{
			get;
		}

		public PhotonServer(string location, string ip)
		{
			Location = location;
			Ip = ip;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", "Ip", Ip, "Location", Location);
		}
	}
}
