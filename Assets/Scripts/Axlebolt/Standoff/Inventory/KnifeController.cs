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
		private int _primaryHitCount;
		private float _lastPrimaryHitTime;
		private const float StaminaResetTime = 1.0f;

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
			Damage dmg;
			int backDmg;
			if (isSecondary)
			{
				dmg = CreateDamage(55, 55, 55, 55);
				backDmg = 65;
			}
			else
			{
				bool isFirstHit = (base.LocalTime - _lastPrimaryHitTime > StaminaResetTime) || _primaryHitCount == 0;
				if (isFirstHit)
				{
					_primaryHitCount = 1;
					dmg = CreateDamage(34, 34, 34, 34);
					backDmg = 40;
				}
				else
				{
					dmg = CreateDamage(21, 21, 21, 21);
					backDmg = 28;
				}
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
			float interval = isSecondary ? 1.0f : 0.4f;
			if (!(base.LocalTime - _lastHitTime < interval))
			{
				_lastHitTime = base.LocalTime;
				if (!isSecondary)
				{
					_primaryHitCount++;
					_lastPrimaryHitTime = base.LocalTime;
				}
				else
				{
					_primaryHitCount = 0;
				}
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
