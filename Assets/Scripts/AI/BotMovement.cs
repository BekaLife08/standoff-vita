using UnityEngine;
using UnityEngine.AI;

namespace Axlebolt.Standoff.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class BotMovement : MonoBehaviour
	{
		[SerializeField] private Transform[] _waypoints;
		[SerializeField] private float _patrolRadius = 10f;
		[SerializeField] private float _stopDistance = 0.8f;
		[SerializeField] private float _stuckDistanceThreshold = 0.5f;
		[SerializeField] private float _stuckCheckInterval = 0.5f;

		private NavMeshAgent _agent;
		private Transform _transform;
		private int _waypointIndex;
		private Vector3 _destination;
		private bool _hasDestination;
		private float _lastProgressTime;
		private Vector3 _lastPosition;
		private float _stuckTime;

		public bool HasDestination
		{
			get
			{
				return _hasDestination;
			}
		}

		public bool IsArrived
		{
			get
			{
				return _hasDestination && !_agent.pathPending && _agent.remainingDistance <= _stopDistance;
			}
		}

		public bool IsStuck
		{
			get
			{
				return _hasDestination && _stuckTime >= _stuckCheckInterval;
			}
		}

		private void Awake()
		{
			_transform = transform;
			_agent = GetComponent<NavMeshAgent>();
			_agent.stoppingDistance = _stopDistance;
		}

		public void MoveTo(Vector3 destination)
		{
			_destination = destination;
			_hasDestination = true;
			_agent.isStopped = false;
			_agent.SetDestination(destination);
			_lastPosition = _transform.position;
			_lastProgressTime = Time.time;
			_stuckTime = 0f;
		}

		public void Stop()
		{
			_hasDestination = false;
			_agent.isStopped = true;
			_stuckTime = 0f;
		}

		public void Tick(float deltaTime)
		{
			if (!_hasDestination || _agent.pathPending)
			{
				return;
			}
			float time = Time.time;
			if (time - _lastProgressTime < _stuckCheckInterval)
			{
				return;
			}
			_lastProgressTime = time;
			float distance = Vector3.Distance(_transform.position, _lastPosition);
			_lastPosition = _transform.position;
			if (distance < _stuckDistanceThreshold && !IsArrived)
			{
				_stuckTime += _stuckCheckInterval;
			}
			else
			{
				_stuckTime = 0f;
			}
		}

		public void ResolveStuck()
		{
			if (!_hasDestination)
			{
				return;
			}
			_agent.isStopped = true;
			_agent.ResetPath();
			_agent.isStopped = false;
			_agent.SetDestination(_destination);
			_stuckTime = 0f;
		}

		public void PatrolNext()
		{
			Vector3 point;
			if (_waypoints != null && _waypoints.Length > 0)
			{
				point = _waypoints[_waypointIndex].position;
				_waypointIndex = (_waypointIndex + 1) % _waypoints.Length;
			}
			else
			{
				point = RandomNavPoint(_transform.position, _patrolRadius);
			}
			MoveTo(point);
		}

		private static Vector3 RandomNavPoint(Vector3 center, float radius)
		{
			for (int i = 0; i < 8; i++)
			{
				Vector3 random = center + Random.insideUnitSphere * radius;
				random.y = center.y;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(random, out hit, radius, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}
			return center;
		}
	}
}