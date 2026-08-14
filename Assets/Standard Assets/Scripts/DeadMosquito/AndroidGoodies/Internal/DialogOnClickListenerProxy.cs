using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class DialogOnClickListenerProxy : AndroidJavaProxy
	{
		private const string InterfaceSignature = "android.content.DialogInterface$OnClickListener";

		private readonly Action _onClickVoid;

		private readonly Action<int> _onClickInt;

		private readonly bool _dismissOnClick;

		public DialogOnClickListenerProxy(Action onClick)
			: base("android.content.DialogInterface$OnClickListener")
		{
			_onClickVoid = onClick;
		}

		public DialogOnClickListenerProxy(Action<int> onClick, bool dismissOnClick = false)
			: base("android.content.DialogInterface$OnClickListener")
		{
			_onClickInt = onClick;
			_dismissOnClick = dismissOnClick;
		}

		public void onClick(AndroidJavaObject dialog, int which)
		{
			if (_onClickVoid != null)
			{
				GoodiesSceneHelper.Queue(_onClickVoid);
			}
			if (_onClickInt != null)
			{
				GoodiesSceneHelper.Queue(delegate
				{
					_onClickInt(which);
				});
			}
			if (_dismissOnClick)
			{
				GoodiesSceneHelper.Queue(delegate
				{
					dialog.Call("dismiss");
				});
			}
		}
	}
}
