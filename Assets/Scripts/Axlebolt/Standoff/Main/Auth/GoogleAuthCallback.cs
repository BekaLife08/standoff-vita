using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class GoogleAuthCallback : AndroidJavaProxy
	{
		public bool IsComplete
		{
			get;
			private set;
		}

		public string AuthCode
		{
			get;
			private set;
		}

		public string ErrorMessage
		{
			get;
			private set;
		}

		public bool IsSuccess
		{
			get;
			private set;
		}

		public GoogleAuthCallback()
			: base("com.axlebolt.bolt.android.playgames.GoogleSignInListener")
		{
		}

		public void onSuccess(string authCode)
		{
			IsSuccess = true;
			AuthCode = authCode;
			IsComplete = true;
		}

		public void onError(string errorMessage)
		{
			IsSuccess = false;
			ErrorMessage = errorMessage;
			IsComplete = true;
		}
	}
}
