using Photon;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Common
{
	public class PhotonBehavior : PunBehaviour
	{
		public PhotonView PhotonView
		{
			[CompilerGenerated]
			get
			{
				return base.photonView;
			}
		}

		public static int ToOwnerId(int viewId)
		{
			return viewId / PhotonNetwork.MAX_VIEW_IDS;
		}

		public static bool IsLocalMessage(PhotonMessageInfo messageInfo)
		{
			return messageInfo.sender != null && messageInfo.sender.IsLocal;
		}
	}
}
