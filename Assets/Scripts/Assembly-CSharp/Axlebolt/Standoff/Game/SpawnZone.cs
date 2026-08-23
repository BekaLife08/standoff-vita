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
			Ray ray = default(Ray);
			ray.origin = vector;
			ray.direction = Vector3.down;
			Ray ray2 = ray;
			RaycastHit hitInfo;
			if (Physics.Raycast(ray2, out hitInfo, 100f, 1))
			{
				vector = hitInfo.point + new Vector3(0f, 0.03f, 0f);
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
