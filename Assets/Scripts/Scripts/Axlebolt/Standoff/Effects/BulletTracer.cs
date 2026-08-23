using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class BulletTracer : MonoBehaviour
	{
		public const float StartOffset = 2f;

		private MeshRenderer _meshRenderer;

		private Vector3 _startPosition;

		private Vector3 _direction;

		private float _distance;

		private float _speed;

		private bool _enabled;

		private BulletTraceEffectParams _params;

		public void Init(BulletTraceEffectParams effectParams)
		{
			_meshRenderer = GetComponent<MeshRenderer>();
			_params = effectParams;
		}

		internal void Trace(Vector3 startPosition, Vector3 endPosition, BulletTraceType traceType)
		{
			_direction = (endPosition - startPosition).normalized;
			_startPosition = startPosition;
			BulletTraceEffectParams.TracerParams tracerParams = _params.GetTracerParams(traceType);
			_distance = (endPosition - startPosition).magnitude;
			base.transform.rotation = Quaternion.LookRotation(_direction);
			base.transform.position = startPosition + _direction * (2f * tracerParams.Offset);
			if (Vector3.Distance(_startPosition, base.transform.position) > _distance)
			{
				Hide();
				return;
			}
			_speed = tracerParams.Speed;
			base.transform.localScale = new Vector3(tracerParams.ScaleXY, tracerParams.ScaleXY, tracerParams.ScaleZ);
			_meshRenderer.enabled = true;
			_enabled = true;
		}

		public void Hide()
		{
			_meshRenderer.enabled = false;
			_enabled = false;
			Singleton<BulletTraceEmitter>.Instance.ReturnToPool(this);
		}

		internal void Evaluate()
		{
			if (_enabled)
			{
				if (Vector3.Distance(_startPosition, base.transform.position) > _distance)
				{
					Hide();
				}
				else
				{
					base.transform.position += _direction * _speed * Time.deltaTime;
				}
			}
		}
	}
}
