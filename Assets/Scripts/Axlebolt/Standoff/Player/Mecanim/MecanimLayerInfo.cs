using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimLayerInfo : MessageBase
	{
		public class ParameterPair : MessageBase
		{
			public int Hash;

			public float Value;

			public override void Serialize(NetworkWriter writer)
			{
				writer.WritePackedUInt32((uint)Hash);
				writer.Write(Value);
			}

			public override void Deserialize(NetworkReader reader)
			{
				Hash = (int)reader.ReadPackedUInt32();
				Value = reader.ReadSingle();
			}
		}

		public float time;

		public int layerInd;

		public bool isInTransition;

		public int stateNameHash;

		public float stateNormalizedTime;

		public float stateLength;

		public int nextStateNameHash;

		public int transitionNameHash;

		public float transitionNormalizedTime;

		public float transitionDuration;

		public float transitionStartTime;

		public bool IsSynchronized;

		public List<ParameterPair> BufferedParametersList = new List<ParameterPair>();

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
			layerInd = reader.ReadByte();
			byte b = reader.ReadByte();
			BufferedParametersList.Clear();
			for (int i = 0; i < b; i++)
			{
				ParameterPair parameterPair = new ParameterPair();
				parameterPair.Hash = reader.ReadInt32();
				parameterPair.Value = reader.ReadSingle();
				BufferedParametersList.Add(parameterPair);
			}
			isInTransition = reader.ReadBoolean();
			stateNameHash = reader.ReadInt32();
			stateNormalizedTime = (float)reader.ReadInt16() / 1000f;
			stateLength = (float)reader.ReadInt16() / 1000f;
			if (isInTransition)
			{
				nextStateNameHash = reader.ReadInt32();
				transitionNameHash = stateNameHash;
				transitionNormalizedTime = stateNormalizedTime;
				transitionDuration = stateLength;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
			writer.Write((byte)layerInd);
			writer.Write((byte)BufferedParametersList.Count);
			foreach (ParameterPair bufferedParameters in BufferedParametersList)
			{
				writer.Write(bufferedParameters.Hash);
				writer.Write(bufferedParameters.Value);
			}
			writer.Write(isInTransition);
			if (isInTransition)
			{
				writer.Write(transitionNameHash);
				float num = transitionNormalizedTime - (float)Math.Truncate(transitionNormalizedTime);
				writer.Write((short)(num * 1000f));
				writer.Write((short)(transitionDuration * 1000f));
				writer.Write(nextStateNameHash);
			}
			else
			{
				writer.Write(stateNameHash);
				float num = stateNormalizedTime - (float)Math.Truncate(stateNormalizedTime);
				writer.Write((short)(num * 1000f));
				writer.Write((short)(stateLength * 1000f));
			}
		}

		public float GetPastTransTime()
		{
			return transitionNormalizedTime * transitionDuration;
		}

		public MecanimLayerInfo Clone()
		{
			return (MecanimLayerInfo)MemberwiseClone();
		}
	}
}
