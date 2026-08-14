using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Bolt.Friends;
using Axlebolt.Bolt.Player;

namespace Axlebolt.Bolt.Matchmaking
{
	public class BoltLobby
	{
		public enum LobbyType
		{
			Private = 0,
			FriendsOnly = 1,
			Public = 2,
			Invisible = 3
		}

		public string Id { get; internal set; }

		public string LobbyOwnerId
		{
			get
			{
				return LobbyOwner.Id;
			}
			internal set
			{
				LobbyOwner = LobbyMembers.First((BoltFriend bf) => bf.Id == value);
			}
		}

		public BoltFriend LobbyOwner { get; private set; }

		public string Name { get; internal set; }

		public LobbyType Type { get; internal set; }

		public bool Joinable { get; internal set; }

		public int MaxMembers { get; internal set; }

		public IDictionary<string, string> Data { get; internal set; }

		public BoltFriend[] LobbyMembers { get; private set; }

		public BoltFriend[] LobbyInvites { get; private set; }

		public BoltGameServer GameServer { get; internal set; }

		public BoltPhotonGame PhotonGame { get; internal set; }

		public BoltLobby(string id, string lobbyOwnerId, string name, LobbyType type, bool joinable, int maxMembers, ConcurrentDictionary<string, string> data, List<BoltFriend> lobbyMembers, List<BoltFriend> lobbyInvites, BoltGameServer gameServer, BoltPhotonGame photonGame)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (lobbyMembers == null)
			{
				throw new ArgumentNullException("lobbyMembers");
			}
			if (lobbyInvites == null)
			{
				throw new ArgumentNullException("lobbyInvites");
			}
			Id = id;
			Name = name;
			Type = type;
			Joinable = joinable;
			MaxMembers = maxMembers;
			Data = data;
			LobbyMembers = lobbyMembers.ToArray();
			LobbyInvites = lobbyInvites.ToArray();
			LobbyOwnerId = lobbyOwnerId;
			GameServer = gameServer;
			PhotonGame = photonGame;
		}

		public bool IsLobbyMember(BoltFriend friend)
		{
			return LobbyMembers.Any((BoltFriend member) => member.Id == friend.Id);
		}

		public bool IsMemberInvited(BoltFriend friend)
		{
			return LobbyInvites.Any((BoltFriend invite) => invite.Id == friend.Id);
		}

		public BoltFriend GetLobbyMember(string id)
		{
			return LobbyMembers.First((BoltFriend member) => member.Id == id);
		}

		public BoltFriend GetLobbyInvite(string id)
		{
			return LobbyInvites.First((BoltFriend invite) => invite.Id == id);
		}

		internal void AddLobbyMember(BoltFriend member)
		{
			if (!IsLobbyMember(member))
			{
				LobbyMembers = new List<BoltFriend>(LobbyMembers) { member }.ToArray();
			}
		}

		internal void RemoveLobbyMember(BoltFriend member)
		{
			if (IsLobbyMember(member))
			{
				List<BoltFriend> list = new List<BoltFriend>(LobbyMembers);
				list.Remove(member);
				LobbyMembers = list.ToArray();
			}
		}

		internal void AddLobbyInvite(BoltFriend member)
		{
			if (!IsMemberInvited(member))
			{
				LobbyInvites = new List<BoltFriend>(LobbyInvites) { member }.ToArray();
			}
		}

		internal void RemoveLobbyInvite(BoltFriend member)
		{
			if (IsMemberInvited(member))
			{
				List<BoltFriend> list = new List<BoltFriend>(LobbyInvites);
				list.Remove(member);
				LobbyInvites = list.ToArray();
			}
		}
	}
}
