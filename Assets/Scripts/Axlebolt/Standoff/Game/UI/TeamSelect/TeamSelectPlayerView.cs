using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.TeamSelect
{
	public class TeamSelectPlayerView : View
	{
		[NotNull]
		[SerializeField]
		private Image _avatarImage;

		[SerializeField]
		[NotNull]
		private Text _playerNameText;

		public void Refresh(PhotonPlayer photonPlayer)
		{
			_playerNameText.text = photonPlayer.NickName;
			_avatarImage.sprite = AvatarSupport.GetAvatar(photonPlayer);
		}
	}
}
