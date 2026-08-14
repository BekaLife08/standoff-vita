using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Bolt
{
	public class EnumMapper<TP, TO>
	{
		private static readonly Dictionary<TP, TO> OriginalMap;

		private static readonly Dictionary<TO, TP> ProtoMap;

		static EnumMapper()
		{
			Type protoType = typeof(TP);
			Type originalType = typeof(TO);
			OriginalMap = Enum.GetValues(typeof(TO)).Cast<TO>().ToDictionary((TO tp) => (TP)Enum.Parse(protoType, tp.ToString()));
			ProtoMap = Enum.GetValues(typeof(TP)).Cast<TP>().ToDictionary((TP tp) => (TO)Enum.Parse(originalType, tp.ToString()));
		}

		public static TO ToOriginal(TP proto)
		{
			return OriginalMap[proto];
		}

		public static TP ToProto(TO original)
		{
			return ProtoMap[original];
		}

		public static TO[] ToOriginal(TP[] proto)
		{
			return proto.Select(ToOriginal).ToArray();
		}

		public static TP[] ToProto(TO[] original)
		{
			return original.Select(ToProto).ToArray();
		}
	}
}
