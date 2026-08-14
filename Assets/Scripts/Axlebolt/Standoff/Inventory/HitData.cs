using Axlebolt.Standoff.Main.Inventory;
using System.Runtime.CompilerServices;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public class HitData : MessageBase
	{
		public Vector3 Direction;

		public WeaponId WeaponId;

		public InventoryItemId SkinId;

		public BulletHitData[] Hits;

		public BulletHitData Hit
		{
			[CompilerGenerated]
			get
			{
				return Hits[0];
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Direction);
			writer.Write((int)WeaponId);
			writer.Write((int)SkinId);
			GeneratedNetworkCode._WriteArrayBulletHitData_None(writer, Hits);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Direction = reader.ReadVector3();
			WeaponId = (WeaponId)reader.ReadInt32();
			SkinId = (InventoryItemId)reader.ReadInt32();
			Hits = GeneratedNetworkCode._ReadArrayBulletHitData_None(reader);
		}
	}
}
