using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement
{
	public class MovementControllerCmd : MessageBase
	{
		public float Horizontal;

		public float Vertical;

		public bool IsCrouching;

		public bool IsToJump;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Horizontal);
			writer.Write(Vertical);
			writer.Write(IsCrouching);
			writer.Write(IsToJump);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Horizontal = reader.ReadSingle();
			Vertical = reader.ReadSingle();
			IsCrouching = reader.ReadBoolean();
			IsToJump = reader.ReadBoolean();
		}
	}
}
