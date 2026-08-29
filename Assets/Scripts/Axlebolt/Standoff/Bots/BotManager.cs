using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Player;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Bots
{
	public static class BotManager
	{
		private const int FirstBotId = 2;

		private static readonly Dictionary<int, int> SubIds = new Dictionary<int, int>();

		public static void CreateBotPlayers(int count)
		{
			if (!PhotonNetwork.offlineMode)
			{
				return;
			}
			NetworkingPeer networkingPeer = PhotonNetwork.networkingPeer;
			for (int i = 0; i < count; i++)
			{
				int id = FirstBotId + i;
				PhotonPlayer photonPlayer;
				if (!networkingPeer.mActors.TryGetValue(id, out photonPlayer))
				{
					photonPlayer = new PhotonPlayer(false, id, "Bot_" + id);
					Team team = (i % 2 == 0) ? Team.Ct : Team.Tr;
					photonPlayer.SetTeam(team);
					networkingPeer.mActors[id] = photonPlayer;
					SubIds[id] = 1;
				}
				photonPlayer.SetHealth(100);
				photonPlayer.SetArmor(100);
				photonPlayer.SetHelmet(true);
			}
			networkingPeer.mPlayerListCopy = networkingPeer.mActors.Values.ToArray();
			networkingPeer.mOtherPlayerListCopy = networkingPeer.mActors.Values.Where((PhotonPlayer p) => !p.IsLocal).ToArray();
		}

		public static void SpawnBots()
		{
			if (!PhotonNetwork.offlineMode)
			{
				return;
			}
			foreach (PhotonPlayer value in PhotonNetwork.networkingPeer.mActors.Values)
			{
				if (value.ID != PhotonNetwork.player.ID && Singleton<PlayerManager>.Instance.GetController(value.ID) == null)
				{
					SpawnBot(value);
				}
			}
		}

		public static void SpawnBot(PhotonPlayer bot)
		{
			if (!PhotonNetwork.offlineMode)
			{
				return;
			}
			bot.SetHealth(100);
			bot.SetArmor(100);
			bot.SetHelmet(true);
			UnityEngine.Debug.Log("BotManager.SpawnBot: bot " + bot.ID);
			Team team = bot.GetTeam();
			string freeId = Singleton<PlayerManager>.Instance.GetFreeId(team);
			SpawnPoint spawnPoint = GetSpawnPoint(team);
			int viewID = GetFreeViewID(bot.ID);
			Hashtable hashtable = new Hashtable();
			hashtable[(byte)0] = freeId;
			hashtable[(byte)1] = spawnPoint.Position;
			hashtable[(byte)2] = spawnPoint.Rotation;
			hashtable[(byte)6] = PhotonNetwork.ServerTimestamp;
			hashtable[(byte)7] = viewID;
			GameObject gameObject = PhotonNetwork.networkingPeer.DoInstantiate(hashtable, bot, null);
			if (gameObject == null)
			{
				UnityEngine.Debug.LogWarning("BotManager.SpawnBot: DoInstantiate returned null for bot " + bot.ID);
				return;
			}
			CharacterController cc = gameObject.GetComponent<CharacterController>();
			if (cc != null)
			{
				Vector3 pos = gameObject.transform.position;
				RaycastHit hitInfo;
				if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out hitInfo, 100f, 1))
				{
					Vector3 groundPoint = hitInfo.point;
					RaycastHit hitInfo2;
					if (Physics.Raycast(groundPoint + Vector3.down * 0.5f, Vector3.down, out hitInfo2, 500f, 1))
					{
						groundPoint = hitInfo2.point;
					}
					gameObject.transform.position = groundPoint;
				}
				else
				{
					cc.Move(Vector3.down * 2f);
				}
			}
			PlayerController playerController = gameObject.GetComponent<PlayerController>();
			playerController.SetCharacterVisible(true);
			BotAI botAI = gameObject.GetComponent<BotAI>();
			if (botAI == null)
			{
				botAI = gameObject.AddComponent<BotAI>();
			}
			botAI.Init(bot, playerController);
		}

		public static void Respawn(PhotonPlayer bot)
		{
			if (!PhotonNetwork.offlineMode)
			{
				return;
			}
			UnityEngine.Debug.Log("BotManager.Respawn: bot " + bot.ID + " dead=" + bot.IsDead() + " health=" + bot.GetHealth());
			PlayerController controller = Singleton<PlayerManager>.Instance.GetController(bot.ID);
			if (controller != null)
			{
				UnityEngine.Debug.Log("BotManager.Respawn: removing old model viewID=" + controller.PhotonView.viewID);
				PhotonNetwork.networkingPeer.RemoveInstantiatedGO(controller.gameObject, localOnly: true);
			}
			else
			{
				UnityEngine.Debug.Log("BotManager.Respawn: controller is null for bot " + bot.ID);
			}
			SpawnBot(bot);
		}

		private static SpawnPoint GetSpawnPoint(Team team)
		{
			SpawnZone[] array = Object.FindObjectsOfType<SpawnZone>().Where((SpawnZone z) => z.Team == team).ToArray();
			if (array.Length == 0)
			{
				return new SpawnPoint(Vector3.zero, Quaternion.identity);
			}
			return array[Random.Range(0, array.Length)].RandomPoint();
		}

		private static int GetFreeViewID(int ownerId)
		{
			int num;
			if (!SubIds.TryGetValue(ownerId, out num))
			{
				num = 1;
				SubIds[ownerId] = 1;
			}
			for (int i = 1; i < PhotonNetwork.MAX_VIEW_IDS; i++)
			{
				int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS + num;
				num = (num % (PhotonNetwork.MAX_VIEW_IDS - 1)) + 1;
				if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num2))
				{
					SubIds[ownerId] = num;
					return num2;
				}
			}
			return ownerId * PhotonNetwork.MAX_VIEW_IDS + 1;
		}
	}
}
