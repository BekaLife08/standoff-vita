using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Player;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class ShortProfileController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSetNameAsync_003Ec__async0 : IAsyncStateMachine
		{
			internal string newName;

			internal ShortProfileController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			private static Action<DialogButton> _003C_003Ef__am_0024cache0;

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
						if (string.IsNullOrEmpty(newName))
						{
							return;
						}
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
							_0024awaiter0 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Common.PleaseWait, ScriptLocalization.Common.SavingChanges), BoltService<BoltPlayerService>.Instance.SetName(newName)).GetAwaiter();
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
						_0024awaiter0.GetResult();
						_0024this.InternalSetPlayer();
					}
					catch (ConnectionFailedException)
					{
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						Dialog dialog = Dialogs.Create(ScriptLocalization.Common.Error, ex2.Message, new DialogButton(ScriptLocalization.Common.Ok));
						dialog.Show(delegate
						{
						});
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

			private static void _003C_003Em__0(DialogButton _)
			{
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSetAvatarAsync_003Ec__async1 : IAsyncStateMachine
		{
			internal byte[] avatar;

			internal ShortProfileController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			private static Action _003C_003Ef__am_0024cache0;

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
						if (avatar == null)
						{
							throw new ArgumentNullException("avatar");
						}
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
							if (Log.DebugEnabled)
							{
								Log.Debug(string.Format("SetAvatarAsync size {0}", avatar.Length));
							}
							_0024awaiter0 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Common.PleaseWait, ScriptLocalization.Common.SavingChanges), BoltService<BoltPlayerService>.Instance.SetAvatar(avatar)).GetAwaiter();
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
						_0024awaiter0.GetResult();
						_0024this.InternalSetPlayer();
					}
					catch (ConnectionFailedException message)
					{
						Log.Debug(message);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
						Dialogs.Message(ScriptLocalization.Common.Error, ex.Message, delegate
						{
						});
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

			private static void _003C_003Em__0()
			{
			}
		}

		private static readonly Log Log = Log.Create(typeof(ShortProfileController));

		[SerializeField]
		private ShortProfileView _shortProfileView;

		[SerializeField]
		private PlayerNameChangeDialog _nameChangeDialog;

		[SerializeField]
		private AvatarSelectDialog _avatarSelectDialog;

		private BoltPlayer _player;

		private void Awake()
		{
			_shortProfileView.NameChangeHandler = OnNameChangeHandler;
			_shortProfileView.AvatarChangeEvent = OnAvatarChangeHandler;
		}

		private void OnNameChangeHandler()
		{
			_nameChangeDialog.Name = BoltService<BoltPlayerService>.Instance.Player.Name;
			_nameChangeDialog.ActionHandler = delegate
			{
				SetNameAsync(_nameChangeDialog.Name);
			};
			_nameChangeDialog.Show();
		}

		[AsyncStateMachine(typeof(_003CSetNameAsync_003Ec__async0))]
		[DebuggerStepThrough]
		private void SetNameAsync(string newName)
		{
			_003CSetNameAsync_003Ec__async0 stateMachine = default(_003CSetNameAsync_003Ec__async0);
			stateMachine.newName = newName;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnAvatarChangeHandler()
		{
			_avatarSelectDialog.Show(delegate(Texture2D avatarTexture)
			{
				Log.Debug(string.Format("Avatar selected ({0})", avatarTexture));
				if (avatarTexture != null)
				{
					SetAvatarAsync(avatarTexture.EncodeToJPG());
				}
			});
		}

		[AsyncStateMachine(typeof(_003CSetAvatarAsync_003Ec__async1))]
		[DebuggerStepThrough]
		private void SetAvatarAsync([NotNull] byte[] avatar)
		{
			_003CSetAvatarAsync_003Ec__async1 stateMachine = default(_003CSetAvatarAsync_003Ec__async1);
			stateMachine.avatar = avatar;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void SetPlayer([NotNull] BoltPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			_player = player;
			InternalSetPlayer();
		}

		private void InternalSetPlayer()
		{
			_shortProfileView.SetPlayer(_player, true);
		}

		public void SetFriendPlayer([NotNull] BoltPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			_player = null;
			_shortProfileView.SetPlayer(player, false);
		}
	}
}
