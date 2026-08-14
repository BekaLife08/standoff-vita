using System;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class WeaponBuyZone : BaseZone
	{
		[SerializeField]
		private Team _team;

		public Team Team
		{
			[CompilerGenerated]
			get
			{
				return _team;
			}
		}

		private void OnDrawGizmos()
		{
			switch (_team)
			{
			case Team.Tr:
				Gizmos.color = Color.yellow;
				break;
			case Team.Ct:
				Gizmos.color = Color.blue;
				break;
			case Team.None:
				Gizmos.color = Color.green;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			Gizmos.DrawWireCube(base.transform.position, base.transform.localScale);
		}
	}
}
