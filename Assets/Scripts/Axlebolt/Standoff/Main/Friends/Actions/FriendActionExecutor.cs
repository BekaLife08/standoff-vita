using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt.Friends;
using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class FriendActionExecutor
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExecuteAction_003Ec__async0 : IAsyncStateMachine
		{
			internal FriendActionId id;

			internal BoltFriend friend;

			internal IFriendActionListener listener;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

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
						_0024awaiter0 = GetAction(id).Execute(friend).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
					IFriendActionListener friendActionListener = listener;
					if (friendActionListener != null)
					{
						friendActionListener.OnFriendActionExecuted(id, friend);
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

		private static readonly Log Log;

		private static readonly List<IFriendAction> Actions;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private static readonly FriendActionId[] _003CIds_003Ek__BackingField;

		public static FriendActionId[] Ids
		{
			[CompilerGenerated]
			get
			{
				return _003CIds_003Ek__BackingField;
			}
		}

		static FriendActionExecutor()
		{
			Log = Log.Create(typeof(FriendActionPopupMenu));
			Actions = new List<IFriendAction>();
			Add(new AddFriendAction());
			Add(new RemoveFriendAction());
			Add(new BlockFriendAction());
			Add(new UnblockFriendAction());
			Add(new RevokeInviteAction());
			Add(new IgnoreFriendAction());
			Add(new SendFriendMessageAction());
			Add(new InviteFriendToLobbyAction());
			Add(new KickLobbyMemberAction());
			Add(new RevokeLobbyInviteAction());
			Add(new JoinGameOrLobbyAction());
			_003CIds_003Ek__BackingField = Enum.GetValues(typeof(FriendActionId)).Cast<FriendActionId>().ToArray();
		}

		private static void Add(IFriendAction action)
		{
			if (string.IsNullOrEmpty(action.LocalizedText))
			{
				Log.Error(string.Format("Action {0} is invalid (LocalizedText is null or empty)", action.Id));
			}
			else
			{
				Actions.Add(action);
			}
		}

		public static IFriendAction GetAction(FriendActionId id)
		{
			return Actions.First((IFriendAction action) => action.Id == id);
		}

		public static bool IsSupported(FriendActionId id, BoltFriend friend)
		{
			return GetAction(id).IsSupported(friend);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExecuteAction_003Ec__async0))]
		public static void ExecuteAction(FriendActionId id, BoltFriend friend, IFriendActionListener listener = null)
		{
			_003CExecuteAction_003Ec__async0 stateMachine = default(_003CExecuteAction_003Ec__async0);
			stateMachine.id = id;
			stateMachine.friend = friend;
			stateMachine.listener = listener;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
