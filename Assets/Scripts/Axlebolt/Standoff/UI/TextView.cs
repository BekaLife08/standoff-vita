using JetBrains.Annotations;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class TextView : View
	{
		[NotNull]
		public Text text;

		public string Text
		{
			get
			{
				return text.text;
			}
			set
			{
				text.text = value;
			}
		}
	}
}
