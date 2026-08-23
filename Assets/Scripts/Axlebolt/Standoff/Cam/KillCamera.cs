using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Cam
{
	public class KillCamera : Singleton<KillCamera>
	{
		private Camera _camera;

		public PlayerDieEffect PlayerDieEffect
		{
			get;
			private set;
		}

		public PlayerFocusEffect PlayerFocusEffect
		{
			get;
			private set;
		}

		private void Awake()
		{
			Camera main = Camera.main;
			PlayerDieEffect = main.gameObject.AddComponent<PlayerDieEffect>();
			PlayerFocusEffect = main.gameObject.AddComponent<PlayerFocusEffect>();
		}
	}
}
