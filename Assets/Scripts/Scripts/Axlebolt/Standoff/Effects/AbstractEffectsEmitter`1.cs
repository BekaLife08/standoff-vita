using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Settings.Video;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public abstract class AbstractEffectsEmitter<T> : Singleton<T> where T : MonoBehaviour
	{
		private EffectDetails _effectDetails;

		internal void Init([NotNull] EffectDetails effectDetails)
		{
			if (effectDetails == null)
			{
				throw new ArgumentNullException("effectDetails");
			}
			_effectDetails = effectDetails;
			InitPool(GetPoolSize());
			VideoSettingsManager.Instance.EffectDetailChanged += EffectDetailChanged;
		}

		private void EffectDetailChanged()
		{
			ClearPool();
			InitPool(GetPoolSize());
		}

		private int GetPoolSize()
		{
			QualityLvl effectDetail = VideoSettingsManager.Instance.EffectDetail;
			if (effectDetail >= QualityLvl.High)
			{
				return _effectDetails.HighDetailCount;
			}
			if (effectDetail >= QualityLvl.Medium)
			{
				return _effectDetails.MediumDetailCount;
			}
			if (effectDetail >= QualityLvl.Low)
			{
				return _effectDetails.LowDetailCount;
			}
			return (effectDetail >= QualityLvl.VeryLow) ? _effectDetails.VeryLowDetailCount : 0;
		}

		protected abstract void InitPool(int poolSize);

		protected abstract void ClearPool();

		protected void OnDestroy()
		{
			base.OnDestroy();
			VideoSettingsManager.Instance.EffectDetailChanged -= EffectDetailChanged;
		}
	}
}
