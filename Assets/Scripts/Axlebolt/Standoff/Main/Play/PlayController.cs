using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Main.Friends;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Main.Inventory.Medals;
using Axlebolt.Standoff.Main.Profile;
using Axlebolt.Standoff.Matchmaking;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Play
{
	public class PlayController : TabController<PlayController>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFindGame_003Ec__async1 : IAsyncStateMachine
		{
			internal string gameModeName;

			internal PlayController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

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
							_0024awaiter0 = _0024this.Find(gameModeName).GetAwaiter();
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
					}
					catch (OperationCanceledException)
					{
					}
					catch (PhotonConnectionFailedException)
					{
						_0024this.ConnectionFailed();
					}
					catch (Exception ex3)
					{
						_0024this.UnexpectedException(ex3);
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFind_003Ec__async2 : IAsyncStateMachine
		{
			internal string gameModeName;

			internal GameMode _003CgameMode_003E__0;

			internal MatchmakingFilter _003Cfilter_003E__0;

			internal PlayController _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<MatchmakingResult> _0024awaiter0;

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
						_0024this._gameModeName = gameModeName;
						_003CgameMode_003E__0 = GameModeUtility.GetByName(gameModeName);
						_003Cfilter_003E__0 = new MatchmakingFilter(_003CgameMode_003E__0)
						{
							AllowEmptyRoom = true,
							PlayerId = BoltService<BoltPlayerService>.Instance.Player.Id,
							TimeInGame = BoltService<BoltPlayerService>.Instance.Player.TimeInGame
						};
						_0024awaiter0 = _0024this._matchmakingHelper.Find(_003Cfilter_003E__0).GetAwaiter();
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
					LobbyUtility.StartGame();
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

		private static readonly Log Log = Log.Create(typeof(PlayController));

		[SerializeField]
		private Button _playButton;

		[SerializeField]
		private ShortProfileController _shortProfileController;

		[SerializeField]
		private FriendsScrollView _friendsScrollView;

		[SerializeField]
		private Text _inGameText;

		[SerializeField]
		private Button _medalsButton;

		[SerializeField]
		private MatchmakingHelper _matchmakingHelper;

		[SerializeField]
		private GameModeDialog _gameModeDialog;

		private string _gameModeName;

		private MedalsController _medalsController;

		private string _inGameCountPattern;

		private BoltFriendsService _friendsService;

		public override void Init()
		{
			base.Init();
			_playButton.onClick.AddListener(_gameModeDialog.Show);
			_friendsService = BoltService<BoltFriendsService>.Instance;
			_inGameCountPattern = _inGameText.text;
			Singleton<InventoryManager>.Instance.Init();
			Singleton<InAppManager>.Instance.InitializedEvent += delegate
			{
				if (base.IsOpen)
				{
					_shortProfileController.SetPlayer(BoltService<BoltPlayerService>.Instance.Player);
				}
			};
			Singleton<InAppManager>.Instance.PurchaseSuccessfullyEvent += delegate
			{
				if (base.IsOpen)
				{
					_shortProfileController.SetPlayer(BoltService<BoltPlayerService>.Instance.Player);
				}
			};
			AsyncUtility.StartCoroutine(InitMedals());
			_gameModeDialog.Hide();
			_gameModeDialog.DeathMatchHandler = delegate
			{
				FindGame("DeathMatch");
			};
			_gameModeDialog.DefuseHandler = delegate
			{
				FindGame("Defuse");
			};
		}

		private IEnumerator InitMedals()
		{
			SceneManager.LoadScene("Medals", LoadSceneMode.Additive);
			yield return null;
			_medalsController = UnityEngine.Object.FindObjectOfType<MedalsController>();
			_medalsController.Init();
			Canvas canvas = ViewUtility.GetCanvas(base.transform);
			_medalsController.OpenStateChangedEvent += delegate(bool isOpen)
			{
				SceneManager.SetActiveScene((!isOpen) ? canvas.gameObject.scene : _medalsController.gameObject.scene);
				canvas.gameObject.SetActive(!isOpen);
			};
			_medalsButton.onClick.AddListener(delegate
			{
				_medalsController.Open();
			});
		}

		public override void OnOpen()
		{
			_playButton.gameObject.SetActive(true);
			_shortProfileController.gameObject.SetActive(true);
			_shortProfileController.SetPlayer(BoltService<BoltPlayerService>.Instance.Player);
			_friendsService.FriendAddedEvent.AddListener(OnFriendUpdated);
			_friendsService.FriendRemovedEvent.AddListener(OnFriendUpdated);
			_friendsService.FriendUpdatedEvent.AddListener(OnFriendUpdated);
			Reload();
		}

		public override void OnClose()
		{
			_playButton.gameObject.SetActive(false);
			_shortProfileController.gameObject.SetActive(false);
			_friendsService.FriendAddedEvent.RemoveListener(OnFriendUpdated);
			_friendsService.FriendRemovedEvent.RemoveListener(OnFriendUpdated);
			_friendsService.FriendUpdatedEvent.RemoveListener(OnFriendUpdated);
		}

		private void OnFriendUpdated(BoltFriend friend)
		{
			Reload();
		}

		private void Reload()
		{
			_friendsScrollView.ReplaceContent(_friendsService.GetFriends().OrderByDescending((BoltFriend friend) => friend, new FriendComparer()).ToArray());
			int num = _friendsService.GetFriends().Count((BoltFriend friend) => friend.OnlineStatus == OnlineStatus.StateOnline);
			int num2 = _friendsService.GetFriends().Length;
			string online = ScriptLocalization.PlayerStatus.Online;
			_inGameText.text = string.Format(_inGameCountPattern, num, num2, online);
		}

		[AsyncStateMachine(typeof(_003CFindGame_003Ec__async1))]
		[DebuggerStepThrough]
		private void FindGame(string gameModeName)
		{
			_003CFindGame_003Ec__async1 stateMachine = default(_003CFindGame_003Ec__async1);
			stateMachine.gameModeName = gameModeName;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[AsyncStateMachine(typeof(_003CFind_003Ec__async2))]
		[DebuggerStepThrough]
		private Task Find(string gameModeName)
		{
			_003CFind_003Ec__async2 stateMachine = default(_003CFind_003Ec__async2);
			stateMachine.gameModeName = gameModeName;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
			return stateMachine._0024builder.Task;
		}

		private void ConnectionFailed()
		{
			string connectionFailed = ScriptLocalization.Dialogs.ConnectionFailed;
			string serverConnectionFailed = ScriptLocalization.Common.ServerConnectionFailed;
			Dialogs.Message(connectionFailed, serverConnectionFailed, FindAgain);
		}

		private void UnexpectedException(Exception ex)
		{
			Log.Error(ex);
			string connectionFailed = ScriptLocalization.Dialogs.ConnectionFailed;
			string message = ex.Message;
			Dialogs.Message(connectionFailed, message, FindAgain);
		}

		private void FindAgain()
		{
			FindGame(_gameModeName);
		}
	}
}
