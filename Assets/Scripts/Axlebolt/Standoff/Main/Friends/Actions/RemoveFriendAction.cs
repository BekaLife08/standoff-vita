using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class RemoveFriendAction : IFriendAction
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExecute_003Ec__async0 : IAsyncStateMachine
		{
			internal Dialog _003Cconfirm_003E__0;

			internal BoltFriend friend;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						_003Cconfirm_003E__0 = Dialogs.Confirm(ScriptLocalization.Common.Confirmation, ScriptLocalization.Friends.DeleteConfirmDialog);
						_0024awaiter0 = _003Cconfirm_003E__0.ShowAndWait().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						if (_003Cconfirm_003E__0.ActionButton == Dialogs.Ok)
						{
							_0024awaiter1 = BoltService<BoltFriendsService>.Instance.RemoveFriend(friend).GetAwaiter();
							if (!_0024awaiter1.IsCompleted)
							{
								_0024PC = 2;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
								return;
							}
							break;
						}
						goto end_IL_000e;
					case 2u:
						break;
					}
					_0024awaiter1.GetResult();
					end_IL_000e:;
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

		public FriendActionId Id
		{
			[CompilerGenerated]
			get
			{
				return FriendActionId.RemoveFriend;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.RemoveFriend;
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExecute_003Ec__async0))]
		public Task Execute(BoltFriend friend)
		{
			_003CExecute_003Ec__async0 stateMachine = default(_003CExecute_003Ec__async0);
			stateMachine.friend = friend;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		public bool IsSupported(BoltFriend friend)
		{
			return friend.Relationship == RelationshipStatus.Friend;
		}
	}
}
