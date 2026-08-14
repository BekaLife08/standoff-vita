using Axlebolt.Standoff.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game
{
	public class ReconnectDialog : BaseDialog
	{
		[SerializeField]
		private Text _timerText;

		private float _duration;

		public void Show(float duration)
		{
			_duration = duration;
			Show();
			StartCoroutine(Timer());
		}

		private IEnumerator Timer()
		{
			float time = Time.time;
			while (base.IsVisible)
			{
				float leftTime = _duration - Time.time + time;
				if (leftTime < 0f)
				{
					leftTime = 0f;
				}
				_timerText.text = StringUtils.FormatTwoDigit(leftTime);
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
