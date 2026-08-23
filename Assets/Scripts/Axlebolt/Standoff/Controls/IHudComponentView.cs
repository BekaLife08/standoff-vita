using Axlebolt.Standoff.Player;

namespace Axlebolt.Standoff.Controls
{
	public interface IHudComponentView
	{
		void SetPlayerController(PlayerController playerController);

		void UpdateView(PlayerController playerController);
	}
}
