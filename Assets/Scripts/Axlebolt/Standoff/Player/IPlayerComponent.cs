namespace Axlebolt.Standoff.Player
{
	public interface IPlayerComponent
	{
		void PreInitialize();

		void Initialize();

		void OnInstantiated();
	}
}
