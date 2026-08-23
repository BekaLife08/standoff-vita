using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Matchmaking;
using Axlebolt.Bolt.Matchmaking.Events;
using Axlebolt.Bolt.Player;
using Axlebolt.Standoff.Main.Inventory;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public static class BoltMatchmakingExtension
	{
		public const string LobbyOptionsKey = "LobbyOptions";

		public const string MemberBadgeIdKey = "MemberBadgeId";

		public static LobbyOptions GetLobbyOptions(this BoltMatchmakingService service)
		{
			string json = service.CurrentLobby.Data["LobbyOptions"];
			return JsonUtility.FromJson<LobbyOptions>(json);
		}

		public static bool IsLobbyOptionsExists(this BoltMatchmakingService service)
		{
			return service.CurrentLobby.Data.ContainsKey("LobbyOptions");
		}

		public static Task SetLobbyOptions(this BoltMatchmakingService service, LobbyOptions lobbyOptions)
		{
			string value = JsonUtility.ToJson(lobbyOptions);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["LobbyOptions"] = value;
			return service.SetLobbyData(dictionary);
		}

		public static bool IsLobbyOptionsChanged(this LobbyDataChangedEventArgs args)
		{
			return args.LobbyData.ContainsKey("LobbyOptions");
		}

		public static BoltLobbyInvite GetLobbyInvite(this BoltMatchmakingService service, string friendId)
		{
			return service.LobbyInvitesIncoming.FirstOrDefault((BoltLobbyInvite invite) => invite.Friend.Id == friendId);
		}

		public static InventoryItemId GetMemberBadgeId(this BoltLobby lobby, string memberId)
		{
			string badgetIdKey = GetBadgetIdKey(memberId);
			if (lobby.Data.ContainsKey(badgetIdKey))
			{
				return (InventoryItemId)int.Parse(lobby.Data[badgetIdKey]);
			}
			return InventoryItemId.None;
		}

		public static bool IsSomeMemberBadgeIdChanged(this LobbyDataChangedEventArgs args)
		{
			return args.LobbyData.Keys.Any((string key) => key.StartsWith("MemberBadgeId"));
		}

		private static string GetBadgetIdKey(string memberId)
		{
			return "MemberBadgeId" + memberId;
		}

		public static Task SetBadgeId(this BoltMatchmakingService service, InventoryItemId badgeId)
		{
			string id = BoltService<BoltPlayerService>.Instance.Player.Id;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string badgetIdKey = GetBadgetIdKey(id);
			int num = (int)badgeId;
			dictionary[badgetIdKey] = num.ToString();
			return service.SetLobbyData(dictionary);
		}
	}
}
