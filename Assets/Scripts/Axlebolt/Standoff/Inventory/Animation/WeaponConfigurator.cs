using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[ExecuteInEditMode]
	public class WeaponConfigurator : MonoBehaviour
	{
		[SerializeField]
		[Space]
		[Header("Counter Terrorist")]
		[PlayerArms]
		private string _ctArms;

		[SerializeField]
		private PlayerCharacters _ctCharacters;

		public WeaponId weapon;

		private PlayerController _playerController;

		private WeaponController _weaponController;

		private WeaponAnimationTool _weaponAnimationTool;

		private void Init()
		{
			Singleton<InventoryManager>.Instance.Init();
			Singleton<WeaponManager>.Instance.Init();
			Singleton<PlayerManager>.Instance.Init(_ctArms, _ctCharacters, _ctArms, _ctCharacters, 0);
		}

		public void CreateCharacter()
		{
			Init();
			_playerController = Singleton<PlayerManager>.Instance.GetFromPool(Singleton<PlayerManager>.Instance.GetFreeId(Team.Ct), new Vector3(0f, 0f, 2f), Quaternion.identity).GetComponent<PlayerController>();
			_playerController.SetTPSView();
			_playerController.SetCharacterVisible(isEnabled: true);
			_weaponController = Singleton<WeaponManager>.Instance.GetLocal(weapon);
			_playerController.WeaponryController.SetWeapon(_weaponController);
			_playerController.WeaponryController.CurrentWeapon.AnimationController.SetActive(isActive: true);
			_weaponAnimationTool = base.gameObject.AddComponent<WeaponAnimationTool>();
			_weaponAnimationTool.characterAnimator = _playerController.MecanimController.animator;
			_weaponAnimationTool.WeaponAnimationParameters = _weaponController.AnimationParameters;
			_weaponAnimationTool.playerController = _playerController;
			_weaponAnimationTool.weaponAnimationController = _weaponController.AnimationController;
		}
	}
}
