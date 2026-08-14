using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Mecanim;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

namespace Unity
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public class GeneratedNetworkCode
	{
		public static void _WriteTransformTRS_None(NetworkWriter writer, TransformTRS value)
		{
			writer.Write(value.pos);
			writer.Write(value.rot);
			writer.Write(value.scale);
		}

		public static TransformTRS _ReadTransformTRS_None(NetworkReader reader)
		{
			TransformTRS transformTRS = new TransformTRS();
			transformTRS.pos = reader.ReadVector3();
			transformTRS.rot = reader.ReadVector3();
			transformTRS.scale = reader.ReadVector3();
			return transformTRS;
		}

		public static void _WriteBulletCastData_None(NetworkWriter writer, BulletCastData value)
		{
			writer.Write(value.StartPosition);
			writer.Write(value.Direction);
		}

		public static void _WriteArrayBulletCastData_None(NetworkWriter writer, BulletCastData[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				_WriteBulletCastData_None(writer, value[num]);
			}
		}

		public static BulletCastData _ReadBulletCastData_None(NetworkReader reader)
		{
			BulletCastData bulletCastData = new BulletCastData();
			bulletCastData.StartPosition = reader.ReadVector3();
			bulletCastData.Direction = reader.ReadVector3();
			return bulletCastData;
		}

		public static BulletCastData[] _ReadArrayBulletCastData_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new BulletCastData[0];
			}
			BulletCastData[] array = new BulletCastData[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _ReadBulletCastData_None(reader);
			}
			return array;
		}

		public static void _WriteBulletHitData_None(NetworkWriter writer, BulletHitData value)
		{
			writer.Write(value.Point);
			writer.Write(value.Impulse);
			writer.WritePackedUInt32((uint)value.Damage);
			writer.Write(value.ArmorPenetration);
			writer.Write((int)value.Bone);
			writer.Write(value.Penetrated);
		}

		public static void _WriteArrayBulletHitData_None(NetworkWriter writer, BulletHitData[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				_WriteBulletHitData_None(writer, value[num]);
			}
		}

		public static BulletHitData _ReadBulletHitData_None(NetworkReader reader)
		{
			BulletHitData bulletHitData = new BulletHitData();
			bulletHitData.Point = reader.ReadVector3();
			bulletHitData.Impulse = reader.ReadSingle();
			bulletHitData.Damage = (int)reader.ReadPackedUInt32();
			bulletHitData.ArmorPenetration = reader.ReadSingle();
			bulletHitData.Bone = (BipedMap.Bip)reader.ReadInt32();
			bulletHitData.Penetrated = reader.ReadBoolean();
			return bulletHitData;
		}

		public static BulletHitData[] _ReadArrayBulletHitData_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new BulletHitData[0];
			}
			BulletHitData[] array = new BulletHitData[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _ReadBulletHitData_None(reader);
			}
			return array;
		}

		public static void _WriteMecanimTransitionInfo_None(NetworkWriter writer, MecanimTransitionInfo value)
		{
			writer.Write(value.name);
			writer.Write(value.duration);
			writer.Write(value.offset);
		}

		public static void _WriteArrayMecanimTransitionInfo_None(NetworkWriter writer, MecanimTransitionInfo[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				_WriteMecanimTransitionInfo_None(writer, value[num]);
			}
		}

		public static MecanimTransitionInfo _ReadMecanimTransitionInfo_None(NetworkReader reader)
		{
			MecanimTransitionInfo mecanimTransitionInfo = new MecanimTransitionInfo();
			mecanimTransitionInfo.name = reader.ReadString();
			mecanimTransitionInfo.duration = reader.ReadSingle();
			mecanimTransitionInfo.offset = reader.ReadSingle();
			return mecanimTransitionInfo;
		}

		public static MecanimTransitionInfo[] _ReadArrayMecanimTransitionInfo_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new MecanimTransitionInfo[0];
			}
			MecanimTransitionInfo[] array = new MecanimTransitionInfo[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _ReadMecanimTransitionInfo_None(reader);
			}
			return array;
		}

		public static void _WriteWeaponControllerCmd_None(NetworkWriter writer, WeaponControllerCmd value)
		{
			writer.Write(value.ToFire);
			writer.Write(value.ToAim);
			writer.Write(value.ToReload);
			writer.Write(value.ToAction);
		}

		public static WeaponControllerCmd _ReadWeaponControllerCmd_None(NetworkReader reader)
		{
			WeaponControllerCmd weaponControllerCmd = new WeaponControllerCmd();
			weaponControllerCmd.ToFire = reader.ReadBoolean();
			weaponControllerCmd.ToAim = reader.ReadBoolean();
			weaponControllerCmd.ToReload = reader.ReadBoolean();
			weaponControllerCmd.ToAction = reader.ReadBoolean();
			return weaponControllerCmd;
		}
	}
}
