using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[Serializable]
	public class ShotgunShellParameters
	{
		[Serializable]
		public class ShotDispertion
		{
			public int Sectors;

			public float RadiusRatio;
		}

		public List<ShotDispertion> ShotDispertionCircles;
	}
}
