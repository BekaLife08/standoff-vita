using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class DialogOnDismissListenerProxy : AndroidJavaProxy
	{
		private readonly Action _onDismiss;

		public DialogOnDismissListenerProxy(Action onDismiss)
			: base("android.content.DialogInterface$OnDismissListener")
		{
			_onDismiss = onDismiss;
		}

		private void onDismiss(AndroidJavaObject dialog)
		{
			GoodiesSceneHelper.Queue(_onDismiss);
		}
	}
}
