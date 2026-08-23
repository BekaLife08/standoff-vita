using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.HitHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class SurfaceImpactsEmitter : SimpleEffectsEmitter<SurfaceImpactsEmitter, int>
	{
		private class Pair
		{
			public readonly int Min;

			public readonly int Max;

			public Pair(int min, int max)
			{
				Min = min;
				Max = max;
			}
		}

		private SurfaceImpactParams _params;

		private readonly Dictionary<SurfaceType, int> _indexByType = new Dictionary<SurfaceType, int>();

		private KnifeImpactParam _knifeParams;

		private readonly Dictionary<int, Pair> _decalIndexes = new Dictionary<int, Pair>();

		private Pair _knifeDecalIndex;

		private DecalsDrawer _decalsDrawer;

		public void Init()
		{
			_params = ResourcesUtility.Load<SurfaceImpactParams>("Effects/SurfaceImpactParams");
			if (_params.GunParams == null || _params.GunParams.Length == 0)
			{
				SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error("SurfaceImpactParams.Params are empty");
				return;
			}
			int num = 0;
			GunImpactParam[] gunParams = _params.GunParams;
			foreach (GunImpactParam gunImpactParam in gunParams)
			{
				if (gunImpactParam.SurfaceTypes.Length == 0)
				{
					SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error($"SurfaceTypes not set for ${gunImpactParam}");
					return;
				}
				SurfaceType[] surfaceTypes = gunImpactParam.SurfaceTypes;
				foreach (SurfaceType surfaceType in surfaceTypes)
				{
					if (_indexByType.ContainsKey(surfaceType))
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error($"{surfaceType} already defined");
						return;
					}
					_indexByType[surfaceType] = num;
				}
				num++;
			}
			_knifeParams = _params.KnifeParams;
			if (ValidateParams())
			{
				Init(_params.EffectDetails);
			}
		}

		private bool ValidateParams()
		{
			IEnumerable<SurfaceType> enumerable = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>();
			bool result = true;
			foreach (SurfaceType item in enumerable)
			{
				if (item != SurfaceType.Character && item != 0)
				{
					if (!_indexByType.ContainsKey(item))
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error($"SurfaceImpact not found for SurfaceType {item}");
						result = false;
					}
					else if (_params.GunParams[_indexByType[item]].Particles == null)
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error($"SurfaceImpact.Particles for SurfaceType {item} is null");
						result = false;
					}
				}
			}
			if (_knifeParams == null)
			{
				SimpleEffectsEmitter<SurfaceImpactsEmitter, int>.Log.Error("Knife params not set");
				result = false;
			}
			return result;
		}

		protected override int[] GetTypes()
		{
			int[] array = new int[_params.GunParams.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}
			return array;
		}

		protected override ParticleSystem GetParticles(int type)
		{
			return _params.GunParams[type].Particles;
		}

		protected override void InitPool(int poolSize)
		{
			base.InitPool(poolSize);
			InitDecals();
		}

		private void InitDecals()
		{
			List<Sprite> list = new List<Sprite>();
			int num = 0;
			for (int i = 0; i < _params.GunParams.Length; i++)
			{
				GunImpactParam gunImpactParam = _params.GunParams[i];
				_decalIndexes[i] = new Pair(num, num + gunImpactParam.Decals.Length);
				list.AddRange(gunImpactParam.Decals);
				num += gunImpactParam.Decals.Length;
			}
			_knifeDecalIndex = new Pair(num, num + _knifeParams.Decals.Length);
			list.AddRange(_knifeParams.Decals);
			_decalsDrawer = SimpleDecalsDrawer.Create(_params.BulletHoleMaterial, list.ToArray(), _params.MaxDecalsCount);
		}

		protected override void ClearPool()
		{
			base.ClearPool();
			_decalIndexes.Clear();
			if (_decalsDrawer != null)
			{
				UnityEngine.Object.Destroy(_decalsDrawer.gameObject);
			}
		}

		public void Emit(WeaponId weaponId, RaycastHit raycastHit, SurfaceType surfaceType, bool isLocal)
		{
			if (surfaceType != SurfaceType.Character && surfaceType != 0 && PoolSize != 0)
			{
				int num = _indexByType[surfaceType];
				Emit(num, raycastHit.point, raycastHit.normal, isLocal);
				DrawDecal(weaponId, num, raycastHit);
			}
		}

		private void DrawDecal(WeaponId weaponId, int keyIndex, RaycastHit raycastHit)
		{
			if (weaponId != WeaponId.Knife)
			{
				GunImpactParam gunImpactParam = _params.GunParams[keyIndex];
				Pair pair = _decalIndexes[keyIndex];
				int decalIndex = UnityEngine.Random.Range(pair.Min, pair.Max);
				_decalsDrawer.DrawDecal(raycastHit, gunImpactParam.Size, decalIndex);
			}
			else
			{
				KnifeImpactParam knifeParams = _knifeParams;
				Pair knifeDecalIndex = _knifeDecalIndex;
				int decalIndex2 = UnityEngine.Random.Range(knifeDecalIndex.Min, knifeDecalIndex.Max);
				_decalsDrawer.DrawDecal(raycastHit, knifeParams.Size, decalIndex2);
			}
		}

		public void ClearDecals()
		{
			_decalsDrawer.Clear();
		}
	}
}
