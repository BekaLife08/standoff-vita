using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponOffsetHelperTool : MonoBehaviour
	{
		public static WeaponOffsetHelperTool Instance;

		[NotNull]
		[Space(5f)]
		public PlayerController playerController;

		public bool isBoneVisualization;

		public void SaveCameraOffsetParameters()
		{
		}
	}
}
