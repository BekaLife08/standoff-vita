using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public static class BombPhotonExtension
	{
		public const string BombProp = "bomberId";

		public static void SetInitBomberId(this Hashtable hashtable, int id)
		{
			hashtable["bomberId"] = id;
		}

		public static void ClearInitBomberId(this Room room)
		{
			room.SetCustomProperties(new Hashtable
			{
				{
					"bomberId",
					null
				}
			});
		}

		public static int GetInitBomberId(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("bomberId", out object result))
			{
				return (int)result;
			}
			return -1;
		}
	}
}
