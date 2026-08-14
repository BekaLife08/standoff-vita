using ExitGames.Client.Photon;

public static class HashtableExtension
{
	public static bool TryGetNonNullValue(this Hashtable hashtable, string key, out object result)
	{
		if (hashtable.TryGetValue(key, out result))
		{
			if (result == null)
			{
				return false;
			}
			return true;
		}
		return false;
	}
}
