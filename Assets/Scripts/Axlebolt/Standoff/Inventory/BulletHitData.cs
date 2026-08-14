using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public class BulletHitData : MessageBase
	{
		public Vector3 Point;

		public float Impulse;

		public int Damage;

		public float ArmorPenetration;

		public BipedMap.Bip Bone;

		public bool Penetrated;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Point);
			writer.Write(Impulse);
			writer.WritePackedUInt32((uint)Damage);
			writer.Write(ArmorPenetration);
			writer.Write((int)Bone);
			writer.Write(Penetrated);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Point = reader.ReadVector3();
			Impulse = reader.ReadSingle();
			Damage = (int)reader.ReadPackedUInt32();
			ArmorPenetration = reader.ReadSingle();
			Bone = (BipedMap.Bip)reader.ReadInt32();
			Penetrated = reader.ReadBoolean();
		}
	}
}
