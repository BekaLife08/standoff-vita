using Axlebolt.Standoff.Core;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Linq;

namespace Axlebolt.Standoff.Game
{
	public static class PhotonRoomExtension
	{
		public const string TimeProp = "Time";

		public const string RoundProp = "Round";

		public const string RoundStartTimeProp = "RoundStartTime";

		public const string GameModeProp = "C0";

		public const string LevelNameProp = "C1";

		public const string GameStateProp = "C2";

		public const string SandboxProp = "C3";

		public const string ScoreProp = "Score";

		public const string ConsecutiveLosses = "CoLosses";

		public const string NextLevelsProp = "NextLevels";

		public const string FinalNextLevelVotesProp = "FinalNextLevelVotes";

		public const string NextLevelProp = "NextLevel";

		public const string FinalPlayers = "FinalPlayers";

		public const string WinPlayers = "WinPlayers";

		public const string FinalWinTeam = "FinalWinTeam";

		public const string WinTeam = "WinTeam";

		public static void SetTime(this ExitGames.Client.Photon.Hashtable hashtable, double time)
		{
			hashtable["Time"] = time;
		}

		public static double GetTime(this Room room)
		{
			return (double)room.CustomProperties["Time"];
		}

		public static void SetRound(this ExitGames.Client.Photon.Hashtable hashtable, int round)
		{
			hashtable["Round"] = round;
		}

		public static int GetRound(this Room room)
		{
			if (room.CustomProperties.ContainsKey("Round"))
			{
				return (int)room.CustomProperties["Round"];
			}
			return 0;
		}

		public static void SetRoundStartTime(this ExitGames.Client.Photon.Hashtable hashtable, double time)
		{
			hashtable["RoundStartTime"] = time;
		}

		public static double GetRoundStartTime(this Room room)
		{
			return (double)room.CustomProperties["RoundStartTime"];
		}

		public static void SetGameStateId(this ExitGames.Client.Photon.Hashtable hashtable, byte gameState)
		{
			hashtable["C2"] = gameState;
		}

		public static byte GetGameStateId(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("C2", out object result))
			{
				return (byte)result;
			}
			return 0;
		}

		public static string GetLevelName(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("C1", out object result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("LevelName is null");
		}

		public static string GetGameModeName(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("C0", out object result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("GameMode is null");
		}

		public static void AddScore(this ExitGames.Client.Photon.Hashtable hashtable, Team team)
		{
			int score = PhotonNetwork.room.GetScore(team);
			hashtable[team + "Score"] = score + 1;
		}

		public static int GetScore(this Room room, Team team)
		{
			if (room.CustomProperties.TryGetNonNullValue(team + "Score", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void AddConsecutiveLosses(this ExitGames.Client.Photon.Hashtable hashtable, Team team)
		{
			int consecutiveLosses = PhotonNetwork.room.GetConsecutiveLosses(team);
			hashtable[team + "CoLosses"] = consecutiveLosses + 1;
			hashtable[team.GetOtherTeam() + "CoLosses"] = 0;
		}

		public static int GetConsecutiveLosses(this Room room, Team team)
		{
			if (room.CustomProperties.TryGetNonNullValue(team + "CoLosses", out object result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void SetVotingLevels(this Room room, string[] levelNames)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			for (int i = 0; i < levelNames.Length; i++)
			{
				hashtable[i] = levelNames[i];
			}
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"NextLevels",
					hashtable
				}
			});
		}

		public static string[] GetVotingLevels(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("NextLevels", out object result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				string[] array = new string[hashtable.Count];
				for (int i = 0; i < hashtable.Count; i++)
				{
					array[i] = (string)hashtable[i];
				}
				return array;
			}
			throw new InvalidOperationException("Can't retrive vote levels, is empty");
		}

		public static void SetFinalLevelVotes(this Room room, int[] votes)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			for (int i = 0; i < votes.Length; i++)
			{
				hashtable[i] = votes[i];
			}
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"FinalNextLevelVotes",
					hashtable
				}
			});
		}

		public static int[] GetFinalLevelVotes(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("FinalNextLevelVotes", out object result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				int[] array = new int[hashtable.Count];
				{
					foreach (DictionaryEntry item in hashtable)
					{
						array[(int)item.Key] = (int)item.Value;
					}
					return array;
				}
			}
			throw new InvalidOperationException("Can't retrive final next level votes, is empty");
		}

