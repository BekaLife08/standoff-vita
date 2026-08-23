using System.Collections.Generic;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimLayerDataBuffer : MessageBase
	{
		public List<List<MecanimLayerInfo>> layerDataBuffer = new List<List<MecanimLayerInfo>>();

		public override void Deserialize(NetworkReader reader)
		{
			layerDataBuffer = new List<List<MecanimLayerInfo>>();
			int num = reader.ReadByte();
			int num2 = reader.ReadByte();
			for (int i = 0; i < num; i++)
			{
				List<MecanimLayerInfo> list = new List<MecanimLayerInfo>();
				for (int j = 0; j < num2; j++)
				{
					MecanimLayerInfo mecanimLayerInfo = new MecanimLayerInfo();
					mecanimLayerInfo.Deserialize(reader);
					list.Add(mecanimLayerInfo);
				}
				layerDataBuffer.Add(list);
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			int count = layerDataBuffer.Count;
			int num = 0;
			if (count > 0)
			{
				num = layerDataBuffer[0].Count;
			}
			writer.Write((byte)count);
			writer.Write((byte)num);
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < num; j++)
				{
					layerDataBuffer[i][j].Serialize(writer);
				}
			}
		}

		public MecanimLayerDataBuffer Clone()
		{
			MecanimLayerDataBuffer mecanimLayerDataBuffer = new MecanimLayerDataBuffer();
			for (int i = 0; i < layerDataBuffer.Count; i++)
			{
				List<MecanimLayerInfo> list = layerDataBuffer[i];
				List<MecanimLayerInfo> list2 = null;
				if (list != null)
				{
					list2 = new List<MecanimLayerInfo>();
					for (int j = 0; j < list.Count; j++)
					{
						list2.Add(list[j].Clone());
					}
				}
				mecanimLayerDataBuffer.layerDataBuffer.Add(list2);
			}
			return mecanimLayerDataBuffer;
		}
	}
}
