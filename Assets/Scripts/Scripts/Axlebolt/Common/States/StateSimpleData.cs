using UnityEngine.Networking;

namespace Axlebolt.Common.States
{
	public class StateSimpleData : MessageBase
	{
		public byte stateNO;

		public byte statePrevNO;

		public float timeSwitched;

		public bool isjustSwitched;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32(stateNO);
			writer.WritePackedUInt32(statePrevNO);
			writer.Write(timeSwitched);
			writer.Write(isjustSwitched);
		}

		public override void Deserialize(NetworkReader reader)
		{
			stateNO = (byte)reader.ReadPackedUInt32();
			statePrevNO = (byte)reader.ReadPackedUInt32();
			timeSwitched = reader.ReadSingle();
			isjustSwitched = reader.ReadBoolean();
		}
	}
}
