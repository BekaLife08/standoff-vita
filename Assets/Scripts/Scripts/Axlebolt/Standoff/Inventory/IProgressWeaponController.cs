namespace Axlebolt.Standoff.Inventory
{
	public interface IProgressWeaponController
	{
		bool HasProgress();

		float GetProgress();

		float GetProgressTime();

		string GetProgressDisplayName();
	}
}
