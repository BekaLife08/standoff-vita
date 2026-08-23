using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimSyncData : MessageBase
	{
		public float dTime;

		public float time;

		public int layerCount;

		public MecanimLayerInfo[] layers;

		public bool[] boolParams;

		public int[] intParams;

		public float[] floatParams;

		public MecanimLayerDataBuffer layerBuffer = new MecanimLayerDataBuffer();

		public override void Deserialize(NetworkReader reader)
		{
			dTime = reader.ReadSingle();
			time = reader.ReadSingle();
			layerCount = reader.ReadByte();
			int num = reader.ReadByte();
			floatParams = new float[num];
			for (int i = 0; i < num; i++)
			{
				floatParams[i] = reader.ReadSingle();
			}
			layerBuffer.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(dTime);
			writer.Write(time);
			writer.Write((byte)layerCount);
			writer.Write((byte)floatParams.Length);
			float[] array = floatParams;
			foreach (float value in array)
			{
				writer.Write(value);
			}
			layerBuffer.Serialize(writer);
		}

		public MecanimSyncData Clone()
		{
			MecanimSyncData mecanimSyncData = (MecanimSyncData)MemberwiseClone();
			if (layers != null)
			{
				mecanimSyncData.layers = new MecanimLayerInfo[layers.Length];
				for (int i = 0; i < layers.Length; i++)
				{
					mecanimSyncData.layers[i] = layers[i].Clone();
				}
			}
			if (floatParams != null)
			{
				mecanimSyncData.floatParams = new float[floatParams.Length];
				for (int j = 0; j < floatParams.Length; j++)
				{
					mecanimSyncData.floatParams[j] = floatParams[j];
				}
			}
			mecanimSyncData.layerBuffer = layerBuffer.Clone();
			return mecanimSyncData;
		}
	}
}
