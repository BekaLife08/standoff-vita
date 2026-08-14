using Axlebolt.Standoff.UI;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Matchmaking
{
	public class SearchGameView : View
	{
		[SerializeField]
		private Text _onlinePlayerText;

		[SerializeField]
		private Text _regionText;

		[SerializeField]
		private Text _pingText;

		[SerializeField]
		private Text _timeText;

		[SerializeField]
		private Button _cancelButton;

		public int Online
		{
			set
			{
				_onlinePlayerText.text = value.ToString();
			}
		}

		public string Region
		{
			set
			{
				_regionText.text = value;
			}
		}

		public int Ping
		{
			set
			{
				_pingText.text = value.ToString();
			}
		}

		public Action CancelHandler
		{
			get;
			set;
		}

		protected Button CancelButton
		{
			[CompilerGenerated]
			get
			{
				return _cancelButton;
			}
		}

		protected virtual void Awake()
		{
			_onlinePlayerText.text = string.Empty;
			_regionText.text = string.Empty;
			_pingText.text = string.Empty;
			_cancelButton.onClick.AddListener(delegate
			{
				CancelHandler?.Invoke();
			});
		}

		public override void Show()
		{
			base.Show();
			StartCoroutine(StartTimer());
		}

		public override void Hide()
		{
			base.Hide();
			StopAllCoroutines();
			_onlinePlayerText.text = string.Empty;
			_regionText.text = string.Empty;
			_pingText.text = string.Empty;
			_timeText.text = string.Empty;
		}

		private IEnumerator StartTimer()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (base.IsVisible)
			{
				SetTime(stopwatch.Elapsed.Seconds);
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void SetTime(float seconds)
		{
			_timeText.text = StringUtils.FormatTwoDigit(seconds);
		}
	}
}
