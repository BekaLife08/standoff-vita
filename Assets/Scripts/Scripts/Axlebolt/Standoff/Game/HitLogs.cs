using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game
{
	public class HitLogs
	{
		public int PlayerId
		{
			get;
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonPlayer.Find(PlayerId);
			}
		}

		public List<int> Damages
		{
			get;
		}

		public HitLogs([NotNull] PhotonPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			PlayerId = player.ID;
			Damages = new List<int>();
		}
	}
}
