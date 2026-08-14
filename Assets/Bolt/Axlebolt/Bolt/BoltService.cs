namespace Axlebolt.Bolt
{
	public abstract class BoltService<T> : Service where T : BoltService<T>
	{
		public static T Instance { get; internal set; }

		protected BoltService()
		{
			Instance = (T)this;
		}

		internal override void Destroy()
		{
			Instance = null;
		}
	}
}
