using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class CharacterFollowCamera : MonoBehaviour
	{
		public Transform targetObject;

		public float relativeHeight;

		public float relativeDistance;

		public float angleY;

		public void AttachToCharacter(Transform targetTransform)
		{
			targetObject = targetTransform;
		}

		private void Update()
		{
			if (targetObject != null)
			{
				base.transform.eulerAngles = new Vector3(0f, angleY, 0f);
				base.transform.position = targetObject.position + base.transform.forward * relativeDistance + base.transform.up * relativeHeight;
				base.transform.LookAt(targetObject.position + Vector3.up);
			}
		}
	}
}
