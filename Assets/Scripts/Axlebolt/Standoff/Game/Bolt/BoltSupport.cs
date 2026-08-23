using Axlebolt.Bolt;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.Event;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Bolt
{
	public class BoltSupport : MonoBehaviour
	{
		private static readonly Log Log = Log.Create(typeof(BoltSupport));

		private const int ReconnectInterval = 15;

		private PlayerStatsCollector _playerStatsCollector;

		private void Awake()
		{
			if (BoltApi.IsInitialized && BoltApi.Instance.IsAuthenticated)
			{
				Setup();
			}
		}

		private void Setup()
		{
			InitStatsCollector();
			InitMessages();
			BoltApi.Instance.ConnectionFailedEvent.AddListener(Reconnect);
			if (!BoltApi.Instance.IsConnectedAndReady)
			{
				Reconnect();
			}
			GameController.Instance.GameExitEvent.AddListener(OnGameExit);
		}

		private void InitMessages()
		{
			base.gameObject.AddComponent<MessagesSupport>();
		}

		private void InitStatsCollector()
		{
			if (BoltService<BoltMatchmakingService>.Instance.IsInLobby())
			{
				if (BoltService<BoltMatchmakingService>.Instance.CurrentLobby.Type == BoltLobby.LobbyType.Public)
				{
					_playerStatsCollector = base.gameObject.AddComponent<PlayerStatsCollector>();
				}
			}
			else
			{
				_playerStatsCollector = base.gameObject.AddComponent<PlayerStatsCollector>();
			}
		}

		private void Reconnect()
		{
			StartCoroutine(ReconnectLoop());
		}

		private IEnumerator ReconnectLoop()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("Trying reconnect to Bolt");
			}
			Task task = BoltApi.Instance.Reconnect();
			while (!task.IsCompleted)
			{
				yield return new WaitForSeconds(1f);
			}
			if (!BoltApi.Instance.IsConnectedAndReady)
			{
				if (Log.DebugEnabled)
				{
					Log.Debug($"Reconnect to the Bolt after {15} seconds");
				}
				yield return new WaitForSeconds(15f);
				yield return ReconnectLoop();
			}
			else
			{
				Log.Debug("Reconnected successfully");
			}
		}

		public void OnGameExit(GameExitEventArgs eventArgs)
		{
			StopAllCoroutines();
			if (BoltApi.Instance.IsConnectedAndReady && (object)_playerStatsCollector != null)
			{
				_playerStatsCollector.FlushChanges();
			}
		}

		private void OnDestroy()
		{
			if (BoltApi.IsInitialized)
			{
				BoltApi.Instance.ConnectionFailedEvent.RemoveListener(Reconnect);
			}
		}
	}
}
