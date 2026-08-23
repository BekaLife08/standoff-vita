using System;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Messages;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Messages
{
	public class ChatMessagesView : View
	{
		[SerializeField]
		private LobbyInvitePanel _lobbyInvitePanel;

		[SerializeField]
		private ChatMessagesToolbar _toolbar;

		[SerializeField]
		private GameObject _progressIndicator;

		[SerializeField]
		private GameObject _errorView;

		[SerializeField]
		private Button _retryButton;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private Text _textArea;

		[SerializeField]
		private InputField _textField;

		private DateTime _oldMessageTime = DateTime.Now;

		private DateTime _newMessageTime = DateTime.Now;

		private ChatModel _currentChat;

		private string _messageTemplate;

		protected int Page;

		protected int PageSize;

		private Vector2 _value;

		private bool _isLoading;

		private bool _fullyLoaded;

		private bool _requestFailed;

		public LobbyInvitePanel LobbyInvitePanel
		{
			[CompilerGenerated]
			get
			{
				return _lobbyInvitePanel;
			}
		}

		public Action<string> SendMessageHandler { get; set; }

		protected IAsyncDataProvider<BoltUserMessage> AsyncDataProvider { get; private set; }

		private void Awake()
		{
			_messageTemplate = _textArea.text;
			_textArea.text = string.Empty;
			_scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
			HideProgress();
			_retryButton.onClick.AddListener(LoadPage);
			_textField.onEndEdit.AddListener(delegate(string msg)
			{
				_textField.text = string.Empty;
				if (!string.IsNullOrEmpty(msg))
				{
					SendMessageHandler(msg);
				}
			});
		}

		public virtual void Init(IAsyncDataProvider<BoltUserMessage> dataProvider, int pageSize)
		{
			AsyncDataProvider = dataProvider;
			PageSize = pageSize;
		}

		public void Show([NotNull] ChatModel chat)
		{
			if (chat == null)
			{
				throw new ArgumentNullException("chat");
			}
			Show();
			bool flag = !object.Equals(_currentChat, chat);
			_currentChat = chat;
			if (flag)
			{
				Reload();
			}
			_toolbar.Show(_currentChat.Chat);
			_lobbyInvitePanel.IsVisible = chat.LobbyInvite != null;
			if (chat.LobbyInvite != null)
			{
				_lobbyInvitePanel.SetInvite(chat.LobbyInvite);
			}
		}

		public override void Hide()
		{
			base.Hide();
			_textArea.text = string.Empty;
			Page = 0;
			_fullyLoaded = false;
			_isLoading = false;
			_currentChat = null;
			_toolbar.Hide();
		}

		private void OnScrollValueChanged(Vector2 value)
		{
			if (!_isLoading && !_requestFailed && AsyncDataProvider != null && Mathf.Approximately(value.y, 1f))
			{
				LoadPage();
			}
		}

		public virtual void Reload()
		{
			Page = 0;
			_fullyLoaded = false;
			_scrollRect.verticalNormalizedPosition = 0f;
			HideError();
			HideProgress();
			_textArea.text = string.Empty;
			LoadPage();
		}

		private void LoadPage()
		{
			if (!_fullyLoaded)
			{
				HideError();
				ShowProgress();
				_isLoading = true;
				_requestFailed = false;
				AsyncDataProvider.LoadData(Page, PageSize, LoadSuccess, LoadFailed);
			}
		}

		private void LoadSuccess(BoltUserMessage[] data)
		{
			_isLoading = false;
			HideProgress();
			AddLoadedMessages(data);
			if (data.Length == PageSize)
			{
				Page++;
				return;
			}
			if (!string.IsNullOrEmpty(_textArea.text))
			{
				_textArea.text = GetTimeText(_oldMessageTime) + _textArea.text;
			}
			_fullyLoaded = true;
		}

		private void LoadFailed(Exception ex)
		{
			_isLoading = false;
			_requestFailed = true;
			HideProgress();
			if (!(ex is OperationCanceledException))
			{
				ShowError();
			}
		}

		private void ShowProgress()
		{
			_progressIndicator.transform.SetAsLastSibling();
			_progressIndicator.SetActive(true);
		}

		private void HideProgress()
		{
			_progressIndicator.transform.SetAsFirstSibling();
			_progressIndicator.SetActive(false);
		}

		private void ShowError()
		{
			_errorView.gameObject.SetActive(true);
			_errorView.transform.SetAsLastSibling();
		}

		private void HideError()
		{
			_errorView.gameObject.SetActive(false);
			_errorView.transform.SetAsFirstSibling();
		}

		private void AddLoadedMessages(BoltUserMessage[] messages)
		{
			foreach (BoltUserMessage message in messages)
			{
				AddOldMessage(message);
			}
		}

		private void AddOldMessage(BoltUserMessage message)
		{
			DateTime dateTime = ChatUtility.FromUnixTime(message.Timestamp);
			if (_textArea.text == string.Empty)
			{
				_oldMessageTime = dateTime;
				_newMessageTime = dateTime;
			}
			if (!ChatUtility.IsDayEquals(dateTime, _oldMessageTime))
			{
				_textArea.text = GetTimeText(_oldMessageTime) + _textArea.text;
			}
			_oldMessageTime = dateTime;
			string text = ChatUtility.FormatHourMinutes(dateTime);
			string text2 = string.Format(_messageTemplate, string.Empty, GetMessageAuthorName(message), text, message.Message);
			_textArea.text = text2 + _textArea.text;
		}

		public void AddNewMessage(BoltUserMessage message)
		{
			DateTime dateTime = ChatUtility.FromUnixTime(message.Timestamp);
			if (_textArea.text == string.Empty)
			{
				_oldMessageTime = dateTime;
				_newMessageTime = dateTime;
			}
			if (!ChatUtility.IsDayEquals(dateTime, _newMessageTime))
			{
				_textArea.text += GetTimeText(dateTime);
			}
			_newMessageTime = dateTime;
			string text = ChatUtility.FormatHourMinutes(dateTime);
			string text2 = string.Format(_messageTemplate, string.Empty, GetMessageAuthorName(message), text, message.Message);
			_textArea.text += text2;
		}

		public void AddErrorMessage(string errorMsg)
		{
			string text = string.Format(_messageTemplate, errorMsg, string.Empty, string.Empty, string.Empty);
			_textArea.text += text;
		}

		private string GetTimeText(DateTime dateTime)
		{
			return string.Format("\n<align=\"center\">{0}</align>\n", ChatUtility.LocalizeDayMonthYear(dateTime));
		}

		private string GetMessageAuthorName(BoltUserMessage message)
		{
			if (message.Sender != null)
			{
				return message.Sender.Name;
			}
			return ScriptLocalization.Common.You;
		}

		public void SetActionListener(IFriendActionListener actionListener)
		{
			_toolbar.FriendActionPopupMenu.SetListener(actionListener);
		}
	}
}
