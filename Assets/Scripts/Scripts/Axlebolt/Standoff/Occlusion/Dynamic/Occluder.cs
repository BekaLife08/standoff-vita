using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Dynamic
{
	[ExecuteInEditMode]
	public class Occluder : MonoBehaviour
	{
		private readonly Color _colorDefault = new Color(Color.green.r, Color.green.g, Color.green.b, 0.5f);

		private readonly Color _colorSelected = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);

		private readonly Color _colorHidden = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.2f);

		[SerializeField]
		private bool _isHidden;

		private Mesh _mesh;

		private void Awake()
		{
			BoxCollider boxCollider = GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
			boxCollider.isTrigger = true;
			base.gameObject.layer = LayerMask.NameToLayer("DynamicOcclusion");
			if (GetComponent<MeshRenderer>() != null)
			{
				GetComponent<MeshRenderer>().enabled = false;
			}
			_mesh = GetComponent<MeshFilter>().sharedMesh;
		}
	}
}
