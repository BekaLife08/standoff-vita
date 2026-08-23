using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Player.Ragdoll.Test
{
	public class RagdollTestController : MonoBehaviour
	{
		[SerializeField]
		[PlayerArms]
		[Header("Counter Terrorist")]
		private string ctArms;

		[SerializeField]
		private PlayerCharacters ctCharacters;

		[SerializeField]
		[PlayerArms]
		[Header("Terrorist")]
		[Space]
		private string trArms;

		[SerializeField]
		private PlayerCharacters trCharacters;

		public InputField inputBoneID;

		public InputField inputForce;

		public Transform director;

		private SpawnPointManager _spawnPointManager;

		private PlayerControls _playerPlayerControls;

		public static RagdollTestController Instance;

		[HideInInspector]
		public float hitImpulse;

		[HideInInspector]
		public BipedMap.Bip targetBone;

		private PlayerController player;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			PhotonNetwork.offlineMode = true;
			Singleton<WeaponManager>.Instance.Init();
			Singleton<PlayerManager>.Instance.Init(ctArms, ctCharacters, trArms, trCharacters, 0);
			PhotonNetwork.PrefabPool = new GamePrefabPool();
			_spawnPointManager = new SpawnPointManager(SpawnZoneType.Random);
			GameObject gameObject = new GameObject();
			gameObject.name = "CONTROLS";
			_playerPlayerControls = PlayerControls.Instance;
			SpawnPlayer();
			InitTargetCharacter();
		}

		public void InitTargetCharacter()
		{
			Singleton<RagdollManager>.Instance.Init(ctCharacters, trCharacters);
		}

		public PlayerController SpawnPlayer()
		{
			player = Singleton<PlayerManager>.Instance.GetFromPool(Singleton<PlayerManager>.Instance.GetFreeId(Team.Ct), _spawnPointManager.GetSpawnPoint(Team.Tr).Position, Quaternion.identity).GetComponent<PlayerController>();
			player.WeaponryController.SetWeapon(Singleton<WeaponManager>.Instance.GetLocal(WeaponId.AWM));
			_playerPlayerControls.PlayerController = player;
			player.GetComponent<CharacterPlayer>().enabled = false;
			player.SetCharacterVisible(isEnabled: true);
			return player;
		}

		public PlayerController InitPlayer()
		{
			if (player != null)
			{
				Singleton<PlayerManager>.Instance.ReturnToPool(player.gameObject);
			}
			return SpawnPlayer();
		}

		public void Hit()
		{
			_playerPlayerControls.PlayerController = null;
			int result = -1;
			int.TryParse(inputBoneID.text, out result);
			float result2 = 0f;
			float.TryParse(inputForce.text, out result2);
			HitData hitData = new HitData();
			hitData.Direction = director.transform.forward;
			player.HitController.HitImmediate(hitData);
			_playerPlayerControls.PlayerController = null;
			player = null;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.K))
			{
				Hit();
			}
		}
	}
}
