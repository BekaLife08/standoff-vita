using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class PlantedBombController : MonoBehaviour
	{
		private BombParameters _bombParameters;

		private BombExtendedParameters _extendedParameters;

		private ParticleSystem _particleSystem;

		private Transform _mainCamera;

		private BombIndicator _bombIndicator;

		private AudioSource _audioSource;

		private float _beepInterval;

		private int _beepStepIndex;

		private Action _beepHandler;

		public bool IsPlanted
		{
			get;
			private set;
		}

		public bool IsDetonated
		{
			get;
			private set;
		}

		public bool IsDefused
		{
			get;
			private set;
		}

		public double PlantTime
		{
			get;
			private set;
		}

		public Transform Transform
		{
			get;
			private set;
		}

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
		}

		public void Init(BombParameters bombParameters, BombExtendedParameters extendedParameters)
		{
			GameObject original = WeaponUtility.LoadModel(WeaponId.Bomb);
			MeshLodGroup requireComponent = UnityEngine.Object.Instantiate(original, base.transform, worldPositionStays: false).GetRequireComponent<MeshLodGroup>();
			requireComponent.Initialize();
			Singleton<WeaponManager>.Instance.WeaponMaterialManager.SetMaterial(requireComponent, WeaponId.Bomb, InventoryItemId.None);
			_bombIndicator = requireComponent.gameObject.GetRequireComponentInChildren<BombIndicator>();
			_bombIndicator.Init();
			_bombParameters = bombParameters;
			_extendedParameters = extendedParameters;
			_particleSystem = UnityEngine.Object.Instantiate(_extendedParameters.ExplosionParticle, base.transform, worldPositionStays: false);
			_particleSystem.transform.localPosition = _extendedParameters.ExplosionParticleOffset;
			_particleSystem.gameObject.SetActive(value: false);
			_audioSource = base.gameObject.AddComponent<AudioSource>();
			_audioSource.playOnAwake = false;
			_audioSource.spatialBlend = 1f;
		}

		public void Plant(Vector3 position, float yRotation, double plantTime, Action beepHandler)
		{
			if (IsPlanted)
			{
				throw new Exception("Bomb alredy has been planted!");
			}
			Vector3 bombRotation = _extendedParameters.BombRotation;
			base.transform.position = position + _extendedParameters.BombOffset;
			base.transform.eulerAngles = new Vector3(bombRotation.x, yRotation, bombRotation.z);
			Transform = base.transform;
			PlantTime = plantTime;
			IsPlanted = true;
			_beepStepIndex = 0;
			_beepHandler = beepHandler;
			base.gameObject.SetActive(value: true);
			_bombIndicator.Flare = _extendedParameters.PlantedIndicatorFlare;
		}

		public void Clear()
		{
			_particleSystem.Stop();
			_particleSystem.gameObject.SetActive(value: false);
			base.gameObject.SetActive(value: false);
			IsPlanted = false;
			IsDetonated = false;
			IsDefused = false;
			_beepInterval = 0f;
			_audioSource.Stop();
			_beepHandler = null;
			_bombIndicator.Stop();
		}

		public void Detonate()
		{
			_particleSystem.gameObject.SetActive(value: true);
			_particleSystem.Play(withChildren: true);
			IsDetonated = true;
			_audioSource.clip = _extendedParameters.ExplosionClip;
			_audioSource.Play();
			_bombIndicator.Stop();
		}

		public void Defused()
		{
			IsDefused = true;
			_bombIndicator.Stop();
		}

		private void Update()
		{
			if (IsDetonated || IsDefused)
			{
				return;
			}
			float num = (float)((double)_bombParameters.DetonationDuration - (PhotonNetwork.time - PlantTime));
			if (num <= _extendedParameters.FinalClipTime)
			{
				if (_audioSource.clip != _extendedParameters.FinalClip)
				{
					_audioSource.clip = _extendedParameters.FinalClip;
					_audioSource.Play();
					_bombIndicator.Flare = _extendedParameters.DetonatedIndicatorFlare;
					_bombIndicator.Play(float.MaxValue, _extendedParameters.PlantedIndicatorBrightness);
				}
			}
			else if (_beepInterval <= 0f)
			{
				_beepInterval = GetBeepInterval(num);
				Beep(_beepInterval);
			}
			else
			{
				_beepInterval -= Time.deltaTime;
			}
		}

		private float GetBeepInterval(float timeLeft)
		{
			BombBeepStep[] beepSteps = _extendedParameters.BeepSteps;
			if (_beepStepIndex >= beepSteps.Length)
			{
				return 0f;
			}
			while (_beepStepIndex + 1 < beepSteps.Length && beepSteps[_beepStepIndex + 1].Time > timeLeft)
			{
				_beepStepIndex++;
			}
			return beepSteps[_beepStepIndex].Interval;
		}

		private void Beep(float beepInterval)
		{
			_audioSource.PlayOneShot(_extendedParameters.BeepClip);
			_bombIndicator.Play(beepInterval, _extendedParameters.PlantedIndicatorBrightness);
			if (_beepHandler != null)
			{
				_beepHandler();
			}
		}
	}
}
