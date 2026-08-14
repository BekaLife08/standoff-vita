using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class Trigger : MonoBehaviour
	{
		[HideInInspector]
		public bool collides;

		private bool isCollisionDetected;

		private void Awake()
		{
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.mass = 0f;
			rigidbody.useGravity = false;
		}

		private void OnCollisionStay(Collision collision)
		{
			collides = true;
			isCollisionDetected = true;
		}

		private void Update()
		{
			if (isCollisionDetected)
			{
				collides = true;
				isCollisionDetected = false;
			}
			else
			{
				collides = false;
			}
		}
	}
}
