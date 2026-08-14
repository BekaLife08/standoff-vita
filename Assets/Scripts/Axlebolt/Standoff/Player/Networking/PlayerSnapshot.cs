using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Weaponry;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Networking
{
	public class PlayerSnapshot : ObjectSnapshot
	{
		public MovementSnapshot movementSnapshot = new MovementSnapshot();

		public MecanimSnapshot mecanimSnapshot = new MecanimSnapshot();

		public AimSnapshot aimingSnapshot = new AimSnapshot();

		public WeaponrySnapshot WeaponrySnapshot = new WeaponrySnapshot();

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
			mecanimSnapshot.time = time;
			movementSnapshot.time = time;
			aimingSnapshot.time = time;
			WeaponrySnapshot.time = time;
			movementSnapshot.Deserialize(reader);
			mecanimSnapshot.Deserialize(reader);
			aimingSnapshot.Deserialize(reader);
			WeaponrySnapshot.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
			movementSnapshot.Serialize(writer);
			mecanimSnapshot.Serialize(writer);
			aimingSnapshot.Serialize(writer);
			WeaponrySnapshot.Serialize(writer);
		}
	}
}
