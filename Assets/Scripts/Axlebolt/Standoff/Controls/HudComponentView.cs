using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;

namespace Axlebolt.Standoff.Controls
{
	public abstract class HudComponentView : View, IHudComponentView
	{
		public abstract void SetPlayerController(PlayerController playerController);

		public abstract void UpdateView(PlayerController playerController);
	}
}
