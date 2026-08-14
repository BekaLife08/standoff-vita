using System;
using System.Collections;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	public abstract class UpdatebleCounterData : BaseCounterData
	{
		protected Coroutine updateCoroutine;

		protected WaitForSecondsUnscaled cachedWaitForSecondsUnscaled;

		[Tooltip("Update interval in seconds.")]
		[Range(0.1f, 10f)]
		[SerializeField]
		protected float updateInterval = 0.5f;

		public float UpdateInterval
		{
			get
			{
				return updateInterval;
			}
			set
			{
				if (!(Math.Abs(updateInterval - value) < 0.001f) && Application.isPlaying)
				{
					updateInterval = value;
					CacheWaitForSeconds();
				}
			}
		}

		protected override void PerformInitActions()
		{
			base.PerformInitActions();
			StartUpdateCoroutine();
		}

		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();
			CacheWaitForSeconds();
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			StoptUpdateCoroutine();
		}

		protected abstract IEnumerator UpdateCounter();

		private void StartUpdateCoroutine()
		{
			updateCoroutine = main.StartCoroutine(UpdateCounter());
		}

		private void StoptUpdateCoroutine()
		{
			main.StopCoroutine(updateCoroutine);
		}

		private void CacheWaitForSeconds()
		{
			cachedWaitForSecondsUnscaled = new WaitForSecondsUnscaled(updateInterval);
		}
	}
}
