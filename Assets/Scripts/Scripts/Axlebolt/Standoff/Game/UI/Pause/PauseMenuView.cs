using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class PauseMenuView : View
	{
		private static readonly Log Log = Log.Create(typeof(PauseMenuView));

		[NotNull]
		[SerializeField]
		private Text _titleText;

		[SerializeField]
		[NotNull]
		private Button _itemPrefab;

		private readonly Dictionary<KeyCode, UnityAction> _actionByKeyCode = new Dictionary<KeyCode, UnityAction>();

		public string Title
		{
			set
			{
				_titleText.text = value;
			}
		}

		private void Awake()
		{
			_itemPrefab.gameObject.SetActive(value: false);
			if (_itemPrefab.GetComponentInChildren<Text>() == null)
			{
				Log.Error($"{_itemPrefab} invalid, text component not found");
			}
		}

		public void AddItem([NotNull] string text, [NotNull] UnityAction onClick, KeyCode keyCode = KeyCode.None)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (onClick == null)
			{
				throw new ArgumentNullException("onClick");
			}
			Button button = UnityEngine.Object.Instantiate(_itemPrefab, _itemPrefab.transform.parent, worldPositionStays: false);
			button.gameObject.SetActive(value: true);
			button.GetComponentInChildren<Text>().text = text;
			button.onClick.AddListener(onClick);
			if (keyCode != 0)
			{
				_actionByKeyCode[keyCode] = onClick;
			}
		}

		private void Update()
		{
			foreach (KeyValuePair<KeyCode, UnityAction> item in _actionByKeyCode)
			{
				if (UnityEngine.Input.GetKeyUp(item.Key))
				{
					item.Value();
				}
			}
		}
	}
}
