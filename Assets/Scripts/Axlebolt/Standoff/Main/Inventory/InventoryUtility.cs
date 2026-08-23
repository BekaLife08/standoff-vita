using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Inventory
{
	public static class InventoryUtility
	{
		public static Material LoadPbrMaterial(SkinDefinition skin)
		{
			return ResourcesUtility.Load<Material>("Inventory/Skins/" + skin.name + "/" + skin.name + "_PBR");
		}

		public static Material LoadBumpedSpecular(SkinDefinition skin)
		{
			return ResourcesUtility.Load<Material>("Inventory/Skins/" + skin.name + "/" + skin.name + "_BumpedSpecular");
		}

		public static Material LoadBumpedDiffuse(SkinDefinition skin)
		{
			return ResourcesUtility.Load<Material>("Inventory/Skins/" + skin.name + "/" + skin.name + "_BumpedDiffuse");
		}

		public static Material LoadDiffuseMaterial(SkinDefinition skin)
		{
			return ResourcesUtility.Load<Material>("Inventory/Skins/" + skin.name + "/" + skin.name + "_Diffuse");
		}
	}
}
