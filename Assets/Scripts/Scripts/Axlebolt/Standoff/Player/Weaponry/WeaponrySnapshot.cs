using Axlebolt.Networking;
using Axlebolt.Standoff.Inventory;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Weaponry
{
	public class WeaponrySnapshot : ObjectSnapshot
	{
		public class SlotItem : MessageBase
		{
			public byte SlotIndex;

			public byte WeaponId;

			public short SkinId;

			public override void Deserialize(NetworkReader reader)
			{
				SlotIndex = reader.ReadByte();
				WeaponId = reader.ReadByte();
				SkinId = reader.ReadInt16();
			}

			public override void Serialize(NetworkWriter writer)
			{
				writer.Write(SlotIndex);
				writer.Write(WeaponId);
				writer.Write(SkinId);
			}
		}

		public int WeaponCount;

		public byte CurrentSlotIndex;

		public List<SlotItem> Slot = new List<SlotItem>();

		public WeaponType WeaponType;

		public WeaponSnapshot WeaponSnapshot;

		public WeaponType KitType;

		public WeaponId KitWeaponId;

		public WeaponSnapshot KitSnapshot;

		public override void Deserialize(NetworkReader reader)
		{
			WeaponCount = reader.ReadByte();
			CurrentSlotIndex = reader.ReadByte();
			Slot.Clear();
			for (int i = 0; i < WeaponCount; i++)
			{
				SlotItem slotItem = new SlotItem();
				slotItem.Deserialize(reader);
				Slot.Add(slotItem);
			}
			WeaponType = (WeaponType)reader.ReadByte();
			WeaponSnapshot = WeaponTypeMap.NewSnapshotInstance(WeaponType);
			WeaponSnapshot.Deserialize(reader);
			KitType = (WeaponType)reader.ReadByte();
			if (KitType != 0)
			{
				KitWeaponId = (WeaponId)reader.ReadByte();
				KitSnapshot = WeaponTypeMap.NewSnapshotInstance(KitType);
				KitSnapshot.Deserialize(reader);
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			WeaponCount = Slot.Count;
			writer.Write((byte)WeaponCount);
			writer.Write(CurrentSlotIndex);
			for (int i = 0; i < WeaponCount; i++)
			{
				Slot[i].Serialize(writer);
			}
			writer.Write((byte)WeaponType);
			WeaponSnapshot.Serialize(writer);
			writer.Write((byte)KitType);
			if (KitSnapshot != null)
			{
				writer.Write((byte)KitWeaponId);
				KitSnapshot.Serialize(writer);
			}
		}
	}
}
