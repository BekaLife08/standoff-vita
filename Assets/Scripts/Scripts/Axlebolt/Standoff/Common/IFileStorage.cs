using System.Collections;

namespace Axlebolt.Standoff.Common
{
	public interface IFileStorage
	{
		T Load<T>(string key, T defaultValue = default(T));

		IEnumerator Save<T>(string key, T value);
	}
}
