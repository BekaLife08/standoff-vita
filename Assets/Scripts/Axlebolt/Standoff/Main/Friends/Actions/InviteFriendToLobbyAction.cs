using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using I2.Loc;

namespace Axlebolt.Standoff.Main.Friends.Actions
{
	public class InviteFriendToLobbyAction : IFriendAction
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExecute_003Ec__async0 : IAsyncStateMachine
		{
			internal BoltFriend boltFriend;

			internal AsyncTaskMethodBuilder _0024builder;

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
						if (!BoltService<BoltMatchmakingService>.Instance.IsInLobby())
						{
							_0024awaiter0 = LobbyUtility.CreateLobby().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						goto IL_007d;
					case 1u:
						_0024awaiter0.GetResult();
						goto IL_007d;
					case 2u:
						break;
						IL_007d:
						_0024awaiter0 = LobbyUtility.InviteToLobby(boltFriend.Id).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					}
					_0024awaiter0.GetResult();
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
				return FriendActionId.InviteFriendToLobby;
			}
		}

		public string LocalizedText
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.FriendActions.InviteToLobby;
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExecute_003Ec__async0))]
		public Task Execute(BoltFriend boltFriend)
		{
			_003CExecute_003Ec__async0 stateMachine = default(_003CExecute_003Ec__async0);
			stateMachine.boltFriend = boltFriend;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public bool IsSupported(BoltFriend boltFriend)
		{
			if (BoltService<BoltMatchmakingService>.Instance.IsInLobby())
			{
				if (!BoltService<BoltMatchmakingService>.Instance.CurrentLobby.Joinable)
				{
					return false;
				}
				return !BoltService<BoltMatchmakingService>.Instance.CurrentLobby.IsLobbyMember(boltFriend);
			}
			return true;
		}
	}
}
