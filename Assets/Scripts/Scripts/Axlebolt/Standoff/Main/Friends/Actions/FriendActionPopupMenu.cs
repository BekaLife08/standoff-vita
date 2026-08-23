using System;
using System.Collections.Generic;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class FriendActionPopupMenu : PopupMenu
	{
		[SerializeField]
		private FriendActionId[] _ids;

		private BoltFriend _friend;

		private IFriendActionListener _listener;

		private readonly Dictionary<PopupItem, IFriendAction> _dictionary = new Dictionary<PopupItem, IFriendAction>();

		protected override void Awake()
		{
			base.Awake();
			FriendActionId[] ids = _ids;
			foreach (FriendActionId id in ids)
			{
				IFriendAction action = FriendActionExecutor.GetAction(id);
				if (action != null)
				{
					PopupItem popupItem = new PopupItem(action.LocalizedText);
					AddItem(popupItem);
					_dictionary[popupItem] = action;
				}
			}
			base.ActionHandler = ExecuteAction;
		}

		public void SetListener(IFriendActionListener listener)
		{
			_listener = listener;
		}

		public void Show([NotNull] BoltFriend friend, Vector2 position)
		{
			if (friend == null)
			{
				throw new ArgumentNullException("friend");
			}
			base.Show(position);
			_friend = friend;
			bool flag = false;
			foreach (PopupItem item in base.Items)
			{
				IFriendAction friendAction = _dictionary[item];
				bool flag2 = friendAction.IsSupported(_friend);
				flag = flag || flag2;
				SetItemVisible(item, flag2);
			}
			if (!flag)
			{
				Hide();
			}
		}

		private void ExecuteAction(PopupItem item)
		{
			IFriendAction friendAction = _dictionary[item];
			FriendActionExecutor.ExecuteAction(friendAction.Id, _friend, _listener);
		}
	}
}
