using Unity;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class ShotData : MessageBase
	{
		public short shootNo;

		public float ShootTime;

		public BulletCastData[] BulletCastDataList;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)shootNo);
			writer.Write(ShootTime);
			GeneratedNetworkCode._WriteArrayBulletCastData_None(writer, BulletCastDataList);
		}

		public override void Deserialize(NetworkReader reader)
		{
			shootNo = (short)reader.ReadPackedUInt32();
			ShootTime = reader.ReadSingle();
			BulletCastDataList = GeneratedNetworkCode._ReadArrayBulletCastData_None(reader);
		}
	}
}
