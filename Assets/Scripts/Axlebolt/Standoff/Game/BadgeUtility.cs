using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game
{
	public class BadgeUtility
	{
		public static void SetBadge(Image badgeImage, PhotonPlayer player)
		{
			badgeImage.enabled = player.HasBadgeId();
			if (badgeImage.enabled)
			{
				badgeImage.sprite = Singleton<InventoryManager>.Instance.GetBadgeItemDefinition(player.GetBadgetId()).Sprite;
			}
		}
	}
}
