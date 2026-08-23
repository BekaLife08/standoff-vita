namespace Axlebolt.Standoff.Game.UI
{
	public interface IPlayerPropSensitiveView
	{
		bool IsVisible
		{
			get;
		}

		string[] SensitivePlayerProperties
		{
			get;
		}

		void Refresh();
	}
}
