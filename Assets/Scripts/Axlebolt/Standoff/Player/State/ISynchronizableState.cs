using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.State
{
	public interface ISynchronizableState
	{
		MessageBase GetParameters();

		void SetParameters(MessageBase parameters);
	}
}
