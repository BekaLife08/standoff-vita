using Axlebolt.Standoff.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Matchmaking
{
	public class RegionSelectRow : View
	{
		[SerializeField]
		private Text _regionNameText;

		[SerializeField]
		private Image _pingIndicatorImage;

		public Text RegionNameText
		{
			[CompilerGenerated]
			get
			{
				return _regionNameText;
			}
		}

		public Image PingIndicatorImage
		{
			[CompilerGenerated]
			get
			{
				return _pingIndicatorImage;
			}
		}

		public Action ActionHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			this.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				ActionHandler?.Invoke();
			});
		}
	}
}
