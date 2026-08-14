using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class RequestsTabController : TabController<RequestsTabController>, IAsyncDataProvider<BoltFriend>, IFriendActionListener
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadData_003Ec__async0 : IAsyncStateMachine
		{
			internal int page;

			internal int size;

			internal BoltFriend[] _003Cresult_003E__1;

			internal Action<BoltFriend[]> success;

			internal Action<Exception> failed;

			internal RequestsTabController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<BoltFriend[]> _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						num = 4294967293u;
						break;
					case 1u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_0024awaiter0 = _0024this._friendsService.GetFriendsByStatus(_0024this._filter, page, size).GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						case 1u:
							break;
						}
						_003Cresult_003E__1 = _0024awaiter0.GetResult();
						success(_003Cresult_003E__1);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
						failed(ex);
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private static readonly Log Log = Log.Create(typeof(RequestsTabController));

		private Color _selectedColor;

		private Color _unselectedColor;

		private Color _selectedTextColor;

		private Color _unselectedTextColor;

		[SerializeField]
		private Button _newRequestsFilter;

		[SerializeField]
		private Text _newRequestsText;

		[SerializeField]
		private Button _ignoredRequestsFilter;

		[SerializeField]
		private Text _ignoredRequestsText;

		[SerializeField]
		private Button _outgoingRequestsFilter;

		[SerializeField]
		private Text _outgoingRequestsText;

		[SerializeField]
		private FriendsPagingScrollView _scrollView;

		private BoltFriendsService _friendsService;

		private RelationshipStatus[] _filter;

		public override void Init()
		{
			base.Init();
			_friendsService = BoltService<BoltFriendsService>.Instance;
			_friendsService.FriendshipRequestCountChangedEvent.AddListener(OnRequestCountUpdated);
			_newRequestsFilter.onClick.AddListener(NewRequestsFilter);
			_ignoredRequestsFilter.onClick.AddListener(IngoredRequestsFilter);
			_outgoingRequestsFilter.onClick.AddListener(OutgoingRequestsFilter);
			_selectedColor = _newRequestsFilter.targetGraphic.color;
			_unselectedColor = _ignoredRequestsFilter.targetGraphic.color;
			_selectedTextColor = _newRequestsText.color;
			_unselectedTextColor = _ignoredRequestsText.color;
			_scrollView.Init(this, 20);
			_scrollView.SetActionListener(this);
		}

		public override void OnOpen()
		{
			NewRequestsFilter();
		}

		public override void OnClose()
		{
		}

		private void OnRequestCountUpdated(FriendshipRequestCountChangedArgs args)
		{
			OnNewNotificationEvent();
		}

		private void NewRequestsFilter()
		{
			SetSelected(_newRequestsFilter, _newRequestsText);
			SetFilter(new RelationshipStatus[1] { RelationshipStatus.RequestInitiator });
		}

		private void IngoredRequestsFilter()
		{
			SetSelected(_ignoredRequestsFilter, _ignoredRequestsText);
			SetFilter(new RelationshipStatus[1] { RelationshipStatus.Ignored });
		}

		private void OutgoingRequestsFilter()
		{
			SetSelected(_outgoingRequestsFilter, _outgoingRequestsText);
			SetFilter(new RelationshipStatus[1] { RelationshipStatus.RequestRecipient });
		}

		private void SetFilter(RelationshipStatus[] filter)
		{
			_filter = filter;
			_scrollView.Reload();
		}

		private void SetSelected(Selectable button, Graphic text)
		{
			_newRequestsFilter.targetGraphic.color = _unselectedColor;
			_newRequestsText.color = _unselectedTextColor;
			_ignoredRequestsFilter.targetGraphic.color = _unselectedColor;
			_ignoredRequestsText.color = _unselectedTextColor;
			_outgoingRequestsFilter.targetGraphic.color = _unselectedColor;
			_outgoingRequestsText.color = _unselectedTextColor;
			button.targetGraphic.color = _selectedColor;
			text.color = _selectedTextColor;
		}

		public void LoadCount(Action<int> success, Action<Exception> failed)
		{
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CLoadData_003Ec__async0))]
		public void LoadData(int page, int size, Action<BoltFriend[]> success, Action<Exception> failed)
		{
			_003CLoadData_003Ec__async0 stateMachine = default(_003CLoadData_003Ec__async0);
			stateMachine.page = page;
			stateMachine.size = size;
			stateMachine.success = success;
			stateMachine.failed = failed;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void OnFriendActionExecuted(FriendActionId friendAction, BoltFriend friend)
		{
			_scrollView.UpdateView(friend);
		}

		public override int GetNotificationsCount()
		{
			return _friendsService.GetFriendshipRequestCount();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (_friendsService != null)
			{
				_friendsService.FriendshipRequestCountChangedEvent.RemoveListener(OnRequestCountUpdated);
			}
		}
	}
}
