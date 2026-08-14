using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Game
{
	public class WinPlayers
	{
		private readonly PhotonPlayer[] _places;

		public PhotonPlayer FirstPlace
		{
			[CompilerGenerated]
			get
			{
				return _places[0];
			}
		}

		public bool HasSecondPlace
		{
			[CompilerGenerated]
			get
			{
				return SecondPlace != null;
			}
		}

		public PhotonPlayer SecondPlace
		{
			[CompilerGenerated]
			get
			{
				return (_places.Length <= 1) ? null : _places[1];
			}
		}

		public bool HasThirdPlace
		{
			[CompilerGenerated]
			get
			{
				return ThirdPlace != null;
			}
		}

		public PhotonPlayer ThirdPlace
		{
			[CompilerGenerated]
			get
			{
				return (_places.Length <= 2) ? null : _places[2];
			}
		}

		public WinPlayers([NotNull] PhotonPlayer firstPlace, PhotonPlayer secondPlace, PhotonPlayer thirdPlace)
		{
			if (firstPlace == null)
			{
				throw new ArgumentNullException("firstPlace");
			}
			List<PhotonPlayer> list = new List<PhotonPlayer>
			{
				firstPlace
			};
			if (secondPlace != null)
			{
				list.Add(secondPlace);
				if (thirdPlace != null)
				{
					list.Add(thirdPlace);
				}
			}
			_places = list.ToArray();
		}

		public WinPlayers([NotNull] PhotonPlayer[] players)
		{
			if (players == null)
			{
				throw new ArgumentNullException("players");
			}
			if (players.Length == 0 || players[0] == null)
			{
				throw new ArgumentException("First place not found");
			}
			_places = players;
		}

		public PhotonPlayer[] GetAsArray()
		{
			return _places;
		}
	}
}
