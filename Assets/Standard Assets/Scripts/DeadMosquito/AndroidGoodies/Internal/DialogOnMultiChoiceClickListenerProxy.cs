using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class DialogOnMultiChoiceClickListenerProxy : AndroidJavaProxy
	{
		private readonly Action<int, bool> _onClick;

		public DialogOnMultiChoiceClickListenerProxy(Action<int, bool> onClick)
			: base("android.content.DialogInterface$OnMultiChoiceClickListener")
		{
			_onClick = onClick;
		}

		private void onClick(AndroidJavaObject dialog, int which, bool isChecked)
		{
			GoodiesSceneHelper.Queue(delegate
			{
				_onClick(which, isChecked);
			});
		}
	}
}
