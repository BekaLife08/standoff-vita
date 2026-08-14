using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Occlusion.Dynamic
{
	public abstract class ObjectOccludee : MonoBehaviour
	{
		public bool IsVisible { get; internal set; }

		public bool IsActive { get; private set; }

		public virtual void Initialize()
		{
			IsVisible = true;
		}

		public void SetEnabled(bool isEnabled)
		{
			if (IsActive && !isEnabled && !IsVisible)
			{
				OnOcclusionVisible();
			}
			IsActive = isEnabled;
		}

		public void SetVisible(bool isVisible)
		{
			if (isVisible != IsVisible)
			{
				IsVisible = isVisible;
				if (isVisible)
				{
					OnOcclusionVisible();
				}
				else
				{
					OnOcclusionInvisible();
				}
			}
		}

		public virtual void OnOcclusionVisible()
		{
		}

		public virtual void OnOcclusionInvisible()
		{
		}

		public abstract Bounds GetCharacterBounds();

		public abstract List<Vector3> GetRaycastHitPoints(Vector3 casterPoint);
	}
}
