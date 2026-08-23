using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class PlayerNameView : View
	{
		[SerializeField]
		private InputField _inputField;

		public string Name
		{
			get
			{
				return _inputField.text;
			}
			set
			{
				_inputField.text = value;
			}
		}
	}
}
