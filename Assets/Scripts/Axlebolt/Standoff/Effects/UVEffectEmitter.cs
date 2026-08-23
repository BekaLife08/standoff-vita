using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Axlebolt.Standoff.Effects
{
	public class UVEffectEmitter : MonoBehaviour
	{
		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private int _vertexCountPerEffect;

		private int _trianglesCountPerEffect;

		private UVEffect[] _effects;

		private Vector2[] _uvs;

		private Vector3[] _vertices;

		public static UVEffectEmitter Create([NotNull] UVEffect effect, int count)
		{
			if (effect == null)
			{
				throw new ArgumentNullException("effect");
			}
			if (count <= 0)
			{
				throw new ArgumentException(string.Format("{0} must be more 0", "count"));
			}
			GameObject gameObject = new GameObject(effect.name + "Emitter");
			UVEffectEmitter uVEffectEmitter = gameObject.AddComponent<UVEffectEmitter>();
			uVEffectEmitter.Init(effect, count);
			return uVEffectEmitter;
		}

		private void Init([NotNull] UVEffect effect, int count)
		{
			_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			_meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			_effects = new UVEffect[count];
			for (int i = 0; i < count; i++)
			{
				UVEffect uVEffect = UnityEngine.Object.Instantiate(effect, base.transform, worldPositionStays: false);
				uVEffect.EnabledBatching();
				uVEffect.gameObject.SetActive(value: false);
				_effects[i] = uVEffect;
				if (i == 0)
				{
					_meshRenderer.material = uVEffect.Animators[0].GetComponent<MeshRenderer>().material;
				}
			}
			InitMesh();
			Vector3[] vertices = _meshFilter.sharedMesh.vertices;
			int[] triangles = _meshFilter.sharedMesh.triangles;
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				UVEffect uVEffect2 = _effects[j];
				uVEffect2.UpdateVertices(vertices, num);
				uVEffect2.UpdateTriangles(triangles, num, num2);
				num += _vertexCountPerEffect;
				num2 += _trianglesCountPerEffect;
			}
			_meshFilter.sharedMesh.vertices = vertices;
			_meshFilter.sharedMesh.triangles = triangles;
			_meshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
		}

		private void InitMesh()
		{
			int num = _effects[0].Animators.Length;
			_vertexCountPerEffect = num * 4;
			_trianglesCountPerEffect = num * 2 * 3;
			int num2 = _effects.Length;
			int num3 = _vertexCountPerEffect * num2;
			int num4 = _trianglesCountPerEffect * num2;
			_uvs = new Vector2[num3];
			_vertices = new Vector3[num3];
			_meshFilter.sharedMesh = new Mesh
			{
				vertices = new Vector3[num3],
				uv = new Vector2[num3],
				triangles = new int[num4]
			};
		}

		public void Emit(int index, Vector3 position, Quaternion rotation, bool[] playFlags = null)
		{
			UVEffect uVEffect = _effects[index];
			uVEffect.transform.position = position;
			uVEffect.transform.rotation = rotation;
			uVEffect.gameObject.SetActive(value: true);
			int from = index * _vertexCountPerEffect;
			uVEffect.UpdateVertices(_vertices, from);
			_meshFilter.sharedMesh.vertices = _vertices;
			uVEffect.Play(playFlags);
		}

		private void Update()
		{
			bool flag = false;
			for (int i = 0; i < _effects.Length; i++)
			{
				UVEffect uVEffect = _effects[i];
				if (uVEffect.gameObject.activeInHierarchy && uVEffect.IsPlaying())
				{
					Update(_uvs, uVEffect, i);
					flag = true;
				}
				else if (uVEffect.gameObject.activeInHierarchy)
				{
					uVEffect.gameObject.SetActive(value: false);
				}
			}
			if (flag)
			{
				_meshFilter.sharedMesh.uv = _uvs;
			}
		}

		public bool IsPlaying(int index)
		{
			return _effects[index].IsPlaying();
		}

		private void Update(Vector2[] uv, UVEffect uvEffect, int index)
		{
			uvEffect.UpdateUV(uv, index * _vertexCountPerEffect);
		}
	}
}
