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
	public class FindFriendsTabController : TabController<FindFriendsTabController>, IAsyncDataProvider<BoltFriend>, IFriendActionListener
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

			internal FindFriendsTabController _0024this;

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
							_0024awaiter0 = BoltService<BoltFriendsService>.Instance.SearchFriends(_0024this.SearchFilter, page, size).GetAwaiter();
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

		private static readonly Log Log = Log.Create(typeof(FindFriendsTabController));

		[SerializeField]
		private FriendsPagingScrollView _scrollView;

		[SerializeField]
		private InputField _searchField;

		[SerializeField]
		private Button _searchButton;

		private string SearchFilter
		{
			[CompilerGenerated]
			get
			{
				return _searchField.text;
			}
		}

		public override void Init()
		{
			base.Init();
			_searchButton.onClick.AddListener(Reload);
			_scrollView.Init(this, 10);
			_scrollView.SetActionListener(this);
		}

		public override void OnOpen()
		{
			_searchField.text = string.Empty;
			Reload();
		}

		public override void OnClose()
		{
		}

		private void Reload()
		{
			_scrollView.IsLoadEnabled = !string.IsNullOrEmpty(SearchFilter);
			_scrollView.Reload();
		}

		public void LoadCount(Action<int> success, Action<Exception> failed)
		{
			success(0);
		}

		[AsyncStateMachine(typeof(_003CLoadData_003Ec__async0))]
		[DebuggerStepThrough]
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
	}
}
