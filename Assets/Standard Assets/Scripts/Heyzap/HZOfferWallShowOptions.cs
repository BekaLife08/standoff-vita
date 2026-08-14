namespace Heyzap
{
	public class HZOfferWallShowOptions : HZShowOptions
	{
		private const bool DEFAULT_SHOULD_CLOSE_AFTER_FIRST_CLICK = true;

		private bool shouldCloseAfterFirstClick = true;

		public bool ShouldCloseAfterFirstClick
		{
			get
			{
				return shouldCloseAfterFirstClick;
			}
			set
			{
				shouldCloseAfterFirstClick = value;
			}
		}
	}
}
