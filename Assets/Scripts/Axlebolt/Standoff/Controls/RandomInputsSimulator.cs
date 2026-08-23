using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Controls
{
	public class RandomInputsSimulator : MonoBehaviour
	{
		[SerializeField]
		private float _aimAngleChangeDuration;

		[SerializeField]
		private float _aimAngleLerpSpeed;

		[SerializeField]
		private float _moveDirectionChangeDuration;

		[SerializeField]
		private float _moveDirectionLerpSpeed;

		private float _aimAngleChangedTime;

		private float _moveDiractionChangedTime;

		private Vector3 _targetAimAngles;

		private Vector2 _targetMoveDirection;

		private Vector3 _currentAimAngles;

		private Vector3 _prevAimAngles;

		private Vector3 _currentMoveDirection;

		private void Start()
		{
			_aimAngleChangedTime = Time.time;
			_moveDiractionChangedTime = Time.time;
		}

		private void SimulateAimAngles()
		{
			_prevAimAngles = _currentAimAngles;
			_currentAimAngles = Vector3.Lerp(_currentAimAngles, _targetAimAngles, Time.deltaTime * 10f);
		}

		private void SimulateMoveDirection()
		{
			_currentMoveDirection = Vector2.Lerp(_currentMoveDirection, _targetMoveDirection, Time.deltaTime * 5f);
		}

		private Vector2 GetAimAxis()
		{
			return new Vector2(_currentAimAngles.x - _prevAimAngles.x, _currentAimAngles.y - _prevAimAngles.y);
		}

		private Vector2 GetMoveDirection()
		{
			return _currentMoveDirection.normalized;
		}

		public PlayerInputs GetPlayerInputs()
		{
			PlayerInputs playerInputs = new PlayerInputs();
			Vector2 aimAxis = GetAimAxis();
			playerInputs.DeltaAimAngles = new Vector2(aimAxis.x, aimAxis.y);
			Vector2 moveDirection = GetMoveDirection();
			playerInputs.Horizontal = moveDirection.x;
			playerInputs.Vertical = moveDirection.y;
			return playerInputs;
		}

		private void Update()
		{
			SimulateAimAngles();
			if (Time.time - _aimAngleChangedTime > _aimAngleChangeDuration)
			{
				_aimAngleChangedTime = Time.time;
				_targetAimAngles = new Vector3(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(0, 360), 0f);
			}
			SimulateMoveDirection();
			if (Time.time - _moveDiractionChangedTime > _moveDirectionChangeDuration)
			{
				_moveDiractionChangedTime = Time.time;
				_targetMoveDirection = PickNonBlockedDirection();
			}
			// If stuck against wall, re-pick immediately
			if (_targetMoveDirection.sqrMagnitude > 0.01f && IsBlocked(_targetMoveDirection))
			{
				_targetMoveDirection = PickNonBlockedDirection();
				_moveDiractionChangedTime = Time.time;
			}
		}

		private Vector2 PickNonBlockedDirection()
		{
			for (int i = 0; i < 8; i++)
			{
				Vector2 cand = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
				if (cand.sqrMagnitude < 0.1f) continue;
				cand.Normalize();
				if (!IsBlocked(cand)) return cand;
			}
			return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
		}

		private bool IsBlocked(Vector2 dir)
		{
			Vector3 worldDir = transform.TransformDirection(new Vector3(dir.x, 0f, dir.y));
			Vector3 origin = transform.position + Vector3.up * 0.6f;
			// Capsule-like check: raycast forward + small side offsets
			if (Physics.Raycast(origin, worldDir, 1.1f)) return true;
			if (Physics.Raycast(origin + transform.right * 0.3f, worldDir, 1.1f)) return true;
			if (Physics.Raycast(origin - transform.right * 0.3f, worldDir, 1.1f)) return true;
			return false;
		}
	}
}
