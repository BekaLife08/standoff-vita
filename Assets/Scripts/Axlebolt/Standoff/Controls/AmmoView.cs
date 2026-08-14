using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Controls
{
	public class AmmoView : View
	{
		[SerializeField]
		private Text _magazineAmmo;

		[SerializeField]
		private Text _ammoSlash;

		[SerializeField]
		private Text _restAmmo;

		[SerializeField]
		private Color _magazineColorNormal;

		[SerializeField]
		private Color _magazineColorCritical;

		public void SetTextViewVisible(bool isEnabled)
		{
			Text magazineAmmo = _magazineAmmo;
			bool flag = isEnabled;
			_ammoSlash.enabled = flag;
			flag = flag;
			_restAmmo.enabled = flag;
			magazineAmmo.enabled = flag;
		}

		public void SetCurMagazineAmmo(short value)
		{
			_magazineAmmo.text = value.ToString();
		}

		public void SetRestAmmoAmount(short value)
		{
			_restAmmo.text = value.ToString();
		}

		public void SetCriticalMagazineAmmo(bool isCritical)
		{
			_magazineAmmo.color = ((!isCritical) ? _magazineColorNormal : _magazineColorCritical);
		}
	}
}
