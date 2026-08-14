using Axlebolt.Bolt;
using Axlebolt.Bolt.Stats;
using Axlebolt.Standoff.Bolt;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Bolt
{
	public class PlayerStatsCollector : MonoBehaviour
	{
		private BoltPlayerStats _stats;

		private string _gameMode;

		private void Awake()
		{
			_stats = BoltService<BoltPlayerStatsService>.Instance.Stats;
			GameController.Instance.GameInitEvent.AddListener(OnGameInit);
			GameController.Instance.GameExitEvent.AddListener(OnGameExit);
		}

		public void OnGameInit(GameInitEventArgs initEventArgs)
		{
			_gameMode = initEventArgs.GameMode;
			Singleton<HitManager>.Instance.HitEvent.AddListener(OnHit);
			GameController.Instance.PlayerAssistEvent.AddListener(OnPlayerAssist);
			GameController.Instance.PlayerKilledEvent.AddListener(OnPlayerKilled);
			GameController.Instance.PlayerWasKilledEvent.AddListener(OnPlayerWasKilled);
			Singleton<WeaponManager>.Instance.ShootEvent.AddListener(OnShoot);
		}

		public void OnShoot(WeaponController weaponController)
		{
			_stats.IncShots(_gameMode);
			_stats.IncGunShots(_gameMode, weaponController.WeaponParameters.Id);
		}

		private void OnHit(HitEventArgs e)
		{
			if (e.Shooter.IsLocal && e.Weapon is GunParameters)
			{
				_stats.IncHits(_gameMode);
				_stats.IncGunHits(_gameMode, e.Weapon.Id);
				_stats.AddDamage(_gameMode, e.Damage);
				_stats.AddGunDamage(_gameMode, e.Weapon.Id, e.Damage);
			}
		}

		private void OnPlayerAssist(HitEventArgs e)
		{
			if (e.Weapon is GunParameters)
			{
				_stats.IncAssists(_gameMode);
			}
		}

		private void OnPlayerKilled(HitEventArgs e)
		{
			if (e.Weapon is GunParameters)
			{
				_stats.IncKills(_gameMode);
				_stats.IncGunKills(_gameMode, e.Weapon.Id);
				if (e.Headshot)
				{
					_stats.IncHeadshots(_gameMode);
					_stats.IncGunHeadshots(_gameMode, e.Weapon.Id);
				}
			}
		}

		private void OnPlayerWasKilled(HitEventArgs e)
		{
			_stats.IncDeaths(_gameMode);
		}

		public void OnGameExit(GameExitEventArgs eventArgs)
		{
			_stats.IncGamesPlayed(_gameMode);
		}

		public void FlushChanges()
		{
			BoltService<BoltPlayerStatsService>.Instance.StoreStats().Wait();
		}
	}
}
