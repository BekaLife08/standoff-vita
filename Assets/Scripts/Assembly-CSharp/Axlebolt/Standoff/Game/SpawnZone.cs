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
			Vector3 vector = base.transform.position + new Vector3((Random.value - 0.5f) * base.transform.localScale.x, base.transform.localScale.y / 2f, (Random.value - 0.5f) * base.transform.localScale.z);
			Vector3 highOrigin = vector + Vector3.up * 300f;
			RaycastHit hitInfo;
			if (Physics.Raycast(highOrigin, Vector3.down, out hitInfo, 600f, 1))
			{
				Vector3 groundPoint = hitInfo.point;
				RaycastHit hitInfo2;
				if (Physics.Raycast(groundPoint + Vector3.up * 5f, Vector3.down, out hitInfo2, 10f, 1))
				{
					groundPoint = hitInfo2.point;
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
