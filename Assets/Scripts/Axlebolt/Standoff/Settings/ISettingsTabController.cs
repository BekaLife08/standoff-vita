using Axlebolt.Standoff.UI;
using System;

namespace Axlebolt.Standoff.Settings
{
	public interface ISettingsTabController : ITabController
	{
		Action<bool> DirtyChangedEvent
		{
			get;
			set;
		}

		void Apply();

		void ResetDefaults();
	}
}
