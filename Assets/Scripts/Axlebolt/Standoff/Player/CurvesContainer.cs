using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player.State;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class CurvesContainer
	{
		private static readonly Log Log = Log.Create(typeof(CurvesContainer));

		private static readonly Dictionary<int, CurvedValue> CurvesMap = new Dictionary<int, CurvedValue>();

		public static void RegisterCurve(CurvedValue curve)
		{
			if (curve.id < 0)
			{
				UnityEngine.Debug.LogError("Curve Id can not be less than 0");
			}
			else if (!CurvesMap.ContainsKey(curve.id))
			{
				CurvesMap.Add(curve.id, curve);
			}
		}

		public static CurvedValue GetCurve(int id)
		{
			if (id < 0)
			{
				Log.Error($"Curve Id {id} can not be less than 0");
				return null;
			}
			if (CurvesMap.TryGetValue(id, out CurvedValue value))
			{
				return value;
			}
			Log.Error($"Curve Not Found. Id: {id}");
			return null;
		}
	}
}
