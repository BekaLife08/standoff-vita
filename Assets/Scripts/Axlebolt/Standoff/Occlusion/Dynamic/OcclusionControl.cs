using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Dynamic
{
	public class OcclusionControl : Singleton<OcclusionControl>
	{
		private readonly List<ObjectOccludee> _objectOccludees = new List<ObjectOccludee>();

		private Camera _mainCamera;

		public static int OcclusionLayer;

		private Plane[] _frustumPlanes;

		public void Init(Camera mainCamera)
		{
			_mainCamera = mainCamera;
			OcclusionLayer = LayerMask.GetMask("DynamicOcclusion");
		}

		public void Register(ObjectOccludee objectOccludee)
		{
			if (!_objectOccludees.Contains(objectOccludee))
			{
				_objectOccludees.Add(objectOccludee);
			}
		}

		private bool IsInFrustumArea(Bounds bounds)
		{
			return GeometryUtility.TestPlanesAABB(_frustumPlanes, bounds);
		}

		private bool CheckVisibilityByRaycasting(List<Vector3> targetPoints)
		{
			foreach (Vector3 targetPoint in targetPoints)
			{
				Vector3 position = _mainCamera.transform.position;
				Vector3 direction = targetPoint - position;
				RaycastHit hitInfo;
				if (!Physics.Raycast(position, direction, out hitInfo, direction.magnitude, OcclusionLayer))
				{
					return true;
				}
			}
			return false;
		}

		private void HanldeObjectOccludees()
		{
			_frustumPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
			foreach (ObjectOccludee objectOccludee in _objectOccludees)
			{
				if (!objectOccludee.IsActive || !objectOccludee.gameObject.activeInHierarchy)
				{
					continue;
				}
				Bounds characterBounds = objectOccludee.GetCharacterBounds();
				if (IsInFrustumArea(characterBounds))
				{
					if (CheckVisibilityByRaycasting(objectOccludee.GetRaycastHitPoints(_mainCamera.transform.position)))
					{
						if (!objectOccludee.IsVisible)
						{
							objectOccludee.IsVisible = true;
							objectOccludee.OnOcclusionVisible();
						}
					}
					else if (objectOccludee.IsVisible)
					{
						objectOccludee.IsVisible = false;
						objectOccludee.OnOcclusionInvisible();
					}
				}
				else if (objectOccludee.IsVisible)
				{
					objectOccludee.IsVisible = false;
					objectOccludee.OnOcclusionInvisible();
				}
			}
		}

		private void Update()
		{
			if (!(_mainCamera == null))
			{
				HanldeObjectOccludees();
			}
		}
	}
}
