using System;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public abstract class TabController<T> : MonoBehaviour, ITabController where T : TabController<T>
	{
		public static T Instance
		{
			get;
			private set;
		}

		public bool IsOpen
		{
			get;
			private set;
		}

		public event Action<bool> OpenStateChangedEvent;

		public event Action NewNotificationEvent;

		public virtual bool IsWindowLayout()
		{
			return false;
		}

		public virtual void Init()
		{
			Instance = (T)this;
			base.gameObject.SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}

		public virtual void Open()
		{
			if (!((UnityEngine.Object)Instance == (UnityEngine.Object)null))
			{
				base.gameObject.SetActive(value: true);
				OnOpen();
				IsOpen = true;
				OnOpenStateChangedEvent();
			}
		}

		public abstract void OnOpen();

		public virtual void Close()
		{
			if (!((UnityEngine.Object)Instance == (UnityEngine.Object)null))
			{
				OnClose();
				base.gameObject.SetActive(value: false);
				IsOpen = false;
				OnOpenStateChangedEvent();
			}
		}

		public virtual void CanClose(Action<bool> callback)
		{
			callback(obj: true);
		}

		public virtual int GetNotificationsCount()
		{
			return 0;
		}

		public abstract void OnClose();

		protected virtual void OnNewNotificationEvent()
		{
			if (this.NewNotificationEvent != null)
			{
				this.NewNotificationEvent();
			}
		}

		protected virtual void OnDestroy()
		{
			if (IsOpen)
			{
				OnClose();
			}
			Instance = (T)null;
		}

		protected virtual void OnOpenStateChangedEvent()
		{
			if (this.OpenStateChangedEvent != null)
			{
				this.OpenStateChangedEvent(IsOpen);
			}
		}
	}
}
