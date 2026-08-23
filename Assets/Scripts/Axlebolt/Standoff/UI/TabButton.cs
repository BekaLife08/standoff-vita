using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public abstract class TabButton : View
	{
		public abstract bool Selected
		{
			set;
		}

		public abstract int NotificationCount
		{
			set;
		}

		public abstract Button Button
		{
			get;
		}
	}
}
