using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Player.Occlusion;
using Axlebolt.Standoff.Player.Weaponry;

namespace Axlebolt.Standoff.Player
{
	public class PlayerInterpolator : Interpolator
	{
		public MecanimInterpolator mecanimInterpolator = new MecanimInterpolator();

		public AimInterpolator aimInterpolator = new AimInterpolator();

		public MovementInterpolator movementInterpolator = new MovementInterpolator();

		public InventoryInterpolator inventoryInterpolator = new InventoryInterpolator();

		private PlayerSnapshot fromSnapshot;

		private PlayerSnapshot toSnapshot;

		private readonly PlayerSnapshot resultSnapshot = new PlayerSnapshot();

		private ObservationState _observationState;

		public ObservationState ObsrvationState
		{
			get
			{
				return _observationState;
			}
			set
			{
				_observationState = value;
			}
		}

		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			fromSnapshot = (PlayerSnapshot)a;
			toSnapshot = (PlayerSnapshot)b;
			fromSnapshot.WeaponrySnapshot.time = fromSnapshot.time;
			toSnapshot.WeaponrySnapshot.time = toSnapshot.time;
			resultSnapshot.mecanimSnapshot = (MecanimSnapshot)mecanimInterpolator.Interpolate(fromSnapshot.mecanimSnapshot, toSnapshot.mecanimSnapshot, progress);
			if (_observationState == ObservationState.Visible)
			{
				resultSnapshot.aimingSnapshot = (AimSnapshot)aimInterpolator.Interpolate(fromSnapshot.aimingSnapshot, toSnapshot.aimingSnapshot, progress);
			}
			resultSnapshot.WeaponrySnapshot = (WeaponrySnapshot)inventoryInterpolator.Interpolate(fromSnapshot.WeaponrySnapshot, toSnapshot.WeaponrySnapshot, progress);
			resultSnapshot.movementSnapshot = (MovementSnapshot)movementInterpolator.Interpolate(fromSnapshot.movementSnapshot, toSnapshot.movementSnapshot, progress);
			return resultSnapshot;
		}
	}
}
