using System.Linq;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class UVEffect : MonoBehaviour
	{
		public UVTextureAnimator[] Animators
		{
			get;
			private set;
		}

		private void Awake()
		{
			Animators = GetComponentsInChildren<UVTextureAnimator>();
		}

		public void Play(bool[] playFlags = null)
		{
			for (int i = 0; i < Animators.Length; i++)
			{
				UVTextureAnimator uVTextureAnimator = Animators[i];
				if (playFlags == null || playFlags.Length <= i || playFlags[i])
				{
					uVTextureAnimator.Play();
				}
				else
				{
					uVTextureAnimator.DontPlay();
				}
			}
		}

		public void UpdateAnimation()
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				if (uVTextureAnimator.IsPlaying)
				{
					uVTextureAnimator.UpdateFrame();
				}
			}
		}

		public void UpdateUV(Vector2[] uv, int from)
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				uVTextureAnimator.UpdateUV(uv, from);
				from += 4;
			}
		}

		public void UpdateVertices(Vector3[] vertices, int from)
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				uVTextureAnimator.UpdateVertices(vertices, from);
				from += 4;
			}
		}

		public void UpdateTriangles(int[] triangles, int vertexFrom, int trianglesFrom)
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				uVTextureAnimator.UpdateTriangles(triangles, vertexFrom, trianglesFrom);
				vertexFrom += 4;
				trianglesFrom += 6;
			}
		}

		public void UpdateNormals(Vector3[] normals, int from)
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				uVTextureAnimator.UpdateNormals(normals, from);
				from += 4;
			}
		}

		public bool IsPlaying()
		{
			return Animators.Any((UVTextureAnimator animator) => animator.IsPlaying);
		}

		public void EnabledBatching()
		{
			UVTextureAnimator[] animators = Animators;
			foreach (UVTextureAnimator uVTextureAnimator in animators)
			{
				uVTextureAnimator.EnableBatching();
			}
		}
	}
}
