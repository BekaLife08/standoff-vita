using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Matchmaking.Events;
using Axlebolt.Bolt.Matchmaking.Mappers;
using Axlebolt.Bolt.Player;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt.Matchmaking
{
	public class BoltMatchmakingService : BoltService<BoltMatchmakingService>
	{
		public class MatchmakingEventListener : IMatchmakingRemoteEventListener
		{
			private readonly BoltMatchmakingService _boltMatchmaking;

			public MatchmakingEventListener(BoltMatchmakingService boltMatchmaking)
			{
				_boltMatchmaking = boltMatchmaking;
			}

			public void OnNewPlayerJoinedLobby(PlayerFriend protoNewPlayer)
			{
				NewPlayerJoinedEventArgs arg;
				lock (Lock)
				{
					BoltFriend player = FriendMapper.Instance.ToOriginal(protoNewPlayer);
					BoltAvatars.Instance.Fill(player);
					List<BoltLobbyInvite> list = new List<BoltLobbyInvite>(_boltMatchmaking.LobbyInvitesOutcoming);
					if (list.RemoveAll((BoltLobbyInvite item) => item.Friend.Id == player.Id) > 0)
					{
						_boltMatchmaking.LobbyInvitesOutcoming = list;
						_boltMatchmaking.OutcomingInvitesChangedEvent.Invoke();
					}
					_boltMatchmaking.CurrentLobby.AddLobbyMember(player);
					_boltMatchmaking.CurrentLobby.RemoveLobbyInvite(player);
					arg = new NewPlayerJoinedEventArgs(player);
				}
				_boltMatchmaking.NewPlayerJoinedEvent.Invoke(arg);
			}

			public void OnNewPlayerInvitedToLobby(string inviteSenderId, PlayerFriend protoNewPlayer)
			{
				NewPlayerInvitedEventArgs arg;
				lock (Lock)
				{
					BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(inviteSenderId);
					BoltFriend boltFriend = FriendMapper.Instance.ToOriginal(protoNewPlayer);
					BoltAvatars.Instance.Fill(lobbyMember);
					BoltAvatars.Instance.Fill(boltFriend);
					if (lobbyMember.IsLocal() && !_boltMatchmaking.LobbyInvitesOutcoming.Exists((BoltLobbyInvite item) => item.Friend.Id == protoNewPlayer.Player.Id))
					{
						BoltLobbyInvite item2 = new BoltLobbyInvite(_boltMatchmaking.CurrentLobby.Id, boltFriend, GetCurrentMillis());
						List<BoltLobbyInvite> lobbyInvitesOutcoming = new List<BoltLobbyInvite>(_boltMatchmaking.LobbyInvitesOutcoming) { item2 };
						_boltMatchmaking.LobbyInvitesOutcoming = lobbyInvitesOutcoming;
						_boltMatchmaking.OutcomingInvitesChangedEvent.Invoke();
					}
					_boltMatchmaking.CurrentLobby.AddLobbyInvite(boltFriend);
					arg = new NewPlayerInvitedEventArgs(lobbyMember, boltFriend);
				}
				_boltMatchmaking.NewPlayerInvitedEvent.Invoke(arg);
			}

			public void OnReceivedInviteToLobby(PlayerFriend inviteSender, string lobbyId)
			{
				ReceivedInviteEventArgs arg;
				lock (Lock)
				{
					BoltLobbyInvite lobbyInvite = new BoltLobbyInvite(lobbyId, FriendMapper.Instance.ToOriginal(inviteSender), GetCurrentMillis());
					BoltAvatars.Instance.Fill(lobbyInvite.Friend);
					List<BoltLobbyInvite> list = new List<BoltLobbyInvite>(_boltMatchmaking.LobbyInvitesIncoming);
					list.RemoveAll((BoltLobbyInvite item) => item.LobbyId == lobbyInvite.LobbyId);
					list.Add(lobbyInvite);
					_boltMatchmaking.LobbyInvitesIncoming = list;
					arg = new ReceivedInviteEventArgs(lobbyInvite);
				}
				_boltMatchmaking.IncomingInvitesChangedEvent.Invoke();
				_boltMatchmaking.ReceivedInviteEvent.Invoke(arg);
			}

			public void OnPlayerLeftLobby(string leftPlayerId)
			{
				PlayerLeftEventArgs arg;
				lock (Lock)
				{
					BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(leftPlayerId);
					_boltMatchmaking.CurrentLobby.RemoveLobbyMember(lobbyMember);
					if (leftPlayerId == BoltService<BoltPlayerService>.Instance.Player.Id)
					{
						_boltMatchmaking._currentLobby = null;
					}
					arg = new PlayerLeftEventArgs(lobbyMember);
				}
				_boltMatchmaking.PlayerLeftEvent.Invoke(arg);
			}

			public void OnPlayerKickedFromLobby(string kickInitiatorId, string playerId)
			{
				PlayerKickedEventArgs arg;
				lock (Lock)
				{
					BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(kickInitiatorId);
					BoltFriend lobbyMember2 = _boltMatchmaking.CurrentLobby.GetLobbyMember(playerId);
					_boltMatchmaking.CurrentLobby.RemoveLobbyMember(lobbyMember2);
					if (lobbyMember2.IsLocal())
					{
						_boltMatchmaking._currentLobby = null;
						_boltMatchmaking.LobbyInvitesOutcoming = new List<BoltLobbyInvite>();
					}
					arg = new PlayerKickedEventArgs(lobbyMember, lobbyMember2);
				}
				_boltMatchmaking.PlayerKickedEvent.Invoke(arg);
			}

			public void OnRefuseInviteToLobby(string inviteSenderId, string invitedPlayerId)
			{
				PlayerRefusedEventArgs arg;
				lock (Lock)
				{
					BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(inviteSenderId);
					BoltFriend lobbyInvite = _boltMatchmaking.CurrentLobby.GetLobbyInvite(invitedPlayerId);
					_boltMatchmaking.CurrentLobby.RemoveLobbyInvite(lobbyInvite);
					List<BoltLobbyInvite> list = new List<BoltLobbyInvite>(_boltMatchmaking.LobbyInvitesOutcoming);
					list.RemoveAll((BoltLobbyInvite inv) => inv.Friend.Id == invitedPlayerId);
					_boltMatchmaking.LobbyInvitesOutcoming = list;
					arg = new PlayerRefusedEventArgs(lobbyMember, lobbyInvite);
				}
				_boltMatchmaking.OutcomingInvitesChangedEvent.Invoke();
				_boltMatchmaking.PlayerRefusedEvent.Invoke(arg);
			}

			public void OnRevokeInviteToLobby(string inviteSenderId, string invitedPlayerId)
			{
				lock (Lock)
				{
					if (BoltService<BoltPlayerService>.Instance.Player.Id == invitedPlayerId)
					{
						List<BoltLobbyInvite> list = new List<BoltLobbyInvite>(_boltMatchmaking.LobbyInvitesIncoming);
						list.RemoveAll((BoltLobbyInvite item) => item.Friend.Id == inviteSenderId);
						_boltMatchmaking.LobbyInvitesIncoming = list;
						_boltMatchmaking.IncomingInvitesChangedEvent.Invoke();
					}
					else
					{
						BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(inviteSenderId);
						BoltFriend lobbyInvite = _boltMatchmaking.CurrentLobby.GetLobbyInvite(invitedPlayerId);
						_boltMatchmaking.CurrentLobby.RemoveLobbyInvite(lobbyInvite);
						PlayerRevokedEventArgs arg = new PlayerRevokedEventArgs(lobbyMember, lobbyInvite);
						_boltMatchmaking.PlayerRevokedEvent.Invoke(arg);
					}
				}
			}

			public void OnLobbyOwnerChanged(string lobbyOwnerId)
			{
				LobbyOwnerChangedEventArgs arg;
				lock (Lock)
				{
					_boltMatchmaking.CurrentLobby.LobbyOwnerId = lobbyOwnerId;
					BoltFriend lobbyMember = _boltMatchmaking.CurrentLobby.GetLobbyMember(lobbyOwnerId);
					arg = new LobbyOwnerChangedEventArgs(lobbyMember);
				}
				_boltMatchmaking.LobbyOwnerChangedEvent.Invoke(arg);
			}

			public void OnLobbyNameChanged(string name)
			{
				lock (Lock)
				{
					_boltMatchmaking.CurrentLobby.Name = name;
					LobbyNameChangedEventArgs arg = new LobbyNameChangedEventArgs(name);
					_boltMatchmaking.LobbyNameChangedEvent.Invoke(arg);
				}
			}

			public void OnLobbyTypeChanged(LobbyType protoLobbyType)
			{
				LobbyTypeChangedEventArgs arg;
				lock (Lock)
				{
					BoltLobby.LobbyType lobbyType = EnumMapper<LobbyType, BoltLobby.LobbyType>.ToOriginal(protoLobbyType);
					_boltMatchmaking.CurrentLobby.Type = lobbyType;
					arg = new LobbyTypeChangedEventArgs(lobbyType);
				}
				_boltMatchmaking.LobbyTypeChangedEvent.Invoke(arg);
			}

			public void OnLobbyMaxMembersChanged(byte maxMembers)
			{
				LobbyMaxMembersChangedEventArgs arg;
				lock (Lock)
				{
					_boltMatchmaking.CurrentLobby.MaxMembers = maxMembers;
					arg = new LobbyMaxMembersChangedEventArgs(maxMembers);
				}
				_boltMatchmaking.LobbyMaxMembersChangedEvent.Invoke(arg);
			}

			public void OnLobbyJoinableChanged(bool joinable)
			{
				LobbyJoinableChangedEventArgs arg;
				lock (Lock)
				{
					_boltMatchmaking.CurrentLobby.Joinable = joinable;
					arg = new LobbyJoinableChangedEventArgs(joinable);
				}
				_boltMatchmaking.LobbyJoinableChangedEvent.Invoke(arg);
			}

			public void OnLobbyDataChanged(Dictionary data)
			{
				LobbyDataChangedEventArgs arg;
				lock (Lock)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>(_boltMatchmaking.CurrentLobby.Data);
					foreach (KeyValuePair<string, string> item in data.Content)
					{
						if (string.IsNullOrEmpty(item.Value))
						{
							dictionary.Remove(item.Key);
						}
						else
						{
							dictionary[item.Key] = item.Value;
						}
					}
					_boltMatchmaking.CurrentLobby.Data = dictionary;
					arg = new LobbyDataChangedEventArgs(new Dictionary<string, string>(data.Content));
				}
				_boltMatchmaking.LobbyDataChangedEvent.Invoke(arg);
			}

			public void OnLobbyGameServerChanged(GameServer protoGameServer)
			{
				LobbyGameServerChangedEventArgs arg;
				lock (Lock)
				{
					BoltGameServer gameServer = GameServerMapper.Instance.ToOriginal(protoGameServer);
					_boltMatchmaking.CurrentLobby.GameServer = gameServer;
					arg = new LobbyGameServerChangedEventArgs(gameServer);
				}
				_boltMatchmaking.LobbyGameServerChangedEvent.Invoke(arg);
			}

			public void OnLobbyPhotonGameChanged(PhotonGame protoPhotonGame)
			{
				LobbyPhotonGameChangedEventArgs arg;
				lock (Lock)
				{
					BoltPhotonGame photonGame = PhotonGameMapper.Instance.ToOriginal(protoPhotonGame);
					_boltMatchmaking.CurrentLobby.PhotonGame = photonGame;
					arg = new LobbyPhotonGameChangedEventArgs(photonGame);
				}
				_boltMatchmaking.LobbyPhotonGameChangedEvent.Invoke(arg);
			}

			public void OnLobbyChatMessage(string playerId, string message)
			{
				LobbyChatMessageEventArgs arg;
				lock (Lock)
				{
					long date = DateTime.Now.Ticks / 10000;
					BoltLobbyMessage lobbyMessage = new BoltLobbyMessage(playerId, message, date);
					arg = new LobbyChatMessageEventArgs(lobbyMessage);
				}
				_boltMatchmaking.LobbyChatMessageEvent.Invoke(arg);
			}

			private long GetCurrentMillis()
			{
				return (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
			}

			private List<BoltLobbyInvite> AddInvite(List<BoltLobbyInvite> list, BoltLobbyInvite invite)
			{
				if (list.Exists((BoltLobbyInvite item) => item.Friend.Id == invite.Friend.Id))
				{
					return list;
				}
				return new List<BoltLobbyInvite>(list) { invite };
			}

			private List<BoltLobbyInvite> RemoveInvite(List<BoltLobbyInvite> list, BoltLobbyInvite invite)
			{
				List<BoltLobbyInvite> list2 = new List<BoltLobbyInvite>(list);
				list2.Remove(invite);
				return list2;
			}
		}

		private readonly MatchmakingRemoteService _matchmakingRemoteService;

		private readonly MatchmakingEventListener _remoteEventListener;

		private static readonly object Lock = new object();

		public readonly BoltEvent<NewPlayerJoinedEventArgs> NewPlayerJoinedEvent = new BoltEvent<NewPlayerJoinedEventArgs>();

		public readonly BoltEvent<NewPlayerInvitedEventArgs> NewPlayerInvitedEvent = new BoltEvent<NewPlayerInvitedEventArgs>();

		public readonly BoltEvent<PlayerLeftEventArgs> PlayerLeftEvent = new BoltEvent<PlayerLeftEventArgs>();

		public readonly BoltEvent<PlayerKickedEventArgs> PlayerKickedEvent = new BoltEvent<PlayerKickedEventArgs>();

		public readonly BoltEvent<PlayerRefusedEventArgs> PlayerRefusedEvent = new BoltEvent<PlayerRefusedEventArgs>();

		public readonly BoltEvent<PlayerRevokedEventArgs> PlayerRevokedEvent = new BoltEvent<PlayerRevokedEventArgs>();

		public readonly BoltEvent<LobbyJoinedEventArgs> LobbyJoinedEvent = new BoltEvent<LobbyJoinedEventArgs>();

		public readonly BoltEvent<LobbyLeftEventArgs> LobbyLeftEvent = new BoltEvent<LobbyLeftEventArgs>();

		public readonly BoltEvent IncomingInvitesChangedEvent = new BoltEvent();

		public readonly BoltEvent OutcomingInvitesChangedEvent = new BoltEvent();

		public readonly BoltEvent<ReceivedInviteEventArgs> ReceivedInviteEvent = new BoltEvent<ReceivedInviteEventArgs>();

		public readonly BoltEvent<LobbyNameChangedEventArgs> LobbyNameChangedEvent = new BoltEvent<LobbyNameChangedEventArgs>();

		public readonly BoltEvent<LobbyJoinableChangedEventArgs> LobbyJoinableChangedEvent = new BoltEvent<LobbyJoinableChangedEventArgs>();

		public readonly BoltEvent<LobbyTypeChangedEventArgs> LobbyTypeChangedEvent = new BoltEvent<LobbyTypeChangedEventArgs>();

		public readonly BoltEvent<LobbyOwnerChangedEventArgs> LobbyOwnerChangedEvent = new BoltEvent<LobbyOwnerChangedEventArgs>();

		public readonly BoltEvent<LobbyMaxMembersChangedEventArgs> LobbyMaxMembersChangedEvent = new BoltEvent<LobbyMaxMembersChangedEventArgs>();

		public readonly BoltEvent<LobbyDataChangedEventArgs> LobbyDataChangedEvent = new BoltEvent<LobbyDataChangedEventArgs>();

		public readonly BoltEvent<LobbyGameServerChangedEventArgs> LobbyGameServerChangedEvent = new BoltEvent<LobbyGameServerChangedEventArgs>();

		public readonly BoltEvent<LobbyPhotonGameChangedEventArgs> LobbyPhotonGameChangedEvent = new BoltEvent<LobbyPhotonGameChangedEventArgs>();

		public readonly BoltEvent<LobbyChatMessageEventArgs> LobbyChatMessageEvent = new BoltEvent<LobbyChatMessageEventArgs>();

		private BoltLobby _currentLobby;

		public BoltLobby CurrentLobby
		{
			get
			{
				if (_currentLobby == null)
				{
					throw new OutOfLobbyException();
				}
				return _currentLobby;
			}
		}

		public List<BoltLobbyInvite> LobbyInvitesIncoming { get; internal set; }

		public List<BoltLobbyInvite> LobbyInvitesOutcoming { get; internal set; }

		public BoltMatchmakingService()
		{
			_matchmakingRemoteService = new MatchmakingRemoteService(BoltApi.Instance.ClientService);
			_remoteEventListener = new MatchmakingEventListener(this);
			LobbyInvitesIncoming = new List<BoltLobbyInvite>();
			LobbyInvitesOutcoming = new List<BoltLobbyInvite>();
		}

		internal override void BindEvents()
		{
			BoltApi.Instance.AddEventListener(_remoteEventListener);
		}

		internal override void UnbindEvents()
		{
			BoltApi.Instance.RemoveEventListener(_remoteEventListener);
		}

		internal override void Load()
		{
		}

		internal override void Unload()
		{
			_currentLobby = null;
			LobbyInvitesIncoming = new List<BoltLobbyInvite>();
			LobbyInvitesOutcoming = new List<BoltLobbyInvite>();
		}

		public Task<BoltLobby[]> RequestLobbyList(BoltFilter[] filters)
		{
			return BoltApi.Instance.Async(delegate
			{
				Filter[] filters2 = FilterMapper.Instance.ToProtoList(filters).ToArray();
				Lobby[] source = _matchmakingRemoteService.RequestLobbyList(LobbyDistanceFilter.Default, filters2);
				return LobbyMapper.Instance.ToOriginalArray(source.ToList());
			});
		}

		public Task<BoltLobby> GetLobby(string lobbyId)
		{
			return BoltApi.Instance.Async(delegate
			{
				Lobby lobby = _matchmakingRemoteService.GetLobby(lobbyId);
				BoltLobby boltLobby = LobbyMapper.Instance.ToOriginal(lobby);
				BoltAvatars.Instance.Fill(boltLobby.LobbyMembers);
				BoltAvatars.Instance.Fill(boltLobby.LobbyInvites);
				return boltLobby;
			});
		}

		public Task<BoltLobby> CreateLobby(string name, BoltLobby.LobbyType type, int maxMembers)
		{
			return null;
		}

		public Task<BoltLobby> JoinLobby(string lobbyId)
		{
			return null;
		}

		public Task LeaveLobby()
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task InviteToLobby(string playerId)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task RevokeInvitationToLobby(string playerId)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task RefuseInvitationToLobby(string lobbyId)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task KickFromLobby(string playerId)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyName(string name)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyType(BoltLobby.LobbyType type)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyOwner(string playerId)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyMaxMembers(int maxMembers)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyJoinable(bool isJoinable)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task SetLobbyData(Dictionary<string, string> data)
		{
			Dictionary dict = new Dictionary();
			dict.Content.Add(data);
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task DeleteLobbyData(string[] key)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public Task<BoltPlayer> GetLobbyOwner(string lobbyId)
		{
			return null;
		}

		public Task<BoltPlayer[]> GetLobbyMembers(string lobbyId)
		{
			return BoltApi.Instance.Async(delegate
			{
				return new BoltPlayer[0];
			});
		}

		public Task SendLobbyChatMsg(string msg)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}

		public bool IsLobbyOwner()
		{
			lock (Lock)
			{
				return IsInLobby() && CurrentLobby.LobbyOwner.IsLocal();
			}
		}

		public bool IsInLobby()
		{
			return _currentLobby != null;
		}

		public Task SetLobbyPhotonGame(BoltPhotonGame photonGame)
		{
			return BoltApi.Instance.Async(delegate
			{
			});
		}
	}
}
