using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class BombIndicator : MonoBehaviour
	{
		private LensFlare _lensFlare;

		private Transform _transform;

		private Transform _mainCamera;

		private float _brightness;

		private float _beepInterval;

		private float _progress;

		private bool _loop;

		public Flare Flare
		{
			get;
			set;
		}

		private Transform MainCamera
		{
			[CompilerGenerated]
			get
			{
				return _mainCamera ?? (_mainCamera = Camera.main.transform);
			}
		}

		private void Awake()
		{
			if (_lensFlare == null)
			{
				Init();
			}
		}

		public void Init()
		{
			_lensFlare = this.GetRequireComponent<LensFlare>();
			_transform = base.transform;
			_lensFlare.enabled = false;
			base.enabled = false;
		}

		public void Play(float beepInterval, float brightness, bool loop = false)
		{
			_beepInterval = beepInterval;
			_progress = _beepInterval;
			_loop = loop;
			_brightness = brightness;
			if (_lensFlare.flare != Flare)
			{
				_lensFlare.flare = Flare;
			}
			if (!base.enabled)
			{
				base.enabled = true;
				_lensFlare.enabled = true;
			}
			UpdateTick();
		}

		private void OnDisable()
		{
			Stop();
		}

		public void Stop()
		{
			base.enabled = false;
			_lensFlare.enabled = false;
		}

		private void Update()
		{
			UpdateTick();
		}

		private void UpdateTick()
		{
			if (_beepInterval <= 0f)
			{
				return;
			}
			float num = Vector3.Distance(_transform.position, MainCamera.position);
			float num2 = _progress / _beepInterval;
			_lensFlare.brightness = _brightness * num2 / num;
			_progress -= Time.deltaTime;
			if (_progress <= 0f)
			{
				if (_loop)
				{
					_progress = _beepInterval;
				}
				else
				{
					base.enabled = false;
				}
			}
		}
	}
}
