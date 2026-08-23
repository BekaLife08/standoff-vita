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
			action(true, null);
		}

		public override void Logout()
		{
		}

		public override Task AuthenticateBolt()
		{
			return BoltApi.Instance.AuthTest("56c10982-cd05-4b9e-807e-af1e6154612f");
		}
	}
}
