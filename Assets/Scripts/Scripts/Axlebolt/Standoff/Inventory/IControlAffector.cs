namespace Axlebolt.Standoff.Inventory
{
	public interface IControlAffector
	{
		bool IsMovementLocked();

		bool IsFiringLocked();

		bool IsDropLocked();
	}
}
