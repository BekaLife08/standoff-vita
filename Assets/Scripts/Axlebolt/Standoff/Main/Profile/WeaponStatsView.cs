using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class WeaponStatsView : View
	{
		public class WeaponStats
		{
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly WeaponParameters _003CWeaponParameters_003Ek__BackingField;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly int _003CKills_003Ek__BackingField;

			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly float _003CAccuracePercentage_003Ek__BackingField;

			public WeaponParameters WeaponParameters
			{
				[CompilerGenerated]
				get
				{
					return _003CWeaponParameters_003Ek__BackingField;
				}
			}

			public int Kills
			{
				[CompilerGenerated]
				get
				{
					return _003CKills_003Ek__BackingField;
				}
			}

			public float AccuracePercentage
			{
				[CompilerGenerated]
				get
				{
					return _003CAccuracePercentage_003Ek__BackingField;
				}
			}

			public WeaponStats([NotNull] WeaponParameters weaponParameters, int kills, float accuracePercentage)
			{
				if (weaponParameters == null)
				{
					throw new ArgumentNullException("weaponParameters");
				}
				_003CWeaponParameters_003Ek__BackingField = weaponParameters;
				_003CKills_003Ek__BackingField = kills;
				_003CAccuracePercentage_003Ek__BackingField = accuracePercentage;
			}
		}

		[SerializeField]
		private Image _image;

		[SerializeField]
		private Image _glowImage;

		[SerializeField]
		private Text _name;

		[SerializeField]
		private Text _kills;

		[SerializeField]
		private Text _accuracy;

		public void Show(int index, WeaponStats weaponStats)
		{
			WeaponParameters weaponParameters = weaponStats.WeaponParameters;
			_image.sprite = weaponParameters.Sprites.Icon;
			_glowImage.sprite = weaponParameters.Sprites.Glow;
			_name.text = index + 1 + ". " + weaponParameters.DisplayName;
			_kills.text = weaponStats.Kills.ToString();
			_accuracy.text = weaponStats.AccuracePercentage.ToString("0.0") + " %";
			Show();
		}
	}
}
