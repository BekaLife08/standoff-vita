using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Axlebolt.Standoff.Test
{
	public abstract class BaseTest
	{
		protected IEnumerator Async(Task task)
		{
			while (!task.IsCompleted)
			{
				yield return null;
			}
			if (task.IsFaulted)
			{
				UnityEngine.Debug.LogError(task.Exception);
			}
		}
	}
}
