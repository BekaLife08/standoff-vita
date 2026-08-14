using UnityEngine;

namespace PlayerControl
{
	public class PlayerDebugTool : MonoBehaviour
	{
		public static PlayerDebugTool Insance;

		public UI_PlayerDebug playerInfoPanel;

		private void Awake()
		{
			Insance = this;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
