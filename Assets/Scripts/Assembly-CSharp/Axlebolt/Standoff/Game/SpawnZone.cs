using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpawnZone : BaseZone
	{
		private const int MaxHeight = 100;

		private const float Delta = 0.03f;

		[SerializeField]
		private SpawnZoneType _type;

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

		public SpawnZoneType Type
		{
			[CompilerGenerated]
			get
			{
				return _type;
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
			default:
				Gizmos.color = Color.green;
				break;
			}
			Gizmos.DrawWireCube(base.transform.position, base.transform.localScale);
		}

		public Vector3 RandomPosition()
		{
			Vector3 zonePos = base.transform.position;
			Vector3 vector = zonePos + new Vector3((Random.value - 0.5f) * base.transform.localScale.x, base.transform.localScale.y / 2f, (Random.value - 0.5f) * base.transform.localScale.z);
			RaycastHit hitInfo;
			if (Physics.Raycast(vector, Vector3.down, out hitInfo, 500f, 1))
			{
				Vector3 groundPoint = hitInfo.point;
				if (Mathf.Abs(groundPoint.y - zonePos.y) < 2f)
				{
					RaycastHit hitInfo2;
					if (Physics.Raycast(groundPoint + Vector3.down * 0.5f, Vector3.down, out hitInfo2, 500f, 1))
					{
						groundPoint = hitInfo2.point;
					}
				}
				vector = groundPoint + new Vector3(0f, 0.03f, 0f);
			}
			return vector;
		}

		public Quaternion RandomRotation()
		{
			return Quaternion.identity;
		}

		public SpawnPoint RandomPoint()
		{
			return new SpawnPoint(RandomPosition(), RandomRotation());
		}
	}
}
