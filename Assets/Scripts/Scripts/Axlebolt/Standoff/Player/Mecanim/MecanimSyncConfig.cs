using Unity;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimSyncConfig : MessageBase
	{
		public MecanimTransitionInfo[] transitionInfo;

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayMecanimTransitionInfo_None(writer, transitionInfo);
		}

		public override void Deserialize(NetworkReader reader)
		{
			transitionInfo = GeneratedNetworkCode._ReadArrayMecanimTransitionInfo_None(reader);
		}
	}
}
