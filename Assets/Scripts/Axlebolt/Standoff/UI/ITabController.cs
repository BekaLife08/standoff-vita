using System;

namespace Axlebolt.Standoff.UI
{
	public interface ITabController
	{
		event Action NewNotificationEvent;

		bool IsWindowLayout();

		void Init();

		void Open();

		void Close();

		void CanClose(Action<bool> callback);

		int GetNotificationsCount();
	}
}
