using Axlebolt.Common.States;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Inventory.HitHandling;
using Axlebolt.Standoff.Player;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class KnifeController : WeaponController
	{
		public enum StateType
		{
			Switching = 2,
			TryToSwitch,
			Ready
		}

		public const float HitRadius = 1.2f;

		private KnifeParameters _parameters;

		private StateSimple<StateType> _state = new StateSimple<StateType>();

		private readonly float _switchAnimConfirmDuration = 0.5f;

		private float _lastHitTime;

		public override WeaponType WeaponType
		{
			[CompilerGenerated]
			get
			{
				return WeaponType.Knife;
			}
		}

		internal override void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			KnifeParameters knifeParameters = weaponParameters as KnifeParameters;
			if (!(knifeParameters != null))
			{
				throw new ArgumentException("Invalid weaponParameters, expected Weapon actual is " + weaponParameters);
			}
			base.PreInitialize(weaponParameters, animationParameters);
			_parameters = knifeParameters;
			AnimationController.PreInitialize(this);
		}

		public override WeaponSnapshot GetSnapshot()
		{
			return new KnifeSnapshot();
		}

		public override void SetSnapshot(WeaponSnapshot parameters)
		{
		}

		public override void ExecuteCommands(WeaponControllerCmd commands, float duration, float time)
		{
			base.ExecuteCommands(commands, duration, time);
			LocalUpdate();
			if (commands.ToFire)
			{
				Hit(false);
			}
			else if (commands.ToAim || Input.GetMouseButton(1))
			{
				Hit(true);
			}
		}

		public override void SetPlayer(PlayerController playerController)
		{
			base.SetPlayer(playerController);
			base.transform.SetParent(playerController.BipedMap.RightHand);
			_lastHitTime = 0f;
		}

		public override void SetAsDefault(float time)
		{
			base.SetAsDefault(time);
			_state.SetState(StateType.TryToSwitch, base.LocalTime);
		}

		private void CastHit(Vector3 startPosition, Vector3 direction, bool isLocal, bool isSecondary)
		{
			Damage dmg = isSecondary ? _parameters.SecondaryDamage : _parameters.Damage;
			int backDmg = isSecondary ? _parameters.SecondaryBackDamage : _parameters.BackDamage;
			// Fallback if secondary not configured in asset
			if (isSecondary && (dmg == null || (dmg.HeadDamage == 0 && dmg.ChestAndArmsDamage == 0)))
			{
				dmg = CreateDamage(60, 60, 60, 60);
				backDmg = 65;
			}
			else if (!isSecondary && (dmg == null || (dmg.HeadDamage == 0 && dmg.ChestAndArmsDamage == 0)))
			{
				dmg = CreateDamage(25, 25, 25, 25);
				backDmg = 35;
			}
			KnifeHitParameters parameters = KnifeHitParameters.Create(base.WeaponId, base.SkinId, 0.1f, _parameters.ArmorPenetration, _parameters.HitImpulse, dmg, backDmg);
			List<HitCasterResult> list = new List<HitCasterResult>();
			HitCaster.CastHit(startPosition, direction, 1.2f, parameters, isLocal, list);
			if (list.Count > 0)
			{
				HitData hitData = new HitData();
				hitData.Direction = base.transform.position;
				hitData.WeaponId = base.WeaponId;
				hitData.SkinId = base.SkinId;
				hitData.Hits = new BulletHitData[1]
				{
					list[0].BulletHitData
				};
				HitData hitData2 = hitData;
				PlayerHitController playerHitController = list[0].PlayerHitController;
				playerHitController.Hit(hitData2);
			}
		}

		private void Hit(bool isSecondary)
		{
			float interval = isSecondary ? _parameters.SecondaryHitInterval : _parameters.HitInterval;
			if (interval <= 0f) interval = isSecondary ? 0.9f : 0.35f;
			if (!(base.LocalTime - _lastHitTime < interval))
			{
				_lastHitTime = base.LocalTime;
				MecanimController.SetShootType(isSecondary ? 1 : UnityEngine.Random.Range(0, 2));
				MecanimController.SetShooting();
				Vector3 position = PlayerController.MainCameraHolder.transform.position;
				Vector3 forward = PlayerController.MainCameraHolder.transform.forward;
				CastHit(position, forward, isLocal: true, isSecondary: isSecondary);
			}
		}

		private Damage CreateDamage(int head, int chest, int stomach, int legs)
		{
			return Damage.Create(head, chest, stomach, legs);
		}

		private void StateControl()
		{
			if (_state.curState == StateType.TryToSwitch)
			{
				_state.SetState(StateType.Switching, base.LocalTime);
			}
			else if (_state.curState == StateType.Switching)
			{
				if (base.LocalTime - _state.timeSwitched > _parameters.TakeDuration)
				{
					_state.SetState(StateType.Ready, base.LocalTime);
				}
				else if (base.LocalTime - _state.timeSwitched < _switchAnimConfirmDuration && !MecanimController.IsSwitchingWeapon(base.WeaponName))
				{
					MecanimController.SwitchWeapon(base.WeaponNumId);
				}
			}
		}

		public override bool IsDroppable()
		{
			return false;
		}

		private void LocalUpdate()
		{
			StateControl();
		}
	}
}
