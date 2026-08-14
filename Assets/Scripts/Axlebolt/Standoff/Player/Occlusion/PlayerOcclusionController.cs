using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Occlusion.Dynamic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Occlusion
{
	public class PlayerOcclusionController : ObjectOccludee
	{
		private CharacterController _characterController;

		[SerializeField]
		private List<Vector2> _hitPoints;

		public event Action OnOcclusionBecameVisible = delegate
		{
		};

		public event Action OnOcclusionPostBecameVisible = delegate
		{
		};

		public event Action OnOcclusionBecameInvisible = delegate
		{
		};

		private void Awake()
		{
			SetEnabled(isEnabled: true);
		}

		public void PreInitialize()
		{
			_characterController = GetComponent<CharacterController>();
			Singleton<OcclusionControl>.Instance.Register(this);
		}

		public override void OnOcclusionVisible()
		{
			base.OnOcclusionVisible();
			this.OnOcclusionBecameVisible();
			this.OnOcclusionPostBecameVisible();
		}

		public override void OnOcclusionInvisible()
		{
			base.OnOcclusionInvisible();
			this.OnOcclusionBecameInvisible();
		}

		public override Bounds GetCharacterBounds()
		{
			return _characterController.bounds;
		}

		public override List<Vector3> GetRaycastHitPoints(Vector3 casterPoint)
		{
			Vector3 vector = casterPoint - base.transform.position;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			vector2.Normalize();
			Vector3 a = Quaternion.AngleAxis(-90f, Vector3.up) * vector2;
			Vector3 up = Vector3.up;
			List<Vector3> list = new List<Vector3>();
			Vector3 position = base.transform.position;
			foreach (Vector2 hitPoint in _hitPoints)
			{
				Vector2 current = hitPoint;
				list.Add(position + a * current.x + up * current.y);
			}
			float num = 0.4f;
			num *= (float)((!(vector.y > 0f)) ? 1 : (-1));
			list[0] = list[0] + vector2 * num;
			list[1] = list[1] + vector2 * (num * 0.5f);
			list[2] = list[2] + vector2 * (num * 0.5f);
			list[3] = list[3] + vector2 * ((0f - num) * 0.5f);
			list[4] = list[4] + vector2 * ((0f - num) * 0.5f);
			list[5] = list[5] + vector2 * (0f - num);
			return list;
		}
	}
}
