using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Test
{
	public class BombTesting : LevelTesting, IBombListener
	{
		protected override void RegisterManagers(GamePrefabPool pool)
		{
			base.RegisterManagers(pool);
			pool.Register<BombManager>();
		}

		protected override IEnumerator InitManagers()
		{
			yield return base.InitManagers();
			yield return ScenePhotonBehavior<BombManager>.Init(null);
		}

		protected override void OnSpawned(PlayerController playerController)
		{
			base.OnSpawned(playerController);
			ScenePhotonBehavior<BombManager>.Instance.SetPlayer(playerController.gameObject);
			if (!ScenePhotonBehavior<BombManager>.Instance.IsBomberInitialized())
			{
				ScenePhotonBehavior<BombManager>.Instance.SetInitBomberIsMe();
			}
			playerController.WeaponryController.SetWeapon(WeaponId.DefuseKit);
			ScenePhotonBehavior<BombManager>.Instance.SetBombListener(this);
		}

		public void OnBombPlantError()
		{
		}

		public void OnBombPlanting()
		{
		}

		public void OnBombPlanted(PhotonPlayer bomber, float detonationDuration)
		{
		}

		public void OnBombBeepSignal()
		{
		}

		public void OnDetonated(PhotonPlayer bomber)
		{
			StartCoroutine(Reinit(3f));
		}

		public void OnDefusingBomb()
		{
		}

		private IEnumerator Reinit(float time)
		{
			yield return new WaitForSeconds(time);
			ScenePhotonBehavior<BombManager>.Instance.Clear();
			ScenePhotonBehavior<WeaponDropManager>.Instance.Clear();
			SpawnPlayer();
		}

		public void OnDefused(PhotonPlayer sapper, float time)
		{
			StartCoroutine(Reinit(3f));
		}
	}
}
