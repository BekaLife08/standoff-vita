using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Main.Inventory;
using I2.Loc;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class DefuseKitController : KitController, IProgressWeaponController, IControlAffector
	{
		private float _progress;

		private DefuseKitState _state;

		public override WeaponType WeaponType
		{
			[CompilerGenerated]
			get
			{
				return WeaponType.DefuseKit;
			}
		}

		public DefuseKitParameters DefuseKitParameters
		{
			[CompilerGenerated]
			get
			{
				return (DefuseKitParameters)base.WeaponParameters;
			}
		}

		public override WeaponSnapshot GetSnapshot()
		{
			DefuseKitSnapshot defuseKitSnapshot = new DefuseKitSnapshot();
			defuseKitSnapshot.Progress = _progress;
			defuseKitSnapshot.State = _state;
			return defuseKitSnapshot;
		}

		internal override void Initialize(InventoryItemId skinId)
		{
			base.Initialize(skinId);
			_progress = 0f;
			_state = DefuseKitState.None;
		}

		public override void ExecuteCommands(WeaponControllerCmd commands, float duration, float time)
		{
			base.ExecuteCommands(commands, duration, time);
			if (commands.ToAction)
			{
				if (_state == DefuseKitState.None)
				{
					if (ScenePhotonBehavior<BombManager>.Instance.IsSapperMe())
					{
						_state = DefuseKitState.Defusing;
						_progress = 0f;
					}
					else
					{
						ScenePhotonBehavior<BombManager>.Instance.StartDefuse(DefuseKitParameters.DefuseDistance, DefuseKitParameters.DefuseDuration);
					}
				}
				else if (ScenePhotonBehavior<BombManager>.Instance.CanDefuse(DefuseKitParameters.DefuseDistance) && ScenePhotonBehavior<BombManager>.Instance.IsSapperMe())
				{
					_progress = (float)(PhotonNetwork.time - ScenePhotonBehavior<BombManager>.Instance.DefuseStartTime) / DefuseKitParameters.DefuseDuration;
				}
				else
				{
					_state = DefuseKitState.None;
				}
			}
			else if (_state == DefuseKitState.Defusing)
			{
				ScenePhotonBehavior<BombManager>.Instance.BreakDefuse();
				_state = DefuseKitState.None;
			}
		}

		public override bool CanPerformAction()
		{
			return ScenePhotonBehavior<BombManager>.Instance.CanDefuse(DefuseKitParameters.DefuseDistance);
		}

		public bool HasProgress()
		{
			return _state == DefuseKitState.Defusing;
		}

		public float GetProgress()
		{
			return 1f - _progress;
		}

		public float GetProgressTime()
		{
			return (float)((double)DefuseKitParameters.DefuseDuration - (PhotonNetwork.time - ScenePhotonBehavior<BombManager>.Instance.DefuseStartTime));
		}

		public string GetProgressDisplayName()
		{
			return ScriptLocalization.DefuseKit.DefusingProgressName;
		}

		public bool IsMovementLocked()
		{
			return _state == DefuseKitState.Defusing;
		}

		public bool IsFiringLocked()
		{
			return _state == DefuseKitState.Defusing;
		}

		public bool IsDropLocked()
		{
			return _state == DefuseKitState.Defusing;
		}
	}
}
