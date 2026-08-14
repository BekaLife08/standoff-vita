using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class DialogOnCancelListenerPoxy : AndroidJavaProxy
	{
		private readonly Action _onCancel;

		public DialogOnCancelListenerPoxy(Action onCancel)
			: base("android.content.DialogInterface$OnCancelListener")
		{
			_onCancel = onCancel;
		}

		private void onCancel(AndroidJavaObject dialog)
		{
			GoodiesSceneHelper.Queue(_onCancel);
		}
	}
}
