using Axlebolt.Standoff.UI;
using I2.Loc;
using System;
using System.Collections;

namespace Axlebolt.Standoff.Settings
{
	public abstract class SettingsTabController<T> : TabController<T>, ISettingsTabController, ITabController where T : TabController<T>
	{
		private bool _isDirty;

		public Action<bool> DirtyChangedEvent
		{
			get;
			set;
		}

		public void Apply()
		{
			Apply(null);
		}

		public void Apply(Action callback)
		{
			StartCoroutine(ApplyCoroutine(callback));
		}

		private IEnumerator ApplyCoroutine(Action callback)
		{
			Dialog dialog = Dialogs.Create(ScriptLocalization.Dialogs.Processing, ScriptLocalization.Common.PleaseWait);
			dialog.Show();
			yield return null;
			yield return ApplyInternal();
			SetDirty(isDirty: false);
			dialog.Hide();
			callback?.Invoke();
		}

		protected abstract IEnumerator ApplyInternal();

		public abstract void ResetDefaults();

		public override void CanClose(Action<bool> callback)
		{
			if (_isDirty)
			{
				DialogButton yesButton = new DialogButton(ScriptLocalization.Common.Yes);
				DialogButton noButton = new DialogButton(ScriptLocalization.Common.No);
				Dialog dialog = Dialogs.Create(ScriptLocalization.Common.Confirmation, ScriptLocalization.Common.DoYouWantApplyChanges, yesButton, noButton, Dialogs.Cancel);
				dialog.Show(delegate(DialogButton btn)
				{
					if (btn == yesButton)
					{
						Apply(delegate
						{
							callback(obj: true);
						});
					}
					else
					{
						callback(btn == noButton);
					}
				});
			}
			else
			{
				callback(obj: true);
			}
		}

		protected virtual void OnDirty()
		{
			SetDirty(isDirty: true);
		}

		protected virtual void SetDirty(bool isDirty)
		{
			_isDirty = isDirty;
			DirtyChangedEvent?.Invoke(_isDirty);
		}

		public override void OnOpen()
		{
			SetDirty(isDirty: false);
		}
	}
}
