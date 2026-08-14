using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(Renderer))]
	public class UVTextureAnimator : MonoBehaviour
	{
		public const int VertexCount = 4;

		public const int TrianglesCount = 2;

		private static readonly Log Log = Log.Create(typeof(UVTextureAnimator));

		[SerializeField]
		private int _atlasSize;

		[SerializeField]
		private float _atlasX;

		[SerializeField]
		private float _atlasY;

		[SerializeField]
		private int _rows;

		[SerializeField]
		private int _columns;

		[SerializeField]
		private float _duration;

		private float _playerStartTime;

		private float _lastUpdateTime;

		private float _deltaTime;

		private bool _isVisible;

		private Vector2 _size;

		private int _count;

		private Renderer _renderer;

		private MeshFilter _meshFilter;

		private bool _batchingEnabled;

		public bool IsPlaying
		{
			get;
			private set;
		}

		private void Awake()
		{
			_renderer = GetComponent<Renderer>();
			_renderer.enabled = false;
			_meshFilter = GetComponent<MeshFilter>();
			Mesh sharedMesh = _meshFilter.sharedMesh;
			_meshFilter.sharedMesh = new Mesh
			{
				vertices = sharedMesh.vertices,
				normals = sharedMesh.normals,
				triangles = sharedMesh.triangles,
				uv = sharedMesh.uv,
				name = "GeneratedQuad" + base.name
			};
			if (_meshFilter.sharedMesh.uv.Length != 4)
			{
				UnityEngine.Debug.LogError("Expected quads with 4 uv vertex");
				return;
			}
			_count = _rows * _columns;
			_size = new Vector2(1f / (float)(_atlasSize * _columns), 1f / (float)(_atlasSize * _rows));
			_deltaTime = _duration / (float)_count;
		}

		public void Play()
		{
			_playerStartTime = Time.time;
			IsPlaying = true;
			if (!_batchingEnabled)
			{
				if (!_renderer.enabled)
				{
					_renderer.enabled = true;
				}
				UpdateFrame();
			}
		}

		public void DontPlay()
		{
			_playerStartTime = 0f;
			IsPlaying = true;
			if (!_batchingEnabled)
			{
				if (!_renderer.enabled)
				{
					_renderer.enabled = true;
				}
				UpdateFrame();
			}
		}

		private void Stop()
		{
			if (IsPlaying)
			{
				IsPlaying = false;
				if (!_batchingEnabled)
				{
					_renderer.enabled = false;
				}
			}
		}

		public void UpdateFrame()
		{
			if (_batchingEnabled)
			{
				Log.Error("Batching enabled, can't update");
				return;
			}
			Vector2[] uv = _meshFilter.sharedMesh.uv;
			UpdateUV(uv, 0);
			_meshFilter.sharedMesh.uv = uv;
		}

		public void UpdateUV(Vector2[] uvs, int from)
		{
			if (IsPlaying && !(Time.time - _lastUpdateTime < _deltaTime))
			{
				int num = (int)((Time.time - _playerStartTime) / _deltaTime);
				if (num >= _count)
				{
					uvs[from] = Vector2.zero;
					uvs[from + 1] = Vector2.zero;
					uvs[from + 2] = Vector2.zero;
					uvs[from + 3] = Vector2.zero;
					Stop();
				}
				else
				{
					int num2 = num / _columns;
					int num3 = num % _columns;
					uvs[from + 3] = new Vector3(_atlasX + (float)num3 * _size.x, _atlasY - (float)num2 * _size.y);
					uvs[from] = uvs[from + 3] + new Vector2(0f, 0f - _size.y);
					uvs[from + 1] = uvs[from + 3] + new Vector2(_size.x, 0f);
					uvs[from + 2] = uvs[from + 3] + new Vector2(_size.x, 0f - _size.y);
					_lastUpdateTime = Time.time;
				}
			}
		}

		public void UpdateVertices(Vector3[] vertices, int from)
		{
			Vector3[] vertices2 = _meshFilter.sharedMesh.vertices;
			foreach (Vector3 position in vertices2)
			{
				vertices[from++] = base.transform.TransformPoint(position);
			}
		}

		public void UpdateNormals(Vector3[] normals, int from)
		{
			Vector3[] normals2 = _meshFilter.sharedMesh.normals;
			foreach (Vector3 vector in normals2)
			{
				normals[from++] = vector;
			}
		}

		public void UpdateTriangles(int[] triangles, int vertexFrom, int trianglesFrom)
		{
			int[] triangles2 = _meshFilter.sharedMesh.triangles;
			foreach (int num in triangles2)
			{
				triangles[trianglesFrom++] = num + vertexFrom;
			}
		}

		public void EnableBatching()
		{
			_batchingEnabled = true;
			_renderer.enabled = false;
		}
	}
}
