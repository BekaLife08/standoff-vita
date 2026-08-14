using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Game.UI.NextLevel
{
	public class NextLevelVoteView : View
	{
		[NotNull]
		[SerializeField]
		[FormerlySerializedAs("checkmark")]
		private View _checkmark;

		public bool IsOn
		{
			set
			{
				if (value)
				{
					_checkmark.Show();
				}
				else
				{
					_checkmark.Hide();
				}
			}
		}
	}
}
