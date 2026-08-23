using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpawnPoint
	{
		public Vector3 Position
		{
			get;
		}

		public Quaternion Rotation
		{
			get;
		}

		public SpawnPoint(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}
}
