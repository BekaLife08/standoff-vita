using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using ExitGames.Client.Photon;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Game
{
	public class GameStateMachine
	{
		public delegate byte Router();

		private static readonly Log Log = Log.Create(typeof(GameStateMachine));

		private readonly GameStateRouter _router;

		private readonly Dictionary<byte, IGameState> _gameStates = new Dictionary<byte, IGameState>();

		private readonly IGameStateMachineListener _listener;

		private bool _gameFinished;

		public IGameState GameState
		{
			get;
			private set;
		}

		public GameStateMachine([NotNull] GameStateRouter router, [NotNull] IGameStateMachineListener listener)
		{
			if (router == null)
			{
				throw new ArgumentNullException("router");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			_router = router;
			_listener = listener;
			_gameStates[byte.MaxValue] = new GameFinishedState();
		}

		public void Register(IGameState gameState)
		{
			_router.CheckRoutingExists(gameState.Id);
			_gameStates[gameState.Id] = gameState;
		}

		public void Setup(GameController gameController)
		{
			foreach (IGameState value in _gameStates.Values)
			{
				value.Setup(gameController);
			}
		}

		public IEnumerator GameLoop()
		{
			_gameFinished = false;
			while (!InitIfNot())
			{
				yield return null;
			}
			while (!_gameFinished)
			{
				IGameState state = GameState;
				Update();
				if (_gameFinished)
				{
					break;
				}
				if (state == GameState)
				{
					yield return GameState.Instruction();
				}
				else
				{
					yield return null;
				}
			}
		}

		public void Update()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				byte gameStateId = PhotonNetwork.room.GetGameStateId();
				if (GameState.Id != gameStateId)
				{
					GoTo(gameStateId);
				}
			}
			if (GameState.Id == byte.MaxValue)
			{
				_listener.OnGameFinishedState();
				_gameFinished = true;
			}
			else
			{
				GameState.Update();
				ExitIfCan();
			}
		}

		private bool InitIfNot()
		{
			if (GameState != null)
			{
				return true;
			}
			byte gameStateId = PhotonNetwork.room.GetGameStateId();
			if (gameStateId == 0)
			{
				if (!PhotonNetwork.isMasterClient)
				{
					return false;
				}
				RouteFirst();
			}
			else
			{
				GoTo(gameStateId);
			}
			_listener.OnGameInit();
			return true;
		}

		private void RouteFirst()
		{
			byte b = _router.RouteNext(0);
			if (b == 0)
			{
				Log.Error("First GameState is None?");
			}
			else
			{
				GoTo(b);
			}
		}

		private void RouteNext()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				Log.Error("RouteNext available only on MasterClient");
				return;
			}
			byte toGameStateId = _router.RouteNext(GameState.Id);
			GoTo(toGameStateId);
		}

		private void GoTo(byte toGameStateId)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"Get state by Id({toGameStateId})");
			}
			if (!_gameStates.TryGetValue(toGameStateId, out IGameState value))
			{
				Log.Error($"Can't change game state to {toGameStateId}, is not found");
			}
			else
			{
				GoTo(value);
			}
		}

		private void GoTo([NotNull] IGameState toGameState)
		{
			if (toGameState == null)
			{
				throw new ArgumentNullException("toGameState");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug($"GoTo {toGameState}");
			}
			ExitState();
			GameState = toGameState;
			if (PhotonNetwork.isMasterClient)
			{
				ExitGames.Client.Photon.Hashtable hashtable = GameState.Init();
				hashtable.SetGameStateId(GameState.Id);
				PhotonNetwork.room.SetCustomProperties(hashtable);
			}
			GameState.Enter();
			_listener.OnGameStateChanged(GameState);
			if (Log.DebugEnabled)
			{
				Log.Debug($"State changed to {GameState.GetType().Name}");
			}
		}

		private void ExitState()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"ExitState {GameState}");
			}
			GameState?.Exit();
			if (PhotonNetwork.isMasterClient)
			{
				GameState?.Complete();
			}
		}

		private void ExitIfCan()
		{
			if (PhotonNetwork.isMasterClient && GameState.CheckExit())
			{
				RouteNext();
			}
		}
	}
}
