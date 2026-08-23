namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class HitSurface
	{
		public static float GetPenetrationLoss(SurfaceType type, float thickness)
		{
			float num = 1f;
			switch (type)
			{
			case SurfaceType.Cardboard:
				return 3f;
			case SurfaceType.Glass:
				return 5f;
			case SurfaceType.MetalGrate:
				return 5f;
			case SurfaceType.ThinWood:
				return 15f;
			case SurfaceType.ThinMetal:
				return 20f;
			case SurfaceType.Character:
				return 15f;
			case SurfaceType.Wood:
				num = 5f;
				break;
			}
			if (type == SurfaceType.Plaster)
			{
				num = 10f;
			}
			if (type == SurfaceType.Tile)
			{
				num = 15f;
			}
			if (type == SurfaceType.Brick)
			{
				num = 20f;
			}
			if (type == SurfaceType.Concrete)
			{
				num = 25f;
			}
			if (type == SurfaceType.Metal)
			{
				num = 100f;
			}
			if (type == SurfaceType.SolidMetal)
			{
				num = 200f;
			}
			return thickness * 100f * num;
		}

		public static bool IsThinMaterial(SurfaceType surfaceType)
		{
			if (surfaceType == SurfaceType.Cardboard || surfaceType == SurfaceType.Glass || surfaceType == SurfaceType.MetalGrate || surfaceType == SurfaceType.Character)
			{
				return true;
			}
			return false;
		}
	}
}
