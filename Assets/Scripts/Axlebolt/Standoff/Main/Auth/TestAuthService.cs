using Axlebolt.Bolt;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class TestAuthService : AuthService
	{
		[SerializeField]
		private Sprite _sprite;

		public override Sprite Sprite
		{
			[CompilerGenerated]
			get
			{
				return _sprite;
			}
		}

		public override void Authenticate(Action<bool, string> action)
		{
#if UNITY_PSP2
			// PS Vita: never touch online services — instant local success.
			if (action != null)
			{
				action(true, null);
			}
			return;
#endif
			action(true, null);
		}

		public override void Logout()
		{
		}

		public override Task AuthenticateBolt()
		{
#if UNITY_PSP2
			// PS Vita: NO Task API here (no Task.FromResult / await / async —
			// IL2CPP pthread pool is not ready, it hard-crashes C2-12828-1).
			// Plain synchronous return. This method is unreachable on Vita
			// anyway (AuthController short-circuits to CallbackResult), so
			// null is never dereferenced; signature stays Task only because
			// the abstract AuthService declares it.
			return null;
#endif
			return BoltApi.Instance.AuthTest("56c10982-cd05-4b9e-807e-af1e6154612f");
		}
	}
}
