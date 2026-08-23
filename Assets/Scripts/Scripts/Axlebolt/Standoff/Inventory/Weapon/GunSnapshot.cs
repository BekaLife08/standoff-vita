using System.Collections.Generic;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class GunSnapshot : WeaponSnapshot
	{
		public List<ShotData> ShootDataList = new List<ShotData>();

		public short MagazineAmmo;

		public short RestAmmo;

		public override void Deserialize(NetworkReader reader)
		{
			short num = reader.ReadInt16();
			ShootDataList.Clear();
			for (int i = 0; i < num; i++)
			{
				ShotData shotData = new ShotData();
				shotData.Deserialize(reader);
				ShootDataList.Add(shotData);
			}
			MagazineAmmo = reader.ReadInt16();
			RestAmmo = reader.ReadInt16();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((short)ShootDataList.Count);
			for (int i = 0; i < ShootDataList.Count; i++)
			{
				writer.Write(ShootDataList[i]);
			}
			writer.Write(MagazineAmmo);
			writer.Write(RestAmmo);
		}
	}
}
