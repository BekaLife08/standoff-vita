using Axlebolt.Standoff.Utils;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class BipedMap : MonoBehaviour
	{
		public enum Bip
		{
			Head,
			Neck,
			Spine1,
			Spine2,
			LeftUpperarm,
			LeftForearm,
			LeftHand,
			LeftShoulder,
			RightShoulder,
			RightUpperarm,
			RightForearm,
			RightHand,
			Hip,
			LeftThigh,
			LeftCalf,
			LeftFoot,
			RightThigh,
			RightCalf,
			RightFoot,
			RightThumb0,
			LeftThumb0
		}

		public const string HeadNaming = "BipBase Head";

		public const string NeckNaming = "BipBase Neck1";

		public const string Spine1Naming = "BipBase Spine1";

		public const string Spine2Naming = "BipBase Spine2";

		public const string Left1ShoulderNaming = "BipBase L Clavicle";

		public const string LeftUpperarmNaming = "BipBase L UpperArm";

		public const string LeftForearmNaming = "BipBase L Forearm";

		public const string LeftHandNaming = "BipBase L Hand";

		public const string RightShoulderNaming = "BipBase R Clavicle";

		public const string RightUpperarmNaming = "BipBase R UpperArm";

		public const string RightForearmNaming = "BipBase R Forearm";

		public const string RightHandNaming = "BipBase R Hand";

		public const string HipNaming = "BipBase Pelvis";

		public const string LeftThighNaming = "BipBase L Thigh";

		public const string LeftCalfNaming = "BipBase L Calf";

		public const string LeftFootNaming = "BipBase L Foot";

		public const string RightThighNaming = "BipBase R Thigh";

		public const string RightCalfNaming = "BipBase R Calf";

		public const string RightFootNaming = "BipBase R Foot";

		public Transform Head;

		public Transform Neck;

		public Transform Spine1;

		public Transform Spine2;

		public Transform LeftShoulder;

		public Transform LeftUpperarm;

		public Transform LeftForearm;

		public Transform LeftHand;

		public Transform RightShoulder;

		public Transform RightUpperarm;

		public Transform RightForearm;

		public Transform RightHand;

		public Transform Hip;

		public Transform LeftThigh;

		public Transform LeftCalf;

		public Transform LeftFoot;

		public Transform RightThigh;

		public Transform RightCalf;

		public Transform RightFoot;

		public Transform RightThumb0;

		public Transform LeftThumb0;

		[ContextMenu("Auto Detect")]
		private void AutoDetect()
		{
			Head = UnityUtility.FindDeepChild(base.transform, "BipBase Head");
			Neck = UnityUtility.FindDeepChild(base.transform, "BipBase Neck1");
			Spine1 = UnityUtility.FindDeepChild(base.transform, "BipBase Spine1");
			Spine2 = UnityUtility.FindDeepChild(base.transform, "BipBase Spine2");
			LeftShoulder = UnityUtility.FindDeepChild(base.transform, "BipBase L Clavicle");
			LeftUpperarm = UnityUtility.FindDeepChild(base.transform, "BipBase L UpperArm");
			LeftForearm = UnityUtility.FindDeepChild(base.transform, "BipBase L Forearm");
			LeftHand = UnityUtility.FindDeepChild(base.transform, "BipBase L Hand");
			RightShoulder = UnityUtility.FindDeepChild(base.transform, "BipBase R Clavicle");
			RightUpperarm = UnityUtility.FindDeepChild(base.transform, "BipBase R UpperArm");
			RightForearm = UnityUtility.FindDeepChild(base.transform, "BipBase R Forearm");
			RightHand = UnityUtility.FindDeepChild(base.transform, "BipBase R Hand");
			Hip = UnityUtility.FindDeepChild(base.transform, "BipBase Pelvis");
			LeftThigh = UnityUtility.FindDeepChild(base.transform, "BipBase L Thigh");
			LeftCalf = UnityUtility.FindDeepChild(base.transform, "BipBase L Calf");
			LeftFoot = UnityUtility.FindDeepChild(base.transform, "BipBase L Foot");
			RightThigh = UnityUtility.FindDeepChild(base.transform, "BipBase R Thigh");
			RightCalf = UnityUtility.FindDeepChild(base.transform, "BipBase R Calf");
			RightFoot = UnityUtility.FindDeepChild(base.transform, "BipBase R Foot");
		}

		public Transform GetBone(Bip bone)
		{
			switch (bone)
			{
			case Bip.Head:
				return Head;
			case Bip.Neck:
				return Neck;
			case Bip.LeftUpperarm:
				return LeftUpperarm;
			case Bip.LeftForearm:
				return LeftForearm;
			case Bip.LeftHand:
				return LeftHand;
			case Bip.RightForearm:
				return RightUpperarm;
			case Bip.RightUpperarm:
				return RightForearm;
			case Bip.RightHand:
				return RightHand;
			case Bip.Spine2:
				return Spine2;
			case Bip.Spine1:
				return Spine1;
			case Bip.LeftThigh:
				return LeftThigh;
			case Bip.LeftCalf:
				return LeftCalf;
			case Bip.RightThigh:
				return RightThigh;
			case Bip.RightCalf:
				return RightCalf;
			case Bip.LeftShoulder:
				return LeftShoulder;
			case Bip.RightShoulder:
				return RightShoulder;
			case Bip.Hip:
				return Hip;
			case Bip.LeftFoot:
				return LeftFoot;
			case Bip.RightFoot:
				return RightFoot;
			case Bip.LeftThumb0:
				return LeftThumb0;
			case Bip.RightThumb0:
				return RightThumb0;
			default:
				return null;
			}
		}

		public static bool IsHead(Bip bone)
		{
			return bone == Bip.Head || bone == Bip.Neck;
		}

		public static bool IsChestAndArmsDamage(Bip bone)
		{
			return bone == Bip.Spine2 || bone == Bip.LeftShoulder || bone == Bip.LeftUpperarm || bone == Bip.LeftForearm || bone == Bip.LeftHand || bone == Bip.RightShoulder || bone == Bip.LeftUpperarm || bone == Bip.RightForearm || bone == Bip.RightHand || bone == Bip.RightUpperarm;
		}

		public static bool IsStomach(Bip bone)
		{
			return bone == Bip.Hip || bone == Bip.Spine1;
		}

		public static bool IsLegs(Bip bone)
		{
			return bone == Bip.LeftCalf || bone == Bip.LeftThigh || bone == Bip.LeftFoot || bone == Bip.RightCalf || bone == Bip.RightThigh || bone == Bip.RightFoot;
		}

		public static bool IsTorso(Bip bone)
		{
			return bone == Bip.Spine2 || bone == Bip.Spine1;
		}
	}
}
