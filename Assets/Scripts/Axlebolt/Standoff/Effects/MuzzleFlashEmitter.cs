using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class MuzzleFlashEmitter : SimpleEffectsEmitter<MuzzleFlashEmitter, MuzzleFlashId>
	{
		private MuzzleFlashParams _params;

		private readonly Dictionary<MuzzleFlashId, MuzzleFlashParam> _paramByType = new Dictionary<MuzzleFlashId, MuzzleFlashParam>();

		public void Init()
		{
			_params = ResourcesUtility.Load<MuzzleFlashParams>("Effects/MuzzleFlashParams");
			MuzzleFlashParam[] @params = _params.Params;
			foreach (MuzzleFlashParam muzzleFlashParam in @params)
			{
				_paramByType[new MuzzleFlashId(muzzleFlashParam.Type, false)] = muzzleFlashParam;
				_paramByType[new MuzzleFlashId(muzzleFlashParam.Type, true)] = muzzleFlashParam;
			}
			if (ValidateParams())
			{
				Init(_params.EffectDetails);
			}
		}

		private bool ValidateParams()
		{
			IEnumerable<MuzzleFlashType> enumerable = Enum.GetValues(typeof(MuzzleFlashType)).Cast<MuzzleFlashType>();
			bool result = true;
			foreach (MuzzleFlashType item in enumerable)
			{
				MuzzleFlashId key = new MuzzleFlashId(item, true);
				if (!_paramByType.ContainsKey(key))
				{
					SimpleEffectsEmitter<MuzzleFlashEmitter, MuzzleFlashId>.Log.Error(string.Format("MuzzleFlashParam not found for type {0}", item));
					result = false;
				}
				else if (_paramByType[key].Particles == null)
				{
					SimpleEffectsEmitter<MuzzleFlashEmitter, MuzzleFlashId>.Log.Error(string.Format("MuzzleFlashParam.Particles for type {0} is null", item));
					result = false;
				}
			}
			return result;
		}

		public void Emit(MuzzleFlashType type, Vector3 position, Vector3 direction, bool isLocal)
		{
			Emit(new MuzzleFlashId(type, isLocal), position, direction, isLocal);
		}

		protected override void OnParticlesCreated(MuzzleFlashId id, ParticleSystem particles)
		{
			particles.name = id.Type.ToString();
			if (id.IsLocal)
			{
				particles.name += "Local";
				particles.gameObject.layer = 8;
				ParticleSystem[] componentsInChildren = particles.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem particleSystem in componentsInChildren)
				{
					particleSystem.gameObject.layer = 8;
				}
			}
		}

		protected override MuzzleFlashId[] GetTypes()
		{
			return _paramByType.Keys.ToArray();
		}

		protected override ParticleSystem GetParticles(MuzzleFlashId id)
		{
			return _paramByType[id].Particles;
		}
	}
}
