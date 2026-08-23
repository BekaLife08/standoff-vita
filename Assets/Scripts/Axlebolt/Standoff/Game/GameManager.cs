using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Game
{
	public class GameManager : PunBehaviour
	{
		public delegate void ExitHandler(bool gameFinished, string withError);

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExitGameWithConfirm_003Ec__async1 : IAsyncStateMachine
		{
			internal GameManager _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

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
							_0024awaiter0 = _0024this.ExitGameWithConfirmAsync().GetAwaiter();
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
		private struct _003CExitGameWithConfirmAsync_003Ec__async2 : IAsyncStateMachine
		{
			internal Dialog _003Cdialog_003E__0;

			internal GameManager _0024this;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool result;
				try
				{
					switch (num)
					{
						default:
							return;
						case 0u:
							_003Cdialog_003E__0 = _0024this._gameController.CreateConfirmExitDialog();
							_0024awaiter0 = _003Cdialog_003E__0.ShowAndWait().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						case 1u:
							_0024awaiter0.GetResult();
							if (_003Cdialog_003E__0.ActionButton == Dialogs.Ok)
							{
								_0024awaiter1 = _0024this.ExitGameAsync(false, null).GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 2;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								goto case 2u;
							}
							result = false;
							break;
						case 2u:
							_0024awaiter1.GetResult();
							result = true;
							break;
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExitGame_003Ec__async3 : IAsyncStateMachine
		{
			internal bool gameFinished;

			internal GameManager _0024this;

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
							_0024awaiter0 = _0024this.ExitGameAsync(gameFinished, null).GetAwaiter();
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
		private struct _003CExitGameWithError_003Ec__async4 : IAsyncStateMachine
		{
			internal string error;

			internal GameManager _0024this;

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
							_0024awaiter0 = _0024this.ExitGameAsync(false, error).GetAwaiter();
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
		private struct _003CExitGameAsync_003Ec__async5 : IAsyncStateMachine
		{
			internal bool gameFinished;

			internal string error;

			internal GameManager _0024this;

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
							{
								if (_0024this._gameController != null)
								{
									_0024this._gameController.OnExitGame();
									_0024this._gameController.gameObject.SetActive(false);
								}
								PhotonPlayer player = PhotonNetwork.player;
								if (player != null)
								{
									player.Clear();
								}
								_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
						case 1u:
							break;
					}
					_0024awaiter0.GetResult();
					_playerAttr = null;
					_exitHandler(gameFinished, error);
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
		private struct _003CReconnect_003Ec__async8 : IAsyncStateMachine
		{
			internal float _003Ctime_003E__0;

			internal Exception _0024locvar0;

			internal GameManager _0024this;

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
							Log.Debug("Reconnect");
							_0024this._reconnectDialog.Show(20f);
							_003Ctime_003E__0 = Time.time;
							_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								flag = true;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						case 1u:
							_0024awaiter0.GetResult();
							goto IL_01cf;
						case 2u:
						case 3u:
							{
								Exception ex2 = default(Exception);
								int num2 = default(int);
								try
								{
									switch (num)
									{
										default:
											_0024awaiter0 = Task.Delay(500).GetAwaiter();
											if (!_0024awaiter0.IsCompleted)
											{
												_0024PC = 2;
												flag = true;
												_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
												return;
											}
											goto case 2u;
										case 2u:
											_0024awaiter0.GetResult();
											_0024awaiter0 = PhotonAsync.ReconnectAndRejoin(false).GetAwaiter();
											if (!_0024awaiter0.IsCompleted)
											{
												_0024PC = 3;
												flag = true;
												_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
												return;
											}
											break;
										case 3u:
											break;
									}
									_0024awaiter0.GetResult();
								}
								catch (Exception ex)
								{
									ex2 = ex;
									num2 = 1;
								}
								switch (num2)
								{
									case 1:
										break;
									default:
										goto IL_01cf;
								}
								_0024locvar0 = ex2;
								_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 4;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								goto case 4u;
							}
						case 4u:
							{
								_0024awaiter0.GetResult();
								goto IL_01cf;
							}
						IL_01cf:
							if (PhotonNetwork.inRoom || !(Time.time - _003Ctime_003E__0 < 20f))
							{
								break;
							}
							num = 4294967293u;
							goto case 2u;
					}
					_0024this._reconnectDialog.Hide();
					if (PhotonNetwork.inRoom)
					{
						Log.Debug("Reconnected successfully");
						_0024this._gameController.StartStateMachine();
						PhotonNetwork.isMessageQueueRunning = true;
					}
					else
					{
						Log.Debug("Reconnection failed");
						PhotonNetwork.isMessageQueueRunning = true;
						_0024this.ExitGameWithError(ScriptLocalization.GameExitMessages.ReconnectFailed);
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

		private static readonly Log Log = Log.Create(typeof(GameManager));

		private static ExitHandler _exitHandler;

		private static PlayerAttr _playerAttr;

		private static GameManager _instance;

		[SerializeField]
		private LevelLoadingView _loadingView;

		[SerializeField]
		private TutorialView _tutorialView;

		[SerializeField]
		private ReconnectDialog _reconnectDialog;

		private GameMode _gameMode;

		private LevelDefinition _level;

		private AsyncOperation _levelLoadingAsync;

		private GameController _gameController;

		public static GameManager Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new InvalidOperationException("Are you in game?");
				}
				return _instance;
			}
		}

		public static bool InGame
		{
			[CompilerGenerated]
			get
			{
				return _instance != null;
			}
		}

		public static void InitGame([NotNull] PlayerAttr playerAttr, [NotNull] ExitHandler exitHandler)
		{
			if (playerAttr == null)
			{
				throw new ArgumentNullException("playerAttr");
			}
			if (exitHandler == null)
			{
				throw new ArgumentNullException("exitHandler");
			}
			_playerAttr = playerAttr;
			_exitHandler = exitHandler;
			PhotonNetwork.isMessageQueueRunning = false;
			SetupSendRate();
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}

		private IEnumerator Start()
		{
			_instance = this;
			if (!PhotonNetwork.inRoom)
			{
				ExitGameWithError(ScriptLocalization.GameExitMessages.Disconnected);
				yield break;
			}
			try
			{
				_level = GetLevel();
				_gameMode = GetGameMode();
				InitPhotonPlayer();
				LoadLevel();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				_exitHandler(false, ex.Message);
			}
		}

		private static void SetupSendRate()
		{
			PhotonNetwork.sendRate = 15;
			PhotonNetwork.sendRateOnSerialize = 15;
		}

		internal void ReInitGame()
		{
			InitGame(_playerAttr, _exitHandler);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExitGameWithConfirm_003Ec__async1))]
		public void ExitGameWithConfirm()
		{
			_003CExitGameWithConfirm_003Ec__async1 stateMachine = default(_003CExitGameWithConfirm_003Ec__async1);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExitGameWithConfirmAsync_003Ec__async2))]
		public Task<bool> ExitGameWithConfirmAsync()
		{
			_003CExitGameWithConfirmAsync_003Ec__async2 stateMachine = default(_003CExitGameWithConfirmAsync_003Ec__async2);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			AsyncTaskMethodBuilder<bool> _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExitGame_003Ec__async3))]
		private void ExitGame(bool gameFinished)
		{
			_003CExitGame_003Ec__async3 stateMachine = default(_003CExitGame_003Ec__async3);
			stateMachine.gameFinished = gameFinished;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CExitGameWithError_003Ec__async4))]
		private void ExitGameWithError(string error)
		{
			_003CExitGameWithError_003Ec__async4 stateMachine = default(_003CExitGameWithError_003Ec__async4);
			stateMachine.error = error;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[AsyncStateMachine(typeof(_003CExitGameAsync_003Ec__async5))]
		[DebuggerStepThrough]
		private Task ExitGameAsync(bool gameFinished, string error)
		{
			_003CExitGameAsync_003Ec__async5 stateMachine = default(_003CExitGameAsync_003Ec__async5);
			stateMachine.gameFinished = gameFinished;
			stateMachine.error = error;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			AsyncTaskMethodBuilder _0024builder = stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public void GameFinished()
		{
			ExitGame(true);
		}

		private LevelDefinition GetLevel()
		{
			string levelName = PhotonNetwork.room.GetLevelName();
			return LevelUtility.GetLevel(levelName);
		}

		private GameMode GetGameMode()
		{
			string gameModeName = PhotonNetwork.room.GetGameModeName();
			return GameModeUtility.GetByName(gameModeName);
		}

		private static void InitPhotonPlayer()
		{
			PhotonNetwork.player.Clear();
			PhotonNetwork.playerName = _playerAttr.Name;
			PhotonNetwork.player.SetUid(_playerAttr.Uid);
			PhotonNetwork.player.SetBadgeId(_playerAttr.BadgeId);
			AvatarSupport.SetPlayerAvatar(_playerAttr.Avatar);
		}

		private void LoadLevel()
		{
			StartCoroutine(LoadLevelAsync());
		}

		private IEnumerator LoadLevelAsync()
		{
			_loadingView.Level = _level;
			_loadingView.GameMode = _gameMode;
			_loadingView.LoadingText = ScriptLocalization.LevelLoadingView.Loading;
			_loadingView.Show();
			SettingsManager.InitIfNeed();
			_levelLoadingAsync = SceneManager.LoadSceneAsync(_level.SceneName, LoadSceneMode.Additive);
			while (!_levelLoadingAsync.isDone)
			{
				_loadingView.Progress = _levelLoadingAsync.progress;
				yield return null;
			}
			_levelLoadingAsync = null;
			_gameController = UnityEngine.Object.Instantiate(_gameMode.Controller);
			yield return _gameController.Init(_level);
			yield return new WaitForSeconds(0.1f);
			PhotonNetwork.isMessageQueueRunning = true;
			yield return WaitForInit();
		}

		private IEnumerator WaitForInit()
		{
			_loadingView.Progress = 1f;
			_loadingView.LoadingText = ScriptLocalization.LevelLoadingView.InitWaiting;
			while (PhotonNetwork.inRoom && !PhotonNetwork.isMasterClient && PhotonNetwork.room.GetGameStateId() == 0)
			{
				yield return null;
			}
			yield return _gameController.InitNetwork();
			StartGame();
		}

		private void StartGame()
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
			_loadingView.Hide();
			_gameController.StartGame();
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(_level.SceneName));
			InitTutorial();
			InitAntiCheats();
		}

		private void InitTutorial()
		{
			if (PlayerPrefs.GetInt("tutorial", 0) == 0)
			{
				PlayerPrefs.SetInt("tutorial", 1);
				_tutorialView.Show();
			}
			else
			{
				UnityEngine.Object.Destroy(_tutorialView.gameObject);
			}
			_tutorialView = null;
		}

		private void InitAntiCheats()
		{
		}

		private void OnSpeedHackDetected()
		{
			ExitGameWithError("Speed hack detected!");
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CReconnect_003Ec__async8))]
		public void Reconnect()
		{
			_003CReconnect_003Ec__async8 stateMachine = default(_003CReconnect_003Ec__async8);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnDestroy()
		{
			_instance = null;
		}
	}
}
