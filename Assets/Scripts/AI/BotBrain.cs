using UnityEngine;

namespace Axlebolt.Standoff.AI
{
	public enum BotState
	{
		Patrol,
		Chasing,
		EngageTarget,
		SearchLastKnown
	}

	public class BotBrain : MonoBehaviour
	{
		[SerializeField] private BotPerception _perception;
		[SerializeField] private BotMovement _movement;
		[SerializeField] private BotShooting _shooting;
		[SerializeField] private float _engageRange = 25f;
		[SerializeField] private float _lostSightRange = 30f;
		[SerializeField] private float _searchDuration = 5f;
		[SerializeField] private float _destinationRefresh = 0.4f;

		private BotState _state;
		private float _stateEnterTime;
		private float _nextDestinationTime;
		private float _aimTick;

		public BotState State
		{
			get
			{
				return _state;
			}
		}

		private void Awake()
		{
			if (_perception == null)
			{
				_perception = GetComponent<BotPerception>();
			}
			if (_movement == null)
			{
				_movement = GetComponent<BotMovement>();
			}
			if (_shooting == null)
			{
				_shooting = GetComponent<BotShooting>();
			}
		}

		private void OnEnable()
		{
			ResetBot();
		}

		public void ResetBot()
		{
			_state = BotState.Patrol;
			_stateEnterTime = Time.time;
			_nextDestinationTime = 0f;
			_shooting.Release();
			if (_movement != null)
			{
				_movement.Stop();
			}
		}

		public void Tick(float deltaTime)
		{
			if (_perception == null || _movement == null || _shooting == null)
			{
				return;
			}

			Transform target = _perception.Target;
			float targetDistance = _perception.TargetDistance;
			bool hasTarget = _perception.HasTarget;

			switch (_state)
			{
				case BotState.Patrol:
					if (_movement.IsStuck)
					{
						_movement.ResolveStuck();
					}
					if (!_movement.HasDestination || _movement.IsArrived)
					{
						_movement.PatrolNext();
					}
					if (hasTarget)
					{
						ChangeState(BotState.Chasing);
					}
					break;

				case BotState.Chasing:
					if (hasTarget)
					{
						UpdateDestination(target.position);
						if (targetDistance <= _engageRange)
						{
							ChangeState(BotState.EngageTarget);
						}
					}
					else if (_perception.HasLastKnownPosition)
					{
						ChangeState(BotState.SearchLastKnown);
					}
					else
					{
						ChangeState(BotState.Patrol);
					}
					break;

				case BotState.EngageTarget:
					if (hasTarget)
					{
						_aimTick += deltaTime;
						if (_aimTick >= 0.05f)
						{
							_aimTick = 0f;
							_shooting.Acquire(target);
							_shooting.Tick(deltaTime);
						}
						if (targetDistance > _lostSightRange)
						{
							ChangeState(BotState.Chasing);
						}
					}
					else
					{
						_shooting.Release();
						if (_perception.HasLastKnownPosition)
						{
							ChangeState(BotState.SearchLastKnown);
						}
						else
						{
							ChangeState(BotState.Patrol);
						}
					}
					break;

				case BotState.SearchLastKnown:
					if (hasTarget)
					{
						ChangeState(BotState.Chasing);
					}
					else if (Time.time - _stateEnterTime > _searchDuration || _movement.IsArrived)
					{
						ChangeState(BotState.Patrol);
					}
					else
					{
						UpdateDestination(_perception.LastKnownPosition);
					}
					break;
			}
		}

		private void UpdateDestination(Vector3 position)
		{
			if (Time.time >= _nextDestinationTime || !_movement.HasDestination)
			{
				_nextDestinationTime = Time.time + _destinationRefresh;
				_movement.MoveTo(position);
			}
		}

		private void ChangeState(BotState newState)
		{
			if (_state == newState)
			{
				return;
			}
			_state = newState;
			_stateEnterTime = Time.time;
			if (newState == BotState.SearchLastKnown && _perception != null && _perception.HasLastKnownPosition)
			{
				_movement.MoveTo(_perception.LastKnownPosition);
			}
			if (newState != BotState.EngageTarget && _shooting != null)
			{
				_shooting.Release();
			}
		}
	}
}