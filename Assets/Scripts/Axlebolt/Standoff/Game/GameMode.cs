using I2.Loc;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "Standoff/Create Game Mode", order = 1)]
	public class GameMode : ScriptableObject
	{
		[SerializeField]
		private GameController _controller;

		public string LocalizedName
		{
			get;
			private set;
		}

		public string LocalizedDescription
		{
			get;
			private set;
		}

		public string[] LocalizedSettings
		{
			get;
			private set;
		}

		public GameController Controller
		{
			[CompilerGenerated]
			get
			{
				return _controller;
			}
		}

		public byte MaxPlayers
		{
			[CompilerGenerated]
			get
			{
				return _controller.MaxPlayers;
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return base.name;
			}
		}

		private void OnEnable()
		{
			LocalizedName = ScriptLocalization.Get($"GameMode/{base.name}");
			LocalizedDescription = ScriptLocalization.Get($"GameMode/{base.name}Description");
			LocalizedSettings = new string[2];
			LocalizedSettings[0] = ((!_controller.FriendlyFireOn) ? "OFF" : "ON");
			LocalizedSettings[1] = ((!_controller.TeamCollisionOn) ? "OFF" : "ON");
		}
	}
}
