using System;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.UI;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyChat : View
	{
		[SerializeField]
		private Text _messageTemplate;

		[SerializeField]
		private InputField _field;

		private string _pattern;

		public Action<string> SendMessageAction { get; set; }

		private void Awake()
		{
			_pattern = _messageTemplate.text;
			_messageTemplate.text = string.Empty;
			_field.onEndEdit.AddListener(delegate(string msg)
			{
				if (!string.IsNullOrEmpty(msg))
				{
					_field.text = string.Empty;
					Action<string> sendMessageAction = SendMessageAction;
					if (sendMessageAction != null)
					{
						sendMessageAction(msg);
					}
				}
			});
		}

		public override void Hide()
		{
			base.Hide();
			_messageTemplate.text = string.Empty;
		}

		public void AddPlayerMessage(BoltFriend author, string message)
		{
			string text = ((!author.IsLocal()) ? (author.Name + ": ") : (ScriptLocalization.Common.You + ": "));
			if (author.IsLocal())
			{
				_messageTemplate.text += string.Format(_pattern, text, string.Empty, message, string.Empty, string.Empty);
			}
			else
			{
				_messageTemplate.text += string.Format(_pattern, string.Empty, text, message, string.Empty, string.Empty);
			}
		}

		public void AddMessage(string message)
		{
			_messageTemplate.text += string.Format(_pattern, string.Empty, string.Empty, message, string.Empty, string.Empty);
		}

		public void AddWarnMessage(string message)
		{
			_messageTemplate.text += string.Format(_pattern, string.Empty, string.Empty, string.Empty, message, string.Empty);
		}

		public void AddErrorMessage(string message)
		{
			_messageTemplate.text += string.Format(_pattern, string.Empty, string.Empty, string.Empty, string.Empty, message);
		}
	}
}
