namespace Axlebolt.Bolt
{
	public abstract class Service
	{
		internal virtual void BindEvents()
		{
		}

		internal abstract void Load();

		internal virtual void UnbindEvents()
		{
		}

		internal virtual void Unload()
		{
		}

		internal abstract void Destroy();
	}
}
