using Axlebolt.Bolt;
using Axlebolt.Standoff.Core;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class GPGSAuthService : AuthService
	{
		[SerializeField]
		private Sprite _sprite;

		private static readonly Log Log = Log.Create(typeof(GPGSAuthService));

		private Action<bool, string> _action;

		private string _serverAuthCode;

		private GoogleAuthCallback _authCallback;

		public override Sprite Sprite
		{
			[CompilerGenerated]
			get
			{
				return _sprite;
			}
		}

		private void Awake()
		{
			base.enabled = false;
		}

		public override void Authenticate(Action<bool, string> action)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("{0} Authenticate", "GPGSAuthService"));
			}
			if (_action != null)
			{
				throw new InvalidOperationException("GPGSAuthService already in progress");
			}
			_action = action;
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.axlebolt.bolt.android.playgames.GoogleSignInActivity");
			_authCallback = new GoogleAuthCallback();
			androidJavaClass2.CallStatic("launch", @static, _authCallback);
			base.enabled = true;
		}

		private void Update()
		{
			if (!_authCallback.IsComplete)
			{
				return;
			}
			if (_authCallback.IsSuccess)
			{
				_serverAuthCode = _authCallback.AuthCode;
				if (!string.IsNullOrEmpty(_serverAuthCode))
				{
					Complete(true);
					return;
				}
				Log.Error("Can't get server auth code");
				Complete(false, "Can't get server auth code");
			}
			else
			{
				Complete(false, _authCallback.ErrorMessage);
			}
		}

		public override void Logout()
		{
		}

		private void Complete(bool result, string errorMsg = null)
		{
			Action<bool, string> action = _action;
			_action = null;
			_authCallback = null;
			base.enabled = false;
			action(result, errorMsg);
		}

		public override Task AuthenticateBolt()
		{
			return BoltApi.Instance.AuthGPGS(_serverAuthCode);
		}
	}
}
