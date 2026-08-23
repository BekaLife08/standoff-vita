using System;

namespace Axlebolt.Standoff.UI
{
	public interface IAsyncDataProvider<T>
	{
		void LoadCount(Action<int> success, Action<Exception> failed);

		void LoadData(int page, int size, Action<T[]> success, Action<Exception> failed);
	}
}
