using JetBrains.Annotations;
using System;

namespace Axlebolt.Standoff.UI
{
	public class PopupItem
	{
		public string Text
		{
			get;
		}

		public PopupItem([NotNull] string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			Text = text;
		}
	}
}
