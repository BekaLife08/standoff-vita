using Axlebolt.Standoff.Common;

namespace Axlebolt.Standoff.Game.State
{
	public class GameStateHelper : ScenePhotonBehavior<GameStateHelper>
	{
		public bool IsRpcLocked
		{
			get;
			private set;
		}

		public bool IsRpcResult
		{
			get;
			private set;
		}

		public bool ServerCallback()
		{
			if (IsRpcResult)
			{
				return true;
			}
			if (IsRpcLocked)
			{
				return false;
			}
			IsRpcLocked = true;
			base.PhotonView.RPC("CallbackViaServer", PhotonTargets.AllViaServer);
			return false;
		}

		[PunRPC]
		private void CallbackViaServer(PhotonMessageInfo messageInfo)
		{
			if (messageInfo.sender.IsLocal)
			{
				IsRpcLocked = false;
				IsRpcResult = true;
			}
		}

		public override void OnInstantiate(object[] data)
		{
		}

		public override void OnReturnToPool()
		{
			ClearRpc();
		}

		public void ClearRpc()
		{
			IsRpcResult = false;
			IsRpcLocked = false;
		}

		public static double GetTimePassed()
		{
			return PhotonNetwork.time - PhotonNetwork.room.GetTime();
		}

		public static double GetTimeLeft(double duration)
		{
			double num = duration - GetTimePassed();
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}
	}
}
