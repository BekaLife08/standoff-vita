using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class GunParameterView : View
	{
		[NotNull]
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		[NotNull]
		private Image _valueProgressImage;

		[NotNull]
		[SerializeField]
		private Text _valueText;

		public string Name
		{
			set
			{
				_nameText.text = value;
			}
		}

		public float ValueProgress
		{
			set
			{
				_valueProgressImage.fillAmount = value;
			}
		}

		public string Value
		{
			set
			{
				_valueText.text = value;
			}
		}
	}
}
