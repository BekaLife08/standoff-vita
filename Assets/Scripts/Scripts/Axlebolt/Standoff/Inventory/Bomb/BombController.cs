using Axlebolt.Common.States;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class BombController : WeaponController, IControlAffector
	{
		private static readonly Log Log = Log.Create("BombController");

		private float _progress;

		private readonly StateSimple<BombState> _state = new StateSimple<BombState>();

		private readonly float _switchAnimConfirmDuration = 0.1f;

		private BombMap _bombMap;

		public override WeaponType WeaponType
		{
			[CompilerGenerated]
			get
			{
				return WeaponType.Bomb;
			}
		}

		public BombParameters BombParameters
		{
			[CompilerGenerated]
			get
			{
				return (BombParameters)base.WeaponParameters;
			}
		}

		internal override void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			base.PreInitialize(weaponParameters, animationParameters);
			_bombMap = this.GetRequireComponent<BombMap>();
		}

		internal override void Initialize(InventoryItemId skinId)
		{
			base.Initialize(skinId);
			_state.SetState(BombState.None, base.LocalTime);
			_progress = 0f;
		}

		public override WeaponSnapshot GetSnapshot()
		{
			BombSnapshot bombSnapshot = new BombSnapshot();
			bombSnapshot.Progress = _progress;
			bombSnapshot.State = _state.curState;
			return bombSnapshot;
		}

		public override void SetSnapshot(WeaponSnapshot snapshot)
		{
			BombSnapshot bombSnapshot = (BombSnapshot)snapshot;
			_progress = bombSnapshot.Progress;
			_state.curState = bombSnapshot.State;
		}

		public override void SetPlayer(PlayerController playerController)
		{
			base.SetPlayer(playerController);
			base.transform.SetParent(playerController.BipedMap.RightHand);
		}

		public override void SetAsDefault(float time)
		{
			base.SetAsDefault(time);
			_state.SetState(BombState.TryToSwitch, base.LocalTime);
			base.LodGroup.CombineMesh();
			base.transform.SetParent(PlayerController.BipedMap.RightHand);
			AnimationController.SetActive(isActive: true);
		}

		public override void SetAsSecondary()
		{
			base.gameObject.SetActive(value: true);
			base.HandlingState = HandleState.Secondary;
			AnimationController.SetActive(isActive: false);
			base.LodGroup.CombineMesh();
			base.transform.SetParent(PlayerController.BipedMap.Spine2);
			base.transform.localPosition = BombParameters.Spine2Offset.pos;
			base.transform.localEulerAngles = BombParameters.Spine2Offset.rot;
		}

		internal override void SetViewMode(ViewMode viewMode)
		{
			base.SetViewMode(viewMode);
			switch (viewMode)
			{
			case ViewMode.FPS:
				_bombMap.SetLayer(8);
				break;
			case ViewMode.TPS:
				_bombMap.SetLayer(0);
				break;
			}
		}

		public override void ExecuteCommands(WeaponControllerCmd commands, float duration, float time)
		{
			base.ExecuteCommands(commands, duration, time);
			LocalUpdate();
			if (commands.ToFire)
			{
				if (_state.curState == BombState.Ready)
				{
					if (ScenePhotonBehavior<BombManager>.Instance.CanPlantBomb(this))
					{
						StartPlanting();
					}
				}
				else if (_state.curState == BombState.Planting)
				{
					_progress += duration / BombParameters.PlantDuration;
					if (_progress >= 1f)
					{
						PlantBomb();
					}
				}
			}
			else if (_state.curState == BombState.Planting)
			{
				CancelPlanting();
			}
		}

		private void StartPlanting()
		{
			_state.SetState(BombState.Planting, base.LocalTime);
			MecanimController.PlantBomb();
			_progress = 0f;
			base.LodGroup.SplitMesh();
		}

		private void CancelPlanting()
		{
			_state.SetState(BombState.TryToSwitch, base.LocalTime);
			base.LodGroup.CombineMesh();
		}

		private void PlantBomb()
		{
			if (Physics.Raycast(base.Transform.position, Vector3.down, out RaycastHit hitInfo, 2f, 1))
			{
				BombManager instance = ScenePhotonBehavior<BombManager>.Instance;
				Vector3 point = hitInfo.point;
				Vector3 localEulerAngles = PlayerController.Transform.localEulerAngles;
				instance.PlantBomb(point, localEulerAngles.y);
				PlayerController.WeaponryController.ClearSlot(WeaponId.Bomb);
			}
			else
			{
				Log.Error("Can't plant bomb, invalid place!");
			}
		}

		private void StateControl()
		{
			if (_state.curState == BombState.TryToSwitch)
			{
				_state.SetState(BombState.Switching, base.LocalTime);
			}
			else if (_state.curState == BombState.Switching)
			{
				if (base.LocalTime - _state.timeSwitched > BombParameters.TakeDuration)
				{
					_state.SetState(BombState.Ready, base.LocalTime);
				}
				else if (base.LocalTime - _state.timeSwitched < _switchAnimConfirmDuration && !MecanimController.IsSwitchingWeapon(base.WeaponName))
				{
					MecanimController.SwitchWeapon(base.WeaponNumId);
				}
			}
		}

		private void LocalUpdate()
		{
			StateControl();
		}

		public bool IsMovementLocked()
		{
			return _state.curState == BombState.Planting;
		}

		public bool IsFiringLocked()
		{
			return false;
		}

		public bool IsDropLocked()
		{
			return _state.curState == BombState.Planting;
		}
	}
}
