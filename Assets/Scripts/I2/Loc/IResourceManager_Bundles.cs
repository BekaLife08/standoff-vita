using UnityEngine;

namespace I2.Loc
{
	public interface IResourceManager_Bundles
	{
		T LoadFromBundle<T>(string path) where T : Object;
	}
}
