using Axlebolt.Standoff.Settings.Game;
using I2.Loc;
using System.Collections;
using System.Linq;

namespace Axlebolt.Standoff.Matchmaking
{
	public static class Regions
	{
		public const string Europe = "fra";

		public const string Usa = "mia";

		public const string Asia = "tok";

		public static readonly PhotonServer[] Servers = new PhotonServer[3]
		{
			new PhotonServer("fra", "91.108.240.118"),
			new PhotonServer("mia", "91.199.81.130"),
			new PhotonServer("tok", "108.61.179.152")
		};

		public static string GetDisplayName(this PhotonServer photonServer)
		{
			switch (photonServer.Location)
			{
			case "fra":
				return ScriptLocalization.Regions.Europe;
			case "mia":
				return ScriptLocalization.Regions.Usa;
			case "tok":
				return ScriptLocalization.Regions.Asia;
			default:
				return photonServer.Location;
			}
		}

		public static PhotonServer GetCurrentRegion()
		{
			string region = GameSettingsManager.Instance.Model.Region;
			return GetRegion(region);
		}

		public static PhotonServer GetRegion(string region)
		{
			return (!string.IsNullOrEmpty(region)) ? Servers.FirstOrDefault((PhotonServer server) => server.Location == region) : null;
		}

		public static IEnumerator SetCurrentRegion(PhotonServer server)
		{
			GameSettings model = GameSettingsManager.Instance.Model;
			model.Region = server.Location;
			yield return GameSettingsManager.Instance.Save(model);
		}
	}
}
