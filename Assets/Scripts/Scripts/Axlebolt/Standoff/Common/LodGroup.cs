using Axlebolt.Standoff.Core;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public abstract class LodGroup : MonoBehaviour
	{
		public delegate void LayerChangedHandler();

		public const float InvisibleDistance = 50f;

		public const string LodNaming = "LOD";

		public const string CombinedNaming = "_C";

		public const string LayerNaming = "L";

		private static readonly Log Log = Log.Create(typeof(MeshLodGroup));

		[HideInInspector]
		[SerializeField]
		private int _currentLayer;

		[SerializeField]
		[HideInInspector]
		private int _currentLevel;

		[SerializeField]
		[HideInInspector]
		private LodLayer[] _layers;

		public LayerChangedHandler LayerChanged;

		public int LodCount
		{
			[CompilerGenerated]
			get
			{
				return Lods.Length;
			}
		}

		public int CurrentLevel
		{
			[CompilerGenerated]
			get
			{
				return _currentLevel;
			}
		}

		internal Lod[] Lods
		{
			[CompilerGenerated]
			get
			{
				return _layers[_currentLayer].Lods;
			}
		}

		internal LodLayer[] Layers
		{
			get
			{
				CheckIsConfigured();
				return _layers;
			}
		}

		public int LayerCount
		{
			[CompilerGenerated]
			get
			{
				return Layers.Length;
			}
		}

		public int CurrentLayer
		{
			[CompilerGenerated]
			get
			{
				return _currentLayer;
			}
		}

		private void OnValidate()
		{
			LodLayer[] layers = _layers;
			foreach (LodLayer lodLayer in layers)
			{
				for (int j = 1; j < lodLayer.Lods.Length; j++)
				{
					if (lodLayer.Lods[j - 1].Width > lodLayer.Lods[j].Width)
					{
						lodLayer.Lods[j].Width = lodLayer.Lods[j - 1].Width;
					}
				}
			}
		}

		public void CalculateLod(float distance)
		{
			int num = Mathf.Min(QualitySettings.maximumLODLevel, Lods.Length - 1);
			int num2 = num;
			while (true)
			{
				if (num2 < Lods.Length)
				{
					if (Lods[num2].Width * 50f > distance)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			SetLod(num2);
		}

		internal abstract void InternalSetLod(int level);

		public void SetLayer(int layer)
		{
			if (LayerCount < layer)
			{
				Log.Error($"Can't change layer to {layer}, exists only {LayerCount}", this);
				return;
			}
			_currentLayer = layer;
			SetLod(_currentLevel);
			OnLayerChanged();
		}

		public void SetLod(int level)
		{
			if (level >= Lods.Length)
			{
				level = Lods.Length - 1;
			}
			InternalSetLod(level);
			_currentLevel = level;
		}

		private void OnLayerChanged()
		{
			LayerChanged?.Invoke();
		}

		public bool IsConfigured()
		{
			return _layers != null && _layers.Length > 0;
		}

		private void CheckIsConfigured()
		{
			if (!IsConfigured())
			{
				throw new Exception("LodGroup " + base.name + " not configured");
			}
		}

		public void RefreshLod()
		{
			if (CurrentLevel < QualitySettings.maximumLODLevel)
			{
				SetLod(QualitySettings.maximumLODLevel);
			}
		}
	}
}
