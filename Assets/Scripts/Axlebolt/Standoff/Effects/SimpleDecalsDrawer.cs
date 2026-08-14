using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Axlebolt.Standoff.Effects
{
	public class SimpleDecalsDrawer : DecalsDrawer
	{
		private static readonly Log Log = Log.Create(typeof(SimpleDecalsDrawer));

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private Sprite[] _decals;

		private Vector3[] _vertices;

		private int[] _triangles;

		private Vector2[] _uv;

		private Transform _helper;

		private int _maxDecalsCount;

		private int _verticesOffset;

		private int _tringlesOffset;

		private int _uvOffset;

		private int _offset;

		private Bounds _bounds;

		public static SimpleDecalsDrawer Create([NotNull] Material material, [NotNull] Sprite[] decals, int maxDecalsCount)
		{
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			if (decals == null)
			{
				throw new ArgumentNullException("decals");
			}
			if (decals.Length == 0)
			{
				throw new ArgumentException("Decals are empty");
			}
			ValidateDecals(decals);
			GameObject gameObject = new GameObject(material.name + "SimpleDecalsDrawer");
			SimpleDecalsDrawer simpleDecalsDrawer = gameObject.AddComponent<SimpleDecalsDrawer>();
			simpleDecalsDrawer.Init(material, decals, maxDecalsCount);
			return simpleDecalsDrawer;
		}

		private static void ValidateDecals(Sprite[] declas)
		{
			Texture2D texture = declas[0].texture;
			bool flag = true;
			for (int i = 1; i < declas.Length; i++)
			{
				if (declas[i].texture != texture)
				{
					Log.Error($"Multiple textures not supported, {texture}!={declas[i].texture}. Different PackingTag?");
					flag = false;
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException("Invalid decals");
			}
		}

		private void Init(Material material, Sprite[] decals, int maxDecalsCount)
		{
			_decals = decals;
			_maxDecalsCount = maxDecalsCount;
			_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			_meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			_meshRenderer.material = material;
			_meshRenderer.material.mainTexture = _decals[0].texture;
			_bounds = new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
			_meshFilter.sharedMesh = new Mesh
			{
				bounds = _bounds
			};
			InitArrays();
			GameObject gameObject = new GameObject("DecalHelper");
			gameObject.transform.SetParent(base.transform);
			gameObject.SetActive(value: false);
			_helper = gameObject.transform;
		}

		private void InitArrays()
		{
			_vertices = new Vector3[4 * _maxDecalsCount];
			_triangles = new int[6 * _maxDecalsCount];
			_uv = new Vector2[4 * _maxDecalsCount];
		}

		public override void DrawDecal(RaycastHit raycastHit, Vector2 size, int decalIndex)
		{
			_helper.transform.position = raycastHit.point + 0.001f * raycastHit.normal;
			_helper.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
			if (_offset >= _maxDecalsCount)
			{
				_offset = 0;
				_verticesOffset = 0;
				_tringlesOffset = 0;
				_uvOffset = 0;
			}
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			_vertices[_verticesOffset++] = _helper.transform.TransformPoint(new Vector3(0f - num, num2, 0f));
			_vertices[_verticesOffset++] = _helper.transform.TransformPoint(new Vector3(num, num2, 0f));
			_vertices[_verticesOffset++] = _helper.transform.TransformPoint(new Vector3(0f - num, 0f - num2, 0f));
			_vertices[_verticesOffset++] = _helper.transform.TransformPoint(new Vector3(num, 0f - num2, 0f));
			int num3 = _offset * 4;
			_triangles[_tringlesOffset++] = num3;
			_triangles[_tringlesOffset++] = num3 + 1;
			_triangles[_tringlesOffset++] = num3 + 2;
			_triangles[_tringlesOffset++] = num3 + 2;
			_triangles[_tringlesOffset++] = num3 + 1;
			_triangles[_tringlesOffset++] = num3 + 3;
			_uv[_uvOffset++] = _decals[decalIndex].uv[0];
			_uv[_uvOffset++] = _decals[decalIndex].uv[1];
			_uv[_uvOffset++] = _decals[decalIndex].uv[2];
			_uv[_uvOffset++] = _decals[decalIndex].uv[3];
			_meshFilter.sharedMesh.vertices = _vertices;
			_meshFilter.sharedMesh.triangles = _triangles;
			_meshFilter.sharedMesh.uv = _uv;
			if (_offset == 0)
			{
				_meshFilter.sharedMesh.bounds = _bounds;
			}
			_offset++;
		}

		public override void Clear()
		{
			InitArrays();
			_offset = 0;
			_verticesOffset = 0;
			_tringlesOffset = 0;
			_uvOffset = 0;
			_meshFilter.sharedMesh.vertices = _vertices;
			_meshFilter.sharedMesh.triangles = _triangles;
			_meshFilter.sharedMesh.uv = _uv;
		}
	}
}
