using UnityEngine;

namespace Axlebolt.Standoff.AI
{
	public class BotPerception : MonoBehaviour
	{
		[SerializeField] private float _sightRange = 40f;
		[SerializeField] private float _fov = 90f;
		[SerializeField] private LayerMask _detectionMask = -1;
		[SerializeField] private LayerMask _enemyMask;
		[SerializeField] private LayerMask _obstacleMask;
		[SerializeField] private string _enemyTag = "Player";
		[SerializeField] private float _checkInterval = 0.15f;
		[SerializeField] private Vector3 _eyeOffset = new Vector3(0f, 1.5f, 0f);

		private Transform _transform;
		private float _nextCheckTime;
		private Transform _target;
		private float _targetDistance;
		private Vector3 _lastKnownPosition;
		private bool _hasLastKnownPosition;

		private static Collider[] _overlapBuffer = new Collider[32];
		private static RaycastHit[] _raycastBuffer = new RaycastHit[4];

		public Transform Target
		{
			get
			{
				return _target;
			}
		}

		public bool HasTarget
		{
			get
			{
				return _target != null;
			}
		}

		public float TargetDistance
		{
			get
			{
				return _targetDistance;
			}
		}

		public Vector3 LastKnownPosition
		{
			get
			{
				return _lastKnownPosition;
			}
		}

		public bool HasLastKnownPosition
		{
			get
			{
				return _hasLastKnownPosition;
			}
		}

		public float SightRange
		{
			get
			{
				return _sightRange;
			}
		}

		private void Awake()
		{
			_transform = transform;
		}

		private void Update()
		{
			if (Time.time < _nextCheckTime)
			{
				return;
			}
			_nextCheckTime = Time.time + _checkInterval;
			Check();
		}

		public void ForceCheck()
		{
			_nextCheckTime = Time.time;
			Check();
		}

		public void ForgetTarget()
		{
			_target = null;
			_targetDistance = 0f;
		}

		private void Check()
		{
			Vector3 position = _transform.position;
			int count = Physics.OverlapSphereNonAlloc(position, _sightRange, _overlapBuffer, _detectionMask);
			Transform best = null;
			float bestDistance = float.MaxValue;
			for (int i = 0; i < count; i++)
			{
				Collider collider = _overlapBuffer[i];
				if (collider == null)
				{
					continue;
				}
				Transform candidate = collider.transform;
				if (candidate == _transform)
				{
					continue;
				}
				if (!IsEnemy(collider.gameObject))
				{
					continue;
				}
				Vector3 direction = candidate.position - position;
				float distance = direction.magnitude;
				if (distance > _sightRange || distance < 0.01f)
				{
					continue;
				}
				float angle = Vector3.Angle(_transform.forward, direction);
				if (angle > _fov * 0.5f)
				{
					continue;
				}
				if (distance >= bestDistance)
				{
					continue;
				}
				if (!HasLineOfSight(candidate, position, distance))
				{
					continue;
				}
				bestDistance = distance;
				best = candidate;
			}

			if (best != null)
			{
				_target = best;
				_targetDistance = bestDistance;
				_lastKnownPosition = best.position;
				_hasLastKnownPosition = true;
			}
			else
			{
				_target = null;
				_targetDistance = 0f;
			}
		}

		private bool IsEnemy(GameObject gameObject)
		{
			if (_enemyTag.Length > 0 && gameObject.CompareTag(_enemyTag))
			{
				return true;
			}
			return _enemyMask.value != 0 && ((1 << gameObject.layer) & _enemyMask.value) != 0;
		}

		private bool HasLineOfSight(Transform candidate, Vector3 originPosition, float distance)
		{
			Vector3 origin = originPosition + _eyeOffset;
			Vector3 end = candidate.position + Vector3.up * 1.2f;
			Vector3 direction = end - origin;
			int hitCount = Physics.RaycastNonAlloc(origin, direction, _raycastBuffer, direction.magnitude, _obstacleMask);
			for (int i = 0; i < hitCount; i++)
			{
				if (_raycastBuffer[i].collider != null)
				{
					return false;
				}
			}
			return true;
		}
	}
}