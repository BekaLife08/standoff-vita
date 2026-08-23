using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Matchmaking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Test
{
	public class GameTesting : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CStart_003Ec__async0 : IAsyncStateMachine
		{
			internal MatchmakingFilter _003Cfilter_003E__0;

			internal GameTesting _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<MatchmakingResult> _0024awaiter0;

			private static GameManager.ExitHandler _003C_003Ef__am_0024cache0;

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
						_003Cfilter_003E__0 = new MatchmakingFilter(_0024this.GameMode)
						{
							PlayerId = "Unknown" + UnityEngine.Random.Range(0, 10000),
							AllowEmptyRoom = true,
							Levels = new LevelDefinition[1] { _0024this.LevelDefinition },
							LobbyName = "GameTesting"
						};
						_0024awaiter0 = MatchmakingManager.Find("fra", _003Cfilter_003E__0).GetAwaiter();
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
					GameManager.InitGame(PlayerAttr.Random(), delegate
					{
						SceneManager.LoadScene("GameTesting", LoadSceneMode.Single);
					});
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

			private static void _003C_003Em__0(bool finished, string error)
			{
				SceneManager.LoadScene("GameTesting", LoadSceneMode.Single);
			}
		}

		public GameMode GameMode;

		public LevelDefinition LevelDefinition;

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CStart_003Ec__async0))]
		private void Start()
		{
			_003CStart_003Ec__async0 stateMachine = default(_003CStart_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