		public static void SetNextLevel(this Room room, string nextLevel)
		{
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
			{
				{
					"NextLevel",
					nextLevel
				}
			});
		}

		public static string GetNextLevel(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("NextLevel", out object result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("Can't retrive next level, is empty");
		}

		public static string GetNextLevelNullable(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("NextLevel", out object result))
			{
				return (string)result;
			}
			return null;
		}

		public static void SetFinalPlayers(this ExitGames.Client.Photon.Hashtable roomProperties, PhotonPlayer[] players)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			foreach (PhotonPlayer photonPlayer in players)
			{
				ExitGames.Client.Photon.Hashtable allProperties = photonPlayer.AllProperties;
				hashtable[photonPlayer.ID] = allProperties;
			}
			roomProperties["FinalPlayers"] = hashtable;
		}

		public static PhotonPlayer[] GetFinalPlayers(this Room room)
		{
			if (room.CustomProperties.TryGetNonNullValue("FinalPlayers", out object result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				PhotonPlayer[] array = new PhotonPlayer[hashtable.Count];
				int num = 0;
				{
					foreach (DictionaryEntry item in hashtable)
					{
						int actorID = (int)item.Key;
						ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)item.Value;
						array[num++] = new PhotonPlayer(false, actorID, properties);
					}
					return array;
				}
			}
			throw new InvalidOperationException("Can't retrive final players, is empty");
		}

		public static void SetWinPlayers(this ExitGames.Client.Photon.Hashtable hashtable, WinPlayers winPlayers)
		{
			int[] array2 = (int[])(hashtable["WinPlayers"] = (from player in winPlayers.GetAsArray()
				select player.ID).ToArray());
		}

		public static WinPlayers GetWinPlayers(this Room room)
		{
			if (room.CustomProperties.TryGetValue("WinPlayers", out object value))
			{
				PhotonPlayer[] finalPlayers = room.GetFinalPlayers();
				int[] source = (int[])value;
				PhotonPlayer[] players = (from id in source
					select finalPlayers.First((PhotonPlayer p) => p.ID == id)).ToArray();
				return new WinPlayers(players);
			}
			throw new InvalidOperationException("Can't retrive WinPlayers, is empty");
		}

		public static void SetFinalWinTeam(this ExitGames.Client.Photon.Hashtable hashtable, FinalWinTeam finalWinTeam)
		{
			hashtable["FinalWinTeam"] = new ExitGames.Client.Photon.Hashtable
			{
				["isDraw"] = finalWinTeam.IsDraw,
				["isGiveUp"] = finalWinTeam.IsGiveUp,
				["team"] = finalWinTeam.Team
			};
		}

		public static FinalWinTeam GetFinalWinTeam(this Room room)
		{
			if (room.CustomProperties.TryGetValue("FinalWinTeam", out object value))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)value;
				return new FinalWinTeam((bool)hashtable["isDraw"], (bool)hashtable["isGiveUp"], (Team)hashtable["team"]);
			}
			throw new InvalidOperationException("Can't retrive FinalWinTeam, is empty");
		}

		public static void SetWinTeam(this ExitGames.Client.Photon.Hashtable hashtable, WinTeam winTeam)
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2["team"] = winTeam.Team;
			ExitGames.Client.Photon.Hashtable hashtable3 = hashtable2;
			int? num = winTeam.MvpPlayer?.ID;
			hashtable3["mvpPlayer"] = (num.HasValue ? num.Value : 0);
			hashtable2["mvpCode"] = winTeam.MvpCode;
			ExitGames.Client.Photon.Hashtable hashtable5 = (ExitGames.Client.Photon.Hashtable)(hashtable["WinTeam"] = hashtable2);
		}

		public static WinTeam GetWinTeam(this Room room)
		{
			if (room.CustomProperties.TryGetValue("WinTeam", out object value))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)value;
				return new WinTeam((Team)hashtable["team"], PhotonPlayer.Find((int)hashtable["mvpPlayer"]), (byte)hashtable["mvpCode"]);
			}
			throw new InvalidOperationException("Can't retrive WinTeam, is empty");
		}
	}
}
