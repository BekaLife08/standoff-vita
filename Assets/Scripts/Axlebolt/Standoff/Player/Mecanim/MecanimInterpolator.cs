using Axlebolt.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimInterpolator : Interpolator
	{
		private MecanimSnapshot resultSnapshot = new MecanimSnapshot();

		private MecanimSnapshot fromS;

		private MecanimSnapshot toS;

		private MecanimLayerInfo InterpolateLayerData(MecanimLayerInfo fromD, MecanimLayerInfo toD, float progress)
		{
			MecanimLayerInfo mecanimLayerInfo = fromD.Clone();
			if (fromD.BufferedParametersList.Count == toD.BufferedParametersList.Count && toD.BufferedParametersList.Count > 0)
			{
				mecanimLayerInfo.BufferedParametersList = new List<MecanimLayerInfo.ParameterPair>();
				for (int i = 0; i < toD.BufferedParametersList.Count; i++)
				{
					mecanimLayerInfo.BufferedParametersList.Add(new MecanimLayerInfo.ParameterPair
					{
						Hash = toD.BufferedParametersList[i].Hash,
						Value = Mathf.Lerp(fromD.BufferedParametersList[i].Value, toD.BufferedParametersList[i].Value, progress)
					});
				}
			}
			if (!fromD.isInTransition && !toD.isInTransition)
			{
				mecanimLayerInfo = toD.Clone();
				if (fromD.stateNameHash == toD.stateNameHash)
				{
					float stateNormalizedTime = fromD.stateNormalizedTime;
					float num = toD.stateNormalizedTime;
					if (num < stateNormalizedTime)
					{
						num += 1f;
					}
					float num2 = mecanimLayerInfo.stateNormalizedTime = Mathf.Lerp(stateNormalizedTime, num, progress);
				}
			}
			return mecanimLayerInfo;
		}

		private int GetFromLayer(float time, List<MecanimLayerInfo> layersList)
		{
			if (time <= layersList[0].time)
			{
				return 0;
			}
			for (int i = 0; i < layersList.Count - 1; i++)
			{
				if (layersList[i].time <= time && time <= layersList[i + 1].time)
				{
					return i;
				}
			}
			return layersList.Count - 2;
		}

		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			fromS = (MecanimSnapshot)a;
			toS = (MecanimSnapshot)b;
			resultSnapshot = fromS.Clone();
			MecanimSyncData mecanimSyncData = fromS.mecanimSyncData;
			MecanimSyncData mecanimSyncData2 = toS.mecanimSyncData;
			resultSnapshot.mecanimSyncData.layers = new MecanimLayerInfo[mecanimSyncData2.layerCount];
			for (int i = 0; i < mecanimSyncData2.layerCount; i++)
			{
				List<MecanimLayerInfo> list = new List<MecanimLayerInfo>();
				for (int j = 0; j < mecanimSyncData2.layerBuffer.layerDataBuffer.Count; j++)
				{
					list.Add(mecanimSyncData2.layerBuffer.layerDataBuffer[j][i]);
				}
				float time = fromS.time + (toS.time - fromS.time) * progress;
				int fromLayer = GetFromLayer(time, list);
				int num = fromLayer + 1;
				if (num >= list.Count)
				{
					num = fromLayer;
				}
				resultSnapshot.mecanimSyncData.layers[i] = InterpolateLayerData(list[fromLayer], list[num], progress);
			}
			for (int k = 0; k < mecanimSyncData.floatParams.Length; k++)
			{
				resultSnapshot.mecanimSyncData.floatParams[k] = Mathf.Lerp(mecanimSyncData.floatParams[k], mecanimSyncData2.floatParams[k], progress);
			}
			return resultSnapshot;
		}
	}
}
