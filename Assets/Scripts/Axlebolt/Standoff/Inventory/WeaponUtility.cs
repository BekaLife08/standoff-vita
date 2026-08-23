using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Main.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public static class WeaponUtility
	{
		private const byte BaseWeaponId = 99;

		private const string Model = "_Model";

		private const string AnimationParameters = "_Anim";

		private const string ExtendedParameters = "_Extended";

		[CompilerGenerated]
		private static Func<WeaponId, WeaponParameters> _003C_003Ef__mg_0024cache0;

		public static WeaponParameters[] LoadWeapons()
		{
			return ResourcesUtility.LoadAll<WeaponParameters>("Weapons").ToArray();
		}

		public static WeaponParameters[] LoadBaseWeapons()
		{
			return LoadWeapons(GetBaseWeaponIds());
		}

		public static WeaponId[] GetBaseWeaponIds()
		{
			IEnumerable<WeaponId> source = Enum.GetValues(typeof(WeaponId)).Cast<WeaponId>();
			return (from weaponId in source
				where (int)weaponId <= 99
				select weaponId).ToArray();
		}

		public static WeaponParameters[] LoadWeapons(WeaponId[] ids)
		{
			return ids.Select(LoadWeapon).ToArray();
		}

		public static WeaponParameters LoadWeapon(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponParameters>("Weapons/" + text + "/" + text);
		}

		public static WeaponMaterial LoadPbrMaterial(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + text + "/" + text + "_PBR");
		}

		public static WeaponMaterial LoadBumpedSpecular(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + text + "/" + text + "_BumpedSpecular");
		}

		public static WeaponMaterial LoadBumpedDiffuse(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + text + "/" + text + "_BumpedDiffuse");
		}

		public static WeaponMaterial LoadDiffuseMaterial(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + text + "/" + text + "_Diffuse");
		}

		public static GameObject LoadModel(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<GameObject>("Weapons/" + text + "/" + text + "_Model");
		}

		public static WeaponAnimationParameters LoadAnimationParameters(WeaponId weaponId)
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<WeaponAnimationParameters>("Weapons/" + text + "/" + text + "_Anim");
		}

		public static T LoadExtendedParameters<T>(WeaponId weaponId) where T : ScriptableObject
		{
			string text = weaponId.ToString();
			return ResourcesUtility.Load<T>("Weapons/" + text + "/" + text + "_Extended");
		}

		public static string GetWeaponName(WeaponParameters weaponParameters, SkinDefinition skinDefinition)
		{
			return (!(skinDefinition == null)) ? skinDefinition.DisplayName : weaponParameters.DisplayName;
		}

		public static Sprite GetWeaponPreview(WeaponParameters weaponParameters, SkinDefinition skinDefinition)
		{
			return (!(skinDefinition == null)) ? skinDefinition.PreviewImage : weaponParameters.Sprites.Preview;
		}
	}
}
