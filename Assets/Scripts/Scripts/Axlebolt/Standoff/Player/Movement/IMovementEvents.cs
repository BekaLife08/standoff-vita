namespace Axlebolt.Standoff.Player.Movement
{
	public interface IMovementEvents
	{
		void OnMovementInit(MovementController movementController);

		void OnJump(MovementController movementController);

		void OnLand(MovementController movementController, float fallDuration);

		void OnCrouch(MovementController movementController);

		void OnStand(MovementController movementController);
	}
}
