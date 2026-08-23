using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponMap : MonoBehaviour
	{
		public enum WeaponPart
		{
			Magazine1 = 1,
			Magazine2,
			Magazine3,
			Gunlock1,
			Gunlock2,
			Weapon,
			MuzzlePoint,
			Sight,
			CollimatorSight,
			SightLense,
			SightReticle,
			Cartridge1,
			Cartridge2
		}

		public GameObject Magazine1
		{
			get;
			private set;
		}

		public GameObject Magazine2
		{
			get;
			private set;
		}

		public GameObject Magazine3
		{
			get;
			private set;
		}

		public GameObject Gunlock1
		{
			get;
			private set;
		}

		public GameObject Gunlock2
		{
			get;
			private set;
		}

		public GameObject Weapon
		{
			get;
			private set;
		}

		public GameObject MuzzlePoint
		{
			get;
			private set;
		}

		public GameObject Sight
		{
			get;
			private set;
		}

		public GameObject CollimatorSight
		{
			get;
			private set;
		}

		public GameObject SightLense
		{
			get;
			private set;
		}

		public GameObject SightReticle
		{
			get;
			private set;
		}

		public GameObject Cartridge1
		{
			get;
			private set;
		}

		public GameObject Cartridge2
		{
			get;
			private set;
		}

		internal void Initialize()
		{
			Weapon = base.gameObject;
			Magazine1 = base.transform.Find(WeaponPart.Magazine1.ToString())?.gameObject;
			Magazine2 = base.transform.Find(WeaponPart.Magazine2.ToString())?.gameObject;
			Magazine3 = base.transform.Find(WeaponPart.Magazine3.ToString())?.gameObject;
			Gunlock1 = base.transform.Find(WeaponPart.Gunlock1.ToString())?.gameObject;
			Gunlock2 = base.transform.Find(WeaponPart.Gunlock2.ToString())?.gameObject;
			MuzzlePoint = base.transform.Find(WeaponPart.MuzzlePoint.ToString())?.gameObject;
			Sight = base.transform.Find(WeaponPart.Sight.ToString())?.gameObject;
			CollimatorSight = base.transform.Find(WeaponPart.CollimatorSight.ToString())?.gameObject;
			SightLense = base.transform.Find(WeaponPart.SightLense.ToString())?.gameObject;
			SightReticle = base.transform.Find(WeaponPart.SightReticle.ToString())?.gameObject;
			Cartridge1 = base.transform.Find(WeaponPart.Cartridge1.ToString())?.gameObject;
			Cartridge2 = base.transform.Find(WeaponPart.Cartridge2.ToString())?.gameObject;
		}

		public void SetLayer(int layer)
		{
			if (Magazine1 != null)
			{
				Magazine1.layer = layer;
			}
			if (Magazine2 != null)
			{
				Magazine2.layer = layer;
			}
			if (Magazine2 != null)
			{
				Magazine2.layer = layer;
			}
			if (Gunlock1 != null)
			{
				Gunlock1.layer = layer;
			}
			if (Gunlock2 != null)
			{
				Gunlock2.layer = layer;
			}
			if (Weapon != null)
			{
				Weapon.layer = layer;
			}
			if (Sight != null)
			{
				Sight.layer = layer;
			}
			if (CollimatorSight != null)
			{
				CollimatorSight.layer = layer;
			}
			if (SightLense != null)
			{
				SightLense.layer = layer;
			}
			if (SightReticle != null)
			{
				SightReticle.layer = layer;
			}
			if (Cartridge1 != null)
			{
				Cartridge1.layer = layer;
			}
			if (Cartridge2 != null)
			{
				Cartridge2.layer = layer;
			}
		}

		public GameObject GetWeaponPart(WeaponPart part)
		{
			switch (part)
			{
			case WeaponPart.Gunlock1:
				return Gunlock1;
			case WeaponPart.Gunlock2:
				return Gunlock2;
			case WeaponPart.Magazine1:
				return Magazine1;
			case WeaponPart.Magazine2:
				return Magazine2;
			case WeaponPart.Magazine3:
				return Magazine3;
			case WeaponPart.Weapon:
				return Weapon;
			case WeaponPart.CollimatorSight:
				return CollimatorSight;
			case WeaponPart.Sight:
				return Sight;
			case WeaponPart.SightLense:
				return SightLense;
			case WeaponPart.SightReticle:
				return SightReticle;
			case WeaponPart.Cartridge1:
				return Cartridge1;
			case WeaponPart.Cartridge2:
				return Cartridge2;
			default:
				return null;
			}
		}
	}
}
