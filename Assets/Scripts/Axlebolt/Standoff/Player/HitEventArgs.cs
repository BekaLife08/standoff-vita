using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Main.Inventory;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Player
{
	public class HitEventArgs
	{
		public PhotonPlayer Shooter
		{
			get;
		}

		public PhotonPlayer Victim
		{
			get;
		}

		public int Damage
		{
			get;
		}

		public WeaponParameters Weapon
		{
			get;
		}

		public SkinDefinition Skin
		{
			get;
		}

		public bool Headshot
		{
			get;
		}

		public bool Penetrated
		{
			get;
		}

		public HitEventArgs([NotNull] PhotonPlayer shooter, [NotNull] PhotonPlayer victim, int damage, [NotNull] WeaponParameters weapon, SkinDefinition skin, bool headshot, bool penetrated)
		{
			Shooter = shooter;
			Victim = victim;
			Damage = damage;
			Weapon = weapon;
			Skin = skin;
			Headshot = headshot;
			Penetrated = penetrated;
		}
	}
}
