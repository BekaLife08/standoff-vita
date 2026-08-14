using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Friends.Actions;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public class BlockedTabController : TabController<BlockedTabController>, IAsyncDataProvider<BoltFriend>, IFriendActionListener
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadData_003Ec__async0 : IAsyncStateMachine
		{
			internal RelationshipStatus[] _003Cfilter_003E__1;

			internal int page;

			internal int size;

			internal BoltFriend[] _003Cresult_003E__1;

			internal Action<BoltFriend[]> success;

			internal Action<Exception> failed;

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
							_003Cfilter_003E__1 = new RelationshipStatus[1] { RelationshipStatus.Blocked };
							_0024awaiter0 = BoltService<BoltFriendsService>.Instance.GetFriendsByStatus(_003Cfilter_003E__1, page, size).GetAwaiter();
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

		[NotNull]
		[SerializeField]
		private FriendsPagingScrollView _scrollView;

		public override void Init()
		{
			base.Init();
			_scrollView.Init(this, 20);
			_scrollView.SetActionListener(this);
		}

		public override void OnOpen()
		{
			Reload();
		}

		public override void OnClose()
		{
		}

		private void Reload()
		{
			_scrollView.Reload();
		}

		public void LoadCount(Action<int> success, Action<Exception> failed)
		{
			success(0);
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
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void OnFriendActionExecuted(FriendActionId friendAction, BoltFriend friend)
		{
			_scrollView.UpdateView(friend);
		}
	}
}
