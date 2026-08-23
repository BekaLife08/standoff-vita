using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Common
{
	public static class LinqExtension
	{
		public static T Random<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable");
			}
			Random random = new Random();
			IList<T> list = (enumerable as IList<T>) ?? enumerable.ToList();
			return (list.Count != 0) ? list[random.Next(0, list.Count)] : default(T);
		}
	}
}
