using Axlebolt.Standoff.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Video
{
	public class QualityLvlRow : View
	{
		[SerializeField]
		private Text _labelText;

		[SerializeField]
		private QualityLvlField _field;

		public Text LabelText
		{
			[CompilerGenerated]
			get
			{
				return _labelText;
			}
		}

		public QualityLvlField Field
		{
			[CompilerGenerated]
			get
			{
				return _field;
			}
		}
	}
}
