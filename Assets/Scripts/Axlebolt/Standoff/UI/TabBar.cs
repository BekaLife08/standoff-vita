using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.UI
{
	public class TabBar : View
	{
		private readonly Dictionary<ITabController, TabButton> _tabControllers = new Dictionary<ITabController, TabButton>();

		public ITabController CurrentController
		{
			get;
			private set;
		}

		public void Add([NotNull] TabButton button, [NotNull] ITabController controller)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button");
			}
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			_tabControllers[controller] = button;
			controller.Init();
			button.Selected = false;
			button.NotificationCount = controller.GetNotificationsCount();
			button.Button.onClick.AddListener(delegate
			{
				Select(controller);
			});
			controller.NewNotificationEvent += delegate
			{
				button.NotificationCount = controller.GetNotificationsCount();
			};
		}

		public void Select([NotNull] ITabController controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			if (CurrentController != controller)
			{
				if (CurrentController != null && !controller.IsWindowLayout())
				{
					CurrentController.CanClose(delegate(bool canClose)
					{
						if (canClose)
						{
							CurrentController.Close();
							_tabControllers[CurrentController].Selected = false;
							Open(controller);
						}
					});
				}
				else
				{
					Open(controller);
				}
			}
		}

		private void Open(ITabController controller)
		{
			controller.Open();
			if (!controller.IsWindowLayout())
			{
				CurrentController = controller;
				_tabControllers[CurrentController].Selected = true;
			}
		}

		public void Unselect()
		{
			if (CurrentController != null)
			{
				CurrentController.Close();
				_tabControllers[CurrentController].Selected = false;
				CurrentController = null;
			}
		}
	}
}
