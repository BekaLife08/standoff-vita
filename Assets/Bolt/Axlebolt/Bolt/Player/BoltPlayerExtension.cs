namespace Axlebolt.Bolt.Player
{
	public static class BoltPlayerExtension
	{
		public static bool IsLocal(this BoltPlayer player)
		{
			return BoltService<BoltPlayerService>.Instance.Player.Id == player.Id;
		}
	}
}
