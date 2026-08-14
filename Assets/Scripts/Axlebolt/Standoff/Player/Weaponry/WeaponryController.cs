using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Weaponry
{
	public class WeaponryController : BaseController<WeaponrySnapshot, WeaponryControllerCmd>, IAimingEvents, IWeaponryController
	{
		private static readonly Log Log = Log.Create(typeof(WeaponryController));

		private PlayerController _playerController;

		private MecanimController _mecanimController;

		private AimController _aimController;

		private WeaponTakeController _weaponTakeController;

		private bool _isFpsViewDisabled;

		private readonly Dictionary<byte, WeaponController> _weaponSlots = new Dictionary<byte, WeaponController>();

		private byte _currentSlot;

		private IInventoryEvents[] _inventoryEventHandlers;

		private ViewMode _viewMode;

		private bool _weaponsVisible;

		public WeaponController CurrentWeapon
		{
			get;
			private set;
		}

		public KitController CurrentKit
		{
			get;
			private set;
		}

		public float LocalTime
		{
			get;
			private set;
		}

		public float DeltaTime
		{
			get;
			private set;
		}

		public Vector3 WeaponDropPosition
		{
			[CompilerGenerated]
			get
			{
				return _playerController.MainCameraHolder.position;
			}
		}

		public Vector3 WeaponDropDirection
		{
			[CompilerGenerated]
			get
			{
				return _playerController.MainCameraHolder.forward;
			}
		}

		public PhotonView PhotonView
		{
			[CompilerGenerated]
			get
			{
				return _playerController.PhotonView;
			}
		}

		public override void PreInitialize()
		{
			_playerController = this.GetRequireComponent<PlayerController>();
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			LocalTime = _playerController.LocalTime;
			_aimController = _playerController.AimController;
			_aimController.OnSpinePostRotation += OnSpineRotationApplied;
			_mecanimController = _playerController.MecanimController;
			_mecanimController.onAnimatorStateSet += OnAnimatorStateSet;
			_inventoryEventHandlers = GetComponents<IInventoryEvents>();
			_weaponTakeController = base.gameObject.AddComponent<WeaponTakeController>();
			_weaponTakeController.Init(this);
		}

		public override void Initialize()
		{
			_weaponTakeController.enabled = false;
			WeaponController primaryWeapon = Singleton<WeaponManager>.Instance.GetPrimaryWeapon();
			SetWeapon(primaryWeapon);
		}

		public override void OnInstantiated()
		{
			_weaponTakeController.enabled = true;
		}

		public override void OnReturnToPool()
		{
			foreach (KeyValuePair<byte, WeaponController> weaponSlot in _weaponSlots)
			{
				Singleton<WeaponManager>.Instance.Return(weaponSlot.Value);
			}
			_weaponSlots.Clear();
			CurrentWeapon = null;
			_currentSlot = byte.MaxValue;
			if (CurrentKit != null)
			{
				Singleton<WeaponManager>.Instance.Return(CurrentKit);
				CurrentKit = null;
			}
			EnableFpsView();
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			foreach (WeaponController value in _weaponSlots.Values)
			{
				Vector3 localPosition = value.transform.localPosition;
				Quaternion localRotation = value.transform.localRotation;
				value.transform.SetParent(bipedMap.GetBone(BipedMap.Bip.RightHand).transform);
				value.transform.localPosition = localPosition;
				value.transform.localRotation = localRotation;
				value.OnCharacterSkinSet(bipedMap);
			}
		}

		private void OnAnimatorStateSet()
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.OnAnimatorStateSet();
			}
		}

		public override void ExecuteCommands(WeaponryControllerCmd commands, float duration, float time)
		{
			LocalTime = time;
			DeltaTime = duration;
			LocalUpdate();
			if (commands.SlotIndex != -1 && commands.SlotIndex != _currentSlot)
			{
				SwitchWeapon((byte)commands.SlotIndex);
			}
			if (!(CurrentWeapon == null))
			{
				CurrentWeapon.ExecuteCommands(commands.WeaponControllerCmd, duration, time);
				if (commands.ToDrop && CurrentWeapon.IsDroppable())
				{
					Drop(_currentSlot);
				}
				if (CurrentKit != null)
				{
					CurrentKit.ExecuteCommands(commands.WeaponControllerCmd, duration, time);
				}
			}
		}

		public override WeaponrySnapshot GetSnapshot()
		{
			WeaponrySnapshot weaponrySnapshot = new WeaponrySnapshot();
			weaponrySnapshot.time = LocalTime;
			WeaponrySnapshot weaponrySnapshot2 = weaponrySnapshot;
			foreach (KeyValuePair<byte, WeaponController> weaponSlot in _weaponSlots)
			{
				WeaponController value = weaponSlot.Value;
				WeaponrySnapshot.SlotItem slotItem = new WeaponrySnapshot.SlotItem();
				slotItem.SlotIndex = weaponSlot.Key;
				slotItem.WeaponId = (byte)value.WeaponId;
				slotItem.SkinId = (short)value.SkinId;
				WeaponrySnapshot.SlotItem item = slotItem;
				weaponrySnapshot2.Slot.Add(item);
			}
			weaponrySnapshot2.CurrentSlotIndex = _currentSlot;
			if (CurrentWeapon != null)
			{
				weaponrySnapshot2.WeaponSnapshot = CurrentWeapon.GetSnapshot();
				weaponrySnapshot2.WeaponType = CurrentWeapon.WeaponType;
			}
			if (CurrentKit != null)
			{
				weaponrySnapshot2.KitSnapshot = CurrentKit.GetSnapshot();
				weaponrySnapshot2.KitType = CurrentKit.WeaponType;
				weaponrySnapshot2.KitWeaponId = CurrentKit.WeaponId;
			}
			return weaponrySnapshot2;
		}

		public override void SetSnapshot(WeaponrySnapshot snapshot)
		{
			List<byte> list = new List<byte>();
			list.AddRange(_weaponSlots.Keys);
			foreach (WeaponrySnapshot.SlotItem item in snapshot.Slot)
			{
				if (!_weaponSlots.ContainsKey(item.SlotIndex))
				{
					_weaponSlots[item.SlotIndex] = GetWeapon(item.WeaponId, item.SkinId);
					list.Remove(item.SlotIndex);
				}
				else
				{
					WeaponController weaponController = _weaponSlots[item.SlotIndex];
					if (weaponController.WeaponNumId != item.WeaponId || weaponController.SkinNumId != item.SkinId)
					{
						ClearSlot(weaponController.SlotIndex);
						_weaponSlots[item.SlotIndex] = GetWeapon(item.WeaponId, item.SkinId);
					}
					list.Remove(item.SlotIndex);
				}
			}
			foreach (byte item2 in list)
			{
				ClearSlot(item2);
			}
			CurrentWeapon = _weaponSlots[snapshot.CurrentSlotIndex];
			if (CurrentWeapon.HandlingState != 0)
			{
				CurrentWeapon.SetAsDefault(LocalTime);
			}
			foreach (WeaponController value in _weaponSlots.Values)
			{
				if (value != CurrentWeapon && value.HandlingState != WeaponController.HandleState.Secondary)
				{
					value.SetAsSecondary();
				}
			}
			CurrentWeapon.SetSnapshot(snapshot.WeaponSnapshot);
			SetWeaponsVisible(_weaponsVisible);
			_currentSlot = snapshot.CurrentSlotIndex;
			if (snapshot.KitSnapshot == null && CurrentKit != null)
			{
				ClearKit();
			}
			else if (snapshot.KitSnapshot != null)
			{
				if (CurrentKit != null && snapshot.KitWeaponId != CurrentKit.WeaponId)
				{
					ClearKit();
				}
				if (CurrentKit == null)
				{
					CurrentKit = (KitController)Singleton<WeaponManager>.Instance.Get(snapshot.KitWeaponId);
				}
				CurrentKit.SetSnapshot(snapshot.KitSnapshot);
			}
		}

		private void ClearKit()
		{
			Singleton<WeaponManager>.Instance.Return(CurrentKit);
			CurrentKit = null;
		}

		private WeaponController GetWeapon(byte weaponId, short skinId)
		{
			WeaponController weaponController = Singleton<WeaponManager>.Instance.Get((WeaponId)weaponId, (InventoryItemId)skinId);
			weaponController.SetPlayer(_playerController);
			return weaponController;
		}

		private void DisableFpsView()
		{
			_playerController.FpsCameraHolder.SetActive(value: false);
			_isFpsViewDisabled = true;
		}

		private void EnableFpsView()
		{
			_playerController.FpsCameraHolder.SetActive(value: true);
			_isFpsViewDisabled = false;
		}

		private void OnSpineRotationApplied()
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.OnCharacterSpinePostRotation();
			}
		}

		private void LocalUpdate()
		{
			if (CurrentWeapon != null && _isFpsViewDisabled && !_mecanimController.IsStartingWeaponSwitch(CurrentWeapon.WeaponName))
			{
				EnableFpsView();
			}
		}

		public void SetWeapon(WeaponId weaponId)
		{
			WeaponController local = Singleton<WeaponManager>.Instance.GetLocal(weaponId);
			SetWeapon(local);
		}

		public void SetWeapon(WeaponController weapon)
		{
			if (weapon is KitController)
			{
				CurrentKit = (KitController)weapon;
				return;
			}
			byte slotIndex = weapon.SlotIndex;
			if (_weaponSlots.ContainsKey(slotIndex))
			{
				DropImmediately(slotIndex);
			}
			_weaponSlots[slotIndex] = weapon;
			weapon.SetPlayer(_playerController);
			weapon.SetViewMode(_viewMode);
			weapon.SetVisible(_weaponsVisible);
			TakeWeapon(slotIndex);
		}

		private void Drop(byte slotIndex)
		{
			if (!ScenePhotonBehavior<WeaponDropManager>.Instance.InProgress)
			{
				ScenePhotonBehavior<WeaponDropManager>.Instance.Drop(_weaponSlots[slotIndex], WeaponDropDirection);
				ClearSlot(slotIndex);
				if (CurrentWeapon == null)
				{
					SwitchWeapon();
				}
			}
		}

		private void DropImmediately(byte slotIndex)
		{
			if (ScenePhotonBehavior<WeaponDropManager>.IsInitialized() && _weaponSlots[slotIndex].IsDroppable())
			{
				ScenePhotonBehavior<WeaponDropManager>.Instance.DropImmediately(_weaponSlots[slotIndex], WeaponDropDirection);
			}
			ClearSlot(slotIndex);
		}

		public void ClearSlot(WeaponId weaponId)
		{
			KeyValuePair<byte, WeaponController> keyValuePair = _weaponSlots.FirstOrDefault((KeyValuePair<byte, WeaponController> slot) => slot.Value.WeaponId == weaponId);
			if (keyValuePair.Value == null)
			{
				Log.Warning($"RemoveWeaponById slot with weapon {weaponId} not found!");
				return;
			}
			ClearSlot(keyValuePair.Key);
			if (CurrentWeapon == null)
			{
				SwitchWeapon();
			}
		}

		public bool IsSlotFree(byte slotIndex)
		{
			return !_weaponSlots.ContainsKey(slotIndex);
		}

		public bool HasWeapon(WeaponId weaponId)
		{
			return _weaponSlots.Any((KeyValuePair<byte, WeaponController> entry) => entry.Value.WeaponId == weaponId);
		}

		public void DropAllWeapons()
		{
			if (ScenePhotonBehavior<WeaponDropManager>.IsInitialized())
			{
				byte[] array = (from slot in _weaponSlots
					where slot.Value.IsDroppable()
					select slot.Key).ToArray();
				byte[] array2 = array;
				foreach (byte slotIndex in array2)
				{
					DropImmediately(slotIndex);
				}
				if (CurrentWeapon == null)
				{
					SwitchWeapon();
				}
			}
		}

		public WeaponController[] GetWeapons()
		{
			return _weaponSlots.Values.ToArray();
		}

		public void SwitchWeapon(byte slotIndex)
		{
			if (_currentSlot != slotIndex && _weaponSlots.ContainsKey(slotIndex))
			{
				TakeWeapon(slotIndex);
			}
		}

		public void ReswitchCurrentWeapon()
		{
			TakeWeapon(_currentSlot);
		}

		private void TakeWeapon(byte slotIndex)
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.SetAsSecondary();
			}
			CurrentWeapon = _weaponSlots[slotIndex];
			_currentSlot = slotIndex;
			CurrentWeapon.SetAsDefault(LocalTime);
			CurrentWeapon.SetVisible(_weaponsVisible);
			DisableFpsView();
			_mecanimController.SwitchWeapon(CurrentWeapon.WeaponNumId);
			OnWeaponSet(CurrentWeapon);
		}

		private void ClearSlot(byte weaponSlot)
		{
			if (_currentSlot == weaponSlot)
			{
				CurrentWeapon = null;
			}
			Singleton<WeaponManager>.Instance.Return(_weaponSlots[weaponSlot]);
			_weaponSlots.Remove(weaponSlot);
		}

		public void SetWeaponsVisible(bool isVisible)
		{
			_weaponsVisible = isVisible;
			foreach (WeaponController value in _weaponSlots.Values)
			{
				value.SetVisible(isVisible);
			}
		}

		private void OnWeaponSet(WeaponController weapon)
		{
			IInventoryEvents[] inventoryEventHandlers = _inventoryEventHandlers;
			foreach (IInventoryEvents inventoryEvents in inventoryEventHandlers)
			{
				inventoryEvents.OnWeaponSet(weapon);
			}
		}

		public void OnViewModeSet(ViewMode viewMode)
		{
			_viewMode = viewMode;
			foreach (WeaponController value in _weaponSlots.Values)
			{
				value.SetViewMode(viewMode);
			}
		}

		private void SwitchWeapon()
		{
			if (_weaponSlots.ContainsKey(1))
			{
				SwitchWeapon(1);
			}
			else if (_weaponSlots.ContainsKey(2))
			{
				SwitchWeapon(2);
			}
			else
			{
				SwitchWeapon(3);
			}
		}

		Transform IWeaponryController.Transform
		{ get {
			return base.Transform;
		} }
	}
}
