using Axlebolt.Common.States;
using Axlebolt.Networking;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Aim
{
	public class AimSnapshot : ObjectSnapshot
	{
		public AimingData aimingData = new AimingData();

		public StateSimpleData weaponOffsetState = new StateSimpleData();

		public StateSimpleData moveState = new StateSimpleData();

		public override void Deserialize(NetworkReader reader)
		{
			aimingData.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			aimingData.Serialize(writer);
		}
	}
}
