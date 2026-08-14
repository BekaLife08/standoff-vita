using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class WhoKillView : View
	{
		[SerializeField]
		private Image _weaponPreview;

		[SerializeField]
		private Text _weaponText;

		[SerializeField]
		private PlayerView _playerView;

		[SerializeField]
		private Text _damageTakenText;

		[SerializeField]
		private Text _damageGivenText;

		public void Show([NotNull] string weaponName, [NotNull] Sprite weaponPreview, PhotonPlayer killer, HitLogs takenHits, HitLogs givenHits)
		{
			if (weaponName == null)
			{
				throw new ArgumentNullException("weaponName");
			}
			if (weaponPreview == null)
			{
				throw new ArgumentNullException("weaponPreview");
			}
			if (killer == null)
			{
				throw new ArgumentNullException("killer");
			}
			base.Show();
			_playerView.Show(killer);
			_weaponPreview.sprite = weaponPreview;
			_weaponText.text = weaponName;
			_damageTakenText.text = takenHits.Damages.Sum().ToString();
			_damageGivenText.text = givenHits.Damages.Sum().ToString();
		}
	}
}
