using UnityEngine;

namespace Axlebolt.Standoff.AI
{
	public interface IDamageable
	{
		void ApplyDamage(float amount);
	}

	public class BotShooting : MonoBehaviour
	{
		[SerializeField] private Transform _weaponPivot;
		[SerializeField] private float _reactionTime = 0.3f;
		[SerializeField] private float _aimSpeed = 6f;
		[SerializeField] private float _baseSpread = 1.5f;
		[SerializeField] private float _spreadPerMeter = 0.08f;
		[SerializeField] private float _maxSpread = 12f;
		[SerializeField] private int _shotsPerBurst = 3;
		[SerializeField] private float _timeBetweenShots = 0.1f;
		[SerializeField] private float _timeBetweenBursts = 0.8f;
		[SerializeField] private float _fireRange = 60f;
		[SerializeField] private LayerMask _hitMask = -1;

		private Transform _transform;
		private Transform _target;
		private bool _engaged;
		private float _nextShotTime;
		private int _shotsFiredInBurst;
		private Vector3 _aimError;

		public bool IsEngaged
		{
			get
			{
				return _engaged;
			}
		}

		private void Awake()
		{
			_transform = transform;
			if (_weaponPivot == null)
			{
				_weaponPivot = _transform;
			}
		}

		public void Acquire(Transform target)
		{
			_target = target;
			_engaged = target != null;
			if (_engaged)
			{
				_nextShotTime = Time.time + _reactionTime;
				_shotsFiredInBurst = 0;
			}
		}

		public void Release()
		{
			_engaged = false;
			_target = null;
		}

		public void Tick(float deltaTime)
		{
			if (!_engaged || _target == null)
			{
				return;
			}

			Vector3 weaponPosition = _weaponPivot.position;
			Vector3 targetPosition = _target.position + Vector3.up * 1.2f;
			Vector3 toTarget = targetPosition - weaponPosition;
			float distance = toTarget.magnitude;
			if (distance < 0.01f)
			{
				return;
			}
			toTarget /= distance;

			float spread = Mathf.Min(_baseSpread + distance * _spreadPerMeter, _maxSpread);
			_aimError = Random.insideUnitSphere * (spread * 0.01f);
			Vector3 aimDirection = toTarget + _aimError;
			aimDirection.Normalize();

			Quaternion desiredRotation = Quaternion.LookRotation(aimDirection);
			_weaponPivot.rotation = Quaternion.Slerp(_weaponPivot.rotation, desiredRotation, _aimSpeed * deltaTime);

			if (Time.time < _nextShotTime)
			{
				return;
			}
			if (_shotsFiredInBurst >= _shotsPerBurst)
			{
				_nextShotTime = Time.time + _timeBetweenBursts;
				_shotsFiredInBurst = 0;
				return;
			}

			Fire(distance);
			_shotsFiredInBurst++;
			_nextShotTime = Time.time + _timeBetweenShots;
		}

		private void Fire(float distance)
		{
			Vector3 origin = _weaponPivot.position;
			Vector3 direction = _weaponPivot.forward;
			float range = Mathf.Min(_fireRange, distance + 3f);
			RaycastHit hit;
			if (Physics.Raycast(origin, direction, out hit, range, _hitMask))
			{
				IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
				if (damageable != null)
				{
					damageable.ApplyDamage(10f);
				}
			}
		}
	}
}