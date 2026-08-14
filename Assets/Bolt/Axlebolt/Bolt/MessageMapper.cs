using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Bolt
{
	public abstract class MessageMapper<TP, TO>
	{
		public abstract TO ToOriginal(TP proto);

		public virtual TP ToProto(TO original)
		{
			throw new NotSupportedException();
		}

		public List<TO> ToOriginalList(IEnumerable<TP> beans)
		{
			if (beans == null)
			{
				throw new ArgumentNullException("beans");
			}
			return beans.Select(ToOriginal).ToList();
		}

		public List<TP> ToProtoList(IEnumerable<TO> beans)
		{
			if (beans == null)
			{
				throw new ArgumentNullException("beans");
			}
			return beans.Select(ToProto).ToList();
		}

		public TO[] ToOriginalArray(IEnumerable<TP> beans)
		{
			if (beans == null)
			{
				throw new ArgumentNullException("beans");
			}
			return beans.Select(ToOriginal).ToArray();
		}

		public TP[] ToProtoArray(IEnumerable<TO> beans)
		{
			if (beans == null)
			{
				throw new ArgumentNullException("beans");
			}
			return beans.Select(ToProto).ToArray();
		}
	}
}
