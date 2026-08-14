using UnityEngine;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour
{
	private void OnGUI()
	{
		if (InputToEvent.goPointedAt != null)
		{
			PhotonView photonView = InputToEvent.goPointedAt.GetPhotonView();
			if (photonView != null)
			{
				Vector3 mousePosition = UnityEngine.Input.mousePosition;
				float x = mousePosition.x + 5f;
				float num = Screen.height;
				Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
				GUI.Label(new Rect(x, num - mousePosition2.y - 15f, 300f, 30f), string.Format("ViewID {0} {1}{2}", photonView.viewID, (!photonView.isSceneView) ? string.Empty : "scene ", (!photonView.isMine) ? ("owner: " + photonView.ownerId) : "mine"));
			}
		}
	}
}
