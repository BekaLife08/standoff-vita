using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Inventory;
using Axlebolt.Standoff.Settings.Video;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponMaterialManager
	{
		private static readonly Log Log = Log.Create(typeof(WeaponMaterialManager));

		private readonly Dictionary<InventoryItemId, Material> _fpsSkins = new Dictionary<InventoryItemId, Material>();

		private readonly Dictionary<InventoryItemId, Material> _tpsSkins = new Dictionary<InventoryItemId, Material>();

		private readonly Dictionary<WeaponId, WeaponMaterial> _fpsMaterials = new Dictionary<WeaponId, WeaponMaterial>();

		private readonly Dictionary<WeaponId, WeaponMaterial> _tpsMaterials = new Dictionary<WeaponId, WeaponMaterial>();

		private readonly WeaponParameters[] _weapons;

		private readonly SkinDefinition[] _skins;

		public WeaponMaterialManager([NotNull] WeaponParameters[] weapons, [NotNull] SkinDefinition[] skins)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			if (skins == null)
			{
				throw new ArgumentNullException("skins");
			}
			_weapons = weapons;
			_skins = skins;
			UpdateMaterials();
		}

		private void LoadSkinMaterial(SkinDefinition skin)
		{
			UnloadSkinMaterial(skin.Id);
			if (Log.DebugEnabled)
			{
				Log.Debug($"LoadSkinMaterial {skin.Id}");
			}
			QualityLvl shaderDetail = VideoSettingsManager.Instance.ShaderDetail;
			if (shaderDetail >= QualityLvl.High)
			{
				_fpsSkins[skin.Id] = InventoryUtility.LoadPbrMaterial(skin);
				_tpsSkins[skin.Id] = InventoryUtility.LoadDiffuseMaterial(skin);
			}
			else if (shaderDetail >= QualityLvl.Medium)
			{
				_fpsSkins[skin.Id] = InventoryUtility.LoadBumpedSpecular(skin);
				_tpsSkins[skin.Id] = InventoryUtility.LoadDiffuseMaterial(skin);
			}
			else if (shaderDetail >= QualityLvl.Low)
			{
				_fpsSkins[skin.Id] = InventoryUtility.LoadBumpedDiffuse(skin);
				_tpsSkins[skin.Id] = InventoryUtility.LoadDiffuseMaterial(skin);
			}
			else
			{
				_fpsSkins[skin.Id] = InventoryUtility.LoadDiffuseMaterial(skin);
				_tpsSkins[skin.Id] = InventoryUtility.LoadDiffuseMaterial(skin);
			}
		}

		private void UnloadSkinMaterial(InventoryItemId skinId)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"UnloadSkinMaterial {skinId}");
			}
			if (_fpsSkins.ContainsKey(skinId))
			{
				Material material = _fpsSkins[skinId];
				Material material2 = _tpsSkins[skinId];
				_fpsSkins.Remove(skinId);
				_tpsSkins.Remove(skinId);
				if (material == material2)
				{
					ResourcesUtility.Unload(material);
					return;
				}
				ResourcesUtility.Unload(material);
				ResourcesUtility.Unload(material2);
			}
		}

		private void UpdateSkinMaterials()
		{
			SkinDefinition[] skins = _skins;
			foreach (SkinDefinition skin in skins)
			{
				LoadSkinMaterial(skin);
			}
		}

		private void LoadDefaultMaterial(WeaponParameters weapon)
		{
			UnloadDefaultMaterial(weapon);
			if (Log.DebugEnabled)
			{
				Log.Debug($"LoadDefaultMaterial {weapon.Id}");
			}
			QualityLvl shaderDetail = VideoSettingsManager.Instance.ShaderDetail;
			if (shaderDetail >= QualityLvl.High)
			{
				_fpsMaterials[weapon.Id] = WeaponUtility.LoadPbrMaterial(weapon.Id);
				_tpsMaterials[weapon.Id] = WeaponUtility.LoadDiffuseMaterial(weapon.Id);
				return;
			}
			if (shaderDetail >= QualityLvl.Medium)
			{
				_fpsMaterials[weapon.Id] = WeaponUtility.LoadBumpedSpecular(weapon.Id);
				_tpsMaterials[weapon.Id] = WeaponUtility.LoadDiffuseMaterial(weapon.Id);
				return;
			}
			if (shaderDetail >= QualityLvl.Low)
			{
				_fpsMaterials[weapon.Id] = WeaponUtility.LoadBumpedDiffuse(weapon.Id);
				_tpsMaterials[weapon.Id] = WeaponUtility.LoadDiffuseMaterial(weapon.Id);
				return;
			}
			Dictionary<WeaponId, WeaponMaterial> fpsMaterials = _fpsMaterials;
			WeaponId id = weapon.Id;
			WeaponMaterial value = WeaponUtility.LoadDiffuseMaterial(weapon.Id);
			_tpsMaterials[weapon.Id] = value;
			fpsMaterials[id] = value;
		}

		private void UnloadDefaultMaterial(WeaponParameters weapon)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"UnloadDefaultMaterial {weapon.Id}");
			}
			if (_fpsMaterials.ContainsKey(weapon.Id))
			{
				WeaponMaterial weaponMaterial = _fpsMaterials[weapon.Id];
				WeaponMaterial weaponMaterial2 = _tpsMaterials[weapon.Id];
				_fpsMaterials.Remove(weapon.Id);
				_tpsMaterials.Remove(weapon.Id);
				if (weaponMaterial == weaponMaterial2)
				{
					ResourcesUtility.Unload(weaponMaterial);
					return;
				}
				ResourcesUtility.Unload(weaponMaterial);
				ResourcesUtility.Unload(weaponMaterial2);
			}
		}

		private void UpdateDefaultMaterials()
		{
			WeaponParameters[] weapons = _weapons;
			foreach (WeaponParameters weapon in weapons)
			{
				LoadDefaultMaterial(weapon);
			}
		}

		public void UpdateMaterials()
		{
			UpdateDefaultMaterials();
			UpdateSkinMaterials();
		}

		public void SetMaterial(WeaponController controller)
		{
			SetMaterial(controller.LodGroup, controller.WeaponId, controller.SkinId);
		}

		public void SetMaterial(MeshLodGroup lodGroup, WeaponId weaponId, InventoryItemId skinId)
		{
			if (skinId != 0)
			{
				Material material = (lodGroup.CurrentLayer != 0) ? _tpsSkins[skinId] : _fpsSkins[skinId];
				lodGroup.SetMaterial(material);
			}
			else
			{
				WeaponMaterial weaponMaterial = (lodGroup.CurrentLayer != 0) ? _tpsMaterials[weaponId] : _fpsMaterials[weaponId];
				lodGroup.SetMaterial(weaponMaterial.MainMaterial);
			}
		}
	}
}
