using System;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class View : MonoBehaviour
	{
		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				if (value)
				{
					Show();
				}
				else
				{
					Hide();
				}
			}
		}

		public event Action<bool> VisibleChanged;

		public virtual void Hide()
		{
			if (IsVisible)
			{
				base.gameObject.SetActive(value: false);
				OnVisibleChanged(isvisible: false);
			}
		}

		public virtual void Show()
		{
			if (!IsVisible)
			{
				base.gameObject.SetActive(value: true);
				OnVisibleChanged(isvisible: true);
			}
		}

		protected virtual void OnVisibleChanged(bool isvisible)
		{
			if (this.VisibleChanged != null)
			{
				this.VisibleChanged(isvisible);
			}
		}
	}
}
