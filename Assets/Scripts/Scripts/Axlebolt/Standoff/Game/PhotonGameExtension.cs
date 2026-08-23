using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public static class PhotonGameExtension
	{
		private class PlayerComparer : IComparer<PhotonPlayer>
		{
			public int Compare(PhotonPlayer x, PhotonPlayer y)
			{
				return (x.GetScore() != y.GetScore()) ? x.GetScore().CompareTo(y.GetScore()) : y.GetDeath().CompareTo(x.GetDeath());
			}
		}

		public const string TeamPlayerProp = "team";

		public const string PingProp = "ping";

		public const string MoneyProp = "money";

		public const string KillsProp = "kills";

		public const string RoundKillsProp = "round_kills";

		public const string AssistsProp = "assists";

		public const string RoundAssistsProp = "round_assists";

		public const string MvpProp = "mvp";

		public const string DeathProp = "death";

		public const string ScoreProp = "score";

		public const string NextLevelVoteProp = "NextLevelVoteView";

		public const string AvatarProp = "avatar";

		public const string BadgeIdProp = "badgeId";

		public const string UidProp = "uid";

		private const float UpdateInterval = 5f;

		public static readonly string[] Properties = new string[7]
		{
			"team",
			"kills",
			"assists",
			"death",
			"score",
			"NextLevelVoteView",
			"ping"
		};

		public static IEnumerator UpdatePing()
		{
			while (true)
			{
				if (!PhotonNetwork.connectedAndReady)
				{
					yield return new WaitForSeconds(5f);
				}
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
				{
					{
						"ping",
						PhotonNetwork.GetPing()
					}
				});
				yield return new WaitForSeconds(5f);
			}
		}

		public static int GetPing(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetValue("ping", out object value))
			{
				return (int)value;
			}
			return 0;
		}

		public static void SetTeam(this PhotonPlayer player, Team team)
		{
			Team team2 = player.GetTeam();
			if (team2 != team)
			{
				player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
				{
					{
						"team",
						(byte)team
					}
				});
			}
		}

		public static Team GetTeam(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("team", out object result))
			{
				return (Team)result;
			}
			return Team.None;
		}

		public static void SetMoney(this PhotonPlayer player, int money)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"money",
					money
				}
			});
		}

		public static int GetMoney(this PhotonPlayer player)
		{
			return player.GetProperty("money", 0);
		}

		public static void AddMoney(this PhotonPlayer player, int money)
		{
			player.SetMoney(player.GetMoney() + money);
		}

		public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
		{
			int score = player.GetScore();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"score",
					score + scoreToAddToCurrent
				}
			});
		}

		public static int GetScore(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("score", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void ResetScore(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"score",
					0
				}
			});
		}

		public static void AddMvp(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"mvp",
					player.GetMvp() + 1
				}
			});
		}

		public static int GetMvp(this PhotonPlayer player)
		{
			return player.GetProperty("mvp", 0);
		}

		public static void AddKill(this PhotonPlayer player)
		{
			int kills = player.GetKills();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"kills",
					kills + 1
				}
			});
		}

		public static int GetKills(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("kills", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void ResetKills(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"kills",
					0
				}
			});
		}

		public static void AddRoundKills(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"round_kills",
					player.GetRoundKills() + 1
				}
			});
		}

		public static int GetRoundKills(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("round_kills", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void ResetRoundKills(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"round_kills",
					0
				}
			});
		}

		public static void AddAssist(this PhotonPlayer player)
		{
			int assists = player.GetAssists();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"assists",
					assists + 1
				}
			});
		}

		public static int GetAssists(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("assists", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void ResetAssists(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"assists",
					0
				}
			});
		}

		public static void AddRoundAssist(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"round_assists",
					player.GetRoundAssists() + 1
				}
			});
		}

		public static int GetRoundAssists(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("round_assists", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void ResetRoundAssists(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"round_assists",
					0
				}
			});
		}

		public static void AddDeath(this PhotonPlayer player)
		{
			int death = player.GetDeath();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"death",
					death + 1
				}
			});
		}

		public static int GetDeath(this PhotonPlayer player)
		{
			return player.GetProperty("death", 0);
		}

		public static void ResetDeath(this PhotonPlayer player)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"death",
					0
				}
			});
		}

		public static void SetNextLevelVote(this PhotonPlayer player, string levelName)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"NextLevelVoteView",
					levelName
				}
			});
		}

		public static string GetNextLevelVote(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("NextLevelVoteView", out object result))
			{
				return (string)result;
			}
			return null;
		}

		public static void SetAvatar(this PhotonPlayer player, byte[] avatar)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"avatar",
					avatar
				}
			});
		}

		public static byte[] GetAvatar(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("avatar", out object result))
			{
				return (byte[])result;
			}
			return null;
		}

		public static void SetBadgeId(this PhotonPlayer player, InventoryItemId badgeId)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"badgeId",
					(short)badgeId
				}
			});
		}

		public static InventoryItemId GetBadgetId(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("badgeId", out object result))
			{
				return (InventoryItemId)result;
			}
			return InventoryItemId.None;
		}

		public static bool HasBadgeId(this PhotonPlayer player)
		{
			return player.GetBadgetId() != InventoryItemId.None;
		}

		public static void SetUid(this PhotonPlayer player, string uid)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"uid",
					uid
				}
			});
		}

		public static string GetUid(this PhotonPlayer player)
		{
			if (player.CustomProperties.TryGetNonNullValue("uid", out object result))
			{
				return (string)result;
			}
			return string.Empty;
		}

		public static PhotonPlayer[] GetByTeam(this PhotonPlayer[] players, Team team, bool excludeCurrentPlayer = false)
		{
			return (from player in players
				where (excludeCurrentPlayer && object.Equals(player, PhotonNetwork.player)) ? false : (player.GetTeam() == team)
				select player).ToArray();
		}

		public static bool IsAllDead(this PhotonPlayer[] players, Team team)
		{
			return !players.Any((PhotonPlayer player) => player.GetTeam() == team && !player.IsDead());
		}

		public static int GetCtPlayersCount(this PhotonPlayer[] players)
		{
			return players.Count((PhotonPlayer player) => player.GetTeam() == Team.Ct);
		}

		public static int GetTrPlayersCount(this PhotonPlayer[] players)
		{
			return players.Count((PhotonPlayer player) => player.GetTeam() == Team.Tr);
		}

		public static PhotonPlayer[] OrderByScore(this PhotonPlayer[] players)
		{
			return players.OrderByDescending((PhotonPlayer player) => player, new PlayerComparer()).ToArray();
		}
	}
}
