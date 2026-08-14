using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Bolt.Analytics;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Inventory;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Messages;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Stats;
using Axlebolt.Bolt.Storage;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt
{
	public class BoltApi
	{
		private static BoltApi _instance;

		private static readonly List<Service> Services = new List<Service>();

		public BoltEvent ConnectionFailedEvent = new BoltEvent();

		private readonly AuthRemoteService _authRemoteService;

		private readonly HandshakeRemoteService _handshakeRemoteService;

		private string _ticket;

		private SynchronizationContext _synchronizationContext;

		private readonly string _gameId;

		private readonly string _gameVersion;

		private bool _handshakeSuccess;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ClientService _003CClientService_003Ek__BackingField;

		public static string TempPath { get; set; } = Path.GetTempPath();


		public bool SyncContexEnabled { get; set; } = true;


		public string Ip { get; set; } = "46.101.200.35";


		public int Port { get; set; } = 2222;


		public int HttpPort { get; set; } = 8080;


		public static BoltApi Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new BoltApiException("BoltApi is not initialized");
				}
				return _instance;
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return _instance != null;
			}
		}

		public bool IsConnectedAndReady
		{
			get
			{
				//return ClientService.ConnectionState == RpcSupport.ConnectionState.Connected && IsAuthenticated;
				return IsAuthenticated;
			}
		}

		public bool IsAuthenticated
		{
			get
			{
				return !string.IsNullOrEmpty(_ticket);
			}
		}

		public ClientService ClientService
		{
			[CompilerGenerated]
			get
			{
				return _003CClientService_003Ek__BackingField;
			}
		}

		public static void Init(string gameId, string gameVersion)
		{
			if (_instance != null)
			{
				throw new BoltApiException("BoltApi already initialized");
			}
			_instance = new BoltApi(gameId, gameVersion);
			BoltAvatars.Instance = new BoltAvatars(TempPath);
			Services.Add(new BoltPlayerService());
			Services.Add(new BoltPlayerStatsService());
			Services.Add(new BoltFriendsService());
			Services.Add(new BoltMatchmakingService());
			Services.Add(new BoltMessagesService());
			Services.Add(new BoltInventoryService());
			Services.Add(new BoltStorageService());
			Services.Add(new BoltAnalytics());
		}

		public static void Destroy()
		{
			if (_instance == null)
			{
				return;
			}
			_instance.ClientService.Disconnect();
			foreach (Service service in Services)
			{
				service.Destroy();
			}
			Services.Clear();
			BoltAvatars.Instance = null;
			_instance = null;
		}

		private BoltApi(string gameId, string gameVersion)
		{
			if (gameId == null)
			{
				throw new ArgumentNullException("gameId");
			}
			_gameId = gameId;
			_gameVersion = gameVersion;
			_003CClientService_003Ek__BackingField = new ClientService(Assembly.GetExecutingAssembly());
			ClientService.ConnectionFailed += OnConnectionFailedEvent;
			ClientService.RequestTimeout = 120000L;
			_authRemoteService = new AuthRemoteService(ClientService);
			_handshakeRemoteService = new HandshakeRemoteService(ClientService);
		}

		public Task AuthTest(string token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			_synchronizationContext = SynchronizationContext.Current;
			return Async(delegate
			{
				AuthTestInternal(token);
			}, false);
		}

		public Task AuthGPGS(string serverAuthCode)
		{
			if (string.IsNullOrEmpty(serverAuthCode))
			{
				throw new ArgumentNullException("serverAuthCode");
			}
			_synchronizationContext = SynchronizationContext.Current;
			return Async(delegate
			{
				AuthGPGSInternal(serverAuthCode);
			}, false);
		}

		public Task Reconnect()
		{
			return Async(ReconnectInternal);
		}

		public Task Logout()
		{
			return Async(LogoutInternal);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void Connect()
		{
			ClientService.Connect(Ip, 2222);
			if (Logger.LogDebug)
			{
				Logger.Debug("Connected successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void AuthGPGSInternal(string serverAuthCode)
		{
			Connect();
			_ticket = _authRemoteService.AuthGPGS(_gameId, _gameVersion, serverAuthCode);
			Handshake();
			if (Logger.LogDebug)
			{
				Logger.Debug("AuthGPGS successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void AuthTestInternal(string token)
		{
			_ticket = "hui";
			Handshake();
			if (Logger.LogDebug)
			{
				Logger.Debug("AuthTest successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void Handshake()
		{
			if (Logger.LogDebug)
			{
				Logger.Debug("Load services");
			}
			LoadServices();
			_handshakeSuccess = true;
			if (Logger.LogDebug)
			{
				Logger.Debug("Handshake successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void ReconnectInternal()
		{
			if (!IsAuthenticated)
			{
				throw new BoltApiException("Are you authenticated?");
			}
			Connect();
			Handshake();
			if (Logger.LogDebug)
			{
				Logger.Debug("Reconnect successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void LogoutInternal()
		{
			_ticket = null;
			ClientService.Disconnect();
			UnloadService();
			_handshakeSuccess = false;
			if (Logger.LogDebug)
			{
				Logger.Debug("Logout successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void DisconnectImmediate()
		{
			ClientService.Disconnect();
			OnConnectionFailedEvent();
			if (Logger.LogDebug)
			{
				Logger.Debug("DisconnectImmediate successfully!");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void OnConnectionFailedEvent()
		{
			if (Logger.LogDebug)
			{
				Logger.Debug("OnConnectionFailedEvent");
			}
			if (_handshakeSuccess)
			{
				if (Logger.LogDebug)
				{
					Logger.Debug("Unloading services");
				}
				UnloadService();
				_handshakeSuccess = false;
				BoltEvent connectionFailedEvent = ConnectionFailedEvent;
				if (connectionFailedEvent != null)
				{
					connectionFailedEvent.Invoke();
				}
			}
		}

		private void LoadServices()
		{
			try
			{
				ClientService.ProcessEvents = false;
				Load();
				BindEvents();
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
			finally
			{
				ClientService.ProcessEvents = true;
			}
		}

		private void UnloadService()
		{
			try
			{
				ClientService.ProcessEvents = false;
				UnbindEvents();
				Unload();
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
			finally
			{
				ClientService.ProcessEvents = true;
			}
		}

		private static void BindEvents()
		{
			foreach (Service service in Services)
			{
				try
				{
					service.BindEvents();
				}
				catch (Exception msg)
				{
					Logger.Error(msg);
				}
			}
		}

		private static void Unload()
		{
			foreach (Service service in Services)
			{
				try
				{
					service.Unload();
				}
				catch (Exception msg)
				{
					Logger.Error(msg);
				}
			}
		}

		private void Load()
		{
			Task[] tasks = Services.Select((Service service) => Async(delegate
			{
				try
				{
					service.Load();
				}
				catch (Exception msg)
				{
					Logger.Error(msg);
				}
			})).ToArray();
			Task.WaitAll(tasks);
		}

		private static void UnbindEvents()
		{
			foreach (Service service in Services)
			{
				try
				{
					service.UnbindEvents();
				}
				catch (Exception msg)
				{
					Logger.Error(msg);
				}
			}
		}

		internal Task Async(Action action, bool requireAuth = true)
		{
			return Task.Factory.StartNew(action);
		}

		internal Task<T> Async<T>(Func<T> action, bool requireAuth = true)
		{
			return Task.Factory.StartNew(action);
		}

		internal void AddEventListener(object eventListener)
		{
			ClientService.AddEventListener(eventListener);
		}

		internal void RemoveEventListener(object eventListener)
		{
			ClientService.RemoveEventListener(eventListener);
		}

		internal void ExecuteInMainThread(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (SyncContexEnabled)
			{
				_synchronizationContext.Post(delegate
				{
					action();
				}, null);
			}
			else
			{
				action();
			}
		}
	}
}
