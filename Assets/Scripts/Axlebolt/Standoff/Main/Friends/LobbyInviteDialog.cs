using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbyInviteDialog : View
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnAccept_003Ec__async0 : IAsyncStateMachine
		{
			internal LobbyInviteDialog _0024this;

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
						_0024awaiter0 = LobbyUtility.JoinLobby(_0024this._lobbyInvite.LobbyId).GetAwaiter();
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
					_0024this.Hide();
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnReject_003Ec__async1 : IAsyncStateMachine
		{
			internal LobbyInviteDialog _0024this;

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
						_0024awaiter0 = LobbyUtility.RejectInvite(_0024this._lobbyInvite.LobbyId).GetAwaiter();
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
					_0024this.Hide();
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

		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Button _applyButton;

		[SerializeField]
		private Button _rejectButton;

		private Texture _defaultAvatar;

		private BoltLobbyInvite _lobbyInvite;

		private Action _callback;

		private bool _initialized;

		private void Awake()
		{
			_defaultAvatar = _avatarImage.texture;
			_applyButton.onClick.AddListener(OnAccept);
			_rejectButton.onClick.AddListener(OnReject);
			_initialized = true;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003COnAccept_003Ec__async0))]
		private void OnAccept()
		{
			_003COnAccept_003Ec__async0 stateMachine = default(_003COnAccept_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[AsyncStateMachine(typeof(_003COnReject_003Ec__async1))]
		[DebuggerStepThrough]
		private void OnReject()
		{
			_003COnReject_003Ec__async1 stateMachine = default(_003COnReject_003Ec__async1);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		public void Show([NotNull] BoltLobbyInvite lobbyInvite, [NotNull] Action callback)
		{
			if (lobbyInvite == null)
			{
				throw new ArgumentNullException("lobbyInvite");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_lobbyInvite = lobbyInvite;
			_callback = callback;
			if (!_initialized)
			{
				base.transform.SetParent(GameObject.Find("Canvas").transform, false);
			}
			base.Show();
			BoltFriend friend = lobbyInvite.Friend;
			_avatarImage.texture = ((friend.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(friend.Avatar));
			_nameText.text = friend.Name;
		}

		public override void Show()
		{
			throw new NotSupportedException();
		}

		public override void Hide()
		{
			base.Hide();
			Action callback = _callback;
			_lobbyInvite = null;
			_callback = null;
			callback();
		}
	}
}
