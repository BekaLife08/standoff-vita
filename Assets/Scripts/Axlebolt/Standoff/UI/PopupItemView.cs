using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(Button))]
	public class PopupItemView : View
	{
		[SerializeField]
		[NotNull]
		private Text _text;

		[NotNull]
		[SerializeField]
		private GameObject _splitLine;

		public Text Text
		{
			[CompilerGenerated]
			get
			{
				return _text;
			}
		}

		public GameObject SplitLine
		{
			[CompilerGenerated]
			get
			{
				return _splitLine;
			}
		}

		public Action ClickHandler
		{
			get;
			set;
		}

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(delegate
			{
				ClickHandler?.Invoke();
			});
		}
	}
}
