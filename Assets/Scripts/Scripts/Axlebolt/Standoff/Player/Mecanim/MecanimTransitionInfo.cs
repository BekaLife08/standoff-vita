using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	[Serializable]
	public class MecanimTransitionInfo : MessageBase
	{
		public string name;

		public float duration;

		public float offset;

		private int nameHashed;

		public int GetNameHash()
		{
			if (nameHashed == 0)
			{
				nameHashed = Animator.StringToHash(name);
			}
			return nameHashed;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(name);
			writer.Write(duration);
			writer.Write(offset);
		}

		public override void Deserialize(NetworkReader reader)
		{
			name = reader.ReadString();
			duration = reader.ReadSingle();
			offset = reader.ReadSingle();
		}
	}
}
