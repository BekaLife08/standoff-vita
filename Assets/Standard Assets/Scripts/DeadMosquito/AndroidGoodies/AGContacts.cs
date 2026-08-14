using DeadMosquito.AndroidGoodies.Internal;
using System;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGContacts
	{
		private static Action<ContactPickResult> _onSuccessAction;

		private static Action<string> _onCancelAction;

		public static void PickContact(Action<ContactPickResult> onSuccess, Action<string> onError)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				Check.Argument.IsNotNull(onError, "onError", string.Empty);
				_onSuccessAction = onSuccess;
				_onCancelAction = onError;
				AGActivityUtils.PickContact();
			}
		}

		public static void OnSuccessTrigger(string message)
		{
			if (_onSuccessAction != null)
			{
				ContactPickResult obj = ContactPickResult.FromJson(message);
				_onSuccessAction(obj);
			}
		}

		public static void OnErrorTrigger(string message)
		{
			if (_onCancelAction != null)
			{
				_onCancelAction(message);
				_onCancelAction = null;
			}
		}
	}
}
