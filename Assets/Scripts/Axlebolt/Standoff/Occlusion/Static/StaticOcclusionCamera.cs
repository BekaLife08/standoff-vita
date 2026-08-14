using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Static
{
	[ExecuteInEditMode]
	public class StaticOcclusionCamera : MonoBehaviour
	{
		private StaticOcclusionArea[] _areas;

		private StaticOcclusionPortal[] _portals;

		private HashSet<MeshRenderer> _enabledRenderers;

		[SerializeField]
		private StaticOcclusionArea _currentArea;

		private int _areaBoxIndex;

		[SerializeField]
		private StaticOcclusionPortal _currentPortal;

		private int _portalBoxIndex;

		private void Awake()
		{
			Init();
		}

		public void Init()
		{
			_currentArea = null;
			_areaBoxIndex = 0;
			_currentPortal = null;
			_portalBoxIndex = 0;
			_enabledRenderers = new HashSet<MeshRenderer>();
			_portals = UnityEngine.Object.FindObjectsOfType<StaticOcclusionPortal>();
			StaticOcclusionPortal[] portals = _portals;
			foreach (StaticOcclusionPortal staticOcclusionPortal in portals)
			{
				staticOcclusionPortal.Init();
				_enabledRenderers.UnionWith(staticOcclusionPortal.MeshRenderers);
			}
			_areas = UnityEngine.Object.FindObjectsOfType<StaticOcclusionArea>();
			StaticOcclusionArea[] areas = _areas;
			foreach (StaticOcclusionArea staticOcclusionArea in areas)
			{
				staticOcclusionArea.Init();
				_enabledRenderers.UnionWith(staticOcclusionArea.MeshRenderers);
			}
			if (_areas.Length == 0)
			{
				base.enabled = false;
			}
		}

		private void Update()
		{
			StaticOcclusionArea currentArea = _currentArea;
			StaticOcclusionPortal currentPortal = _currentPortal;
			UpdateArea();
			UpdatePortal();
			if (currentArea != _currentArea || currentPortal != _currentPortal)
			{
				UpdateView();
			}
		}

		private void UpdateArea()
		{
			if (_currentArea == null)
			{
				FindArea();
				return;
			}
			Vector3 position = base.transform.position;
			if (_currentArea.Contains(position, _areaBoxIndex) || _currentArea.Contains(position, out _areaBoxIndex))
			{
				return;
			}
			if (_currentPortal == null)
			{
				_currentArea = null;
				FindArea();
				return;
			}
			StaticOcclusionArea[] areas = _currentPortal.Areas;
			foreach (StaticOcclusionArea staticOcclusionArea in areas)
			{
				if (staticOcclusionArea.Contains(position, out _areaBoxIndex))
				{
					_currentArea = staticOcclusionArea;
					return;
				}
			}
			_currentArea = null;
			FindArea();
		}

		private void UpdatePortal()
		{
			if (_currentPortal == null)
			{
				FindPortal();
				return;
			}
			Vector3 position = base.transform.position;
			if (!_currentPortal.Contains(position, _portalBoxIndex) && !_currentPortal.Contains(position, out _portalBoxIndex))
			{
				_currentPortal = null;
				FindPortal();
			}
		}

		private void FindArea()
		{
			Vector3 position = base.transform.position;
			StaticOcclusionArea[] areas = _areas;
			int num = 0;
			StaticOcclusionArea staticOcclusionArea;
			while (true)
			{
				if (num < areas.Length)
				{
					staticOcclusionArea = areas[num];
					if (staticOcclusionArea.Contains(position, out _areaBoxIndex))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			_currentArea = staticOcclusionArea;
		}

		private void FindPortal()
		{
			if (_currentArea == null)
			{
				return;
			}
			StaticOcclusionPortal[] portals = _currentArea.Portals;
			int num = 0;
			StaticOcclusionPortal staticOcclusionPortal;
			while (true)
			{
				if (num < portals.Length)
				{
					staticOcclusionPortal = portals[num];
					if (staticOcclusionPortal.Contains(base.transform.position, out _portalBoxIndex))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			_currentPortal = staticOcclusionPortal;
		}

		private void UpdateView()
		{
			foreach (MeshRenderer enabledRenderer in _enabledRenderers)
			{
				enabledRenderer.enabled = false;
			}
			_enabledRenderers.Clear();
			if (_currentArea != null)
			{
				_enabledRenderers.UnionWith(_currentArea.MeshRenderers);
			}
			if (_currentPortal != null)
			{
				_enabledRenderers.UnionWith(_currentPortal.MeshRenderers);
			}
			foreach (MeshRenderer enabledRenderer2 in _enabledRenderers)
			{
				enabledRenderer2.enabled = true;
			}
		}
	}
}
