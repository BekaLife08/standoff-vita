using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public abstract class AuthService : MonoBehaviour
	{
		public abstract Sprite Sprite
		{
			get;
		}

		public abstract void Authenticate(Action<bool, string> action);

		public abstract void Logout();

		public abstract Task AuthenticateBolt();
	}
}
