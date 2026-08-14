using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class GameStateView : View
	{
		[FormerlySerializedAs("gameStateText")]
		[SerializeField]
		[NotNull]
		private Text _text;

		public string Text
		{
			get
			{
				return _text.text;
			}
			set
			{
				_text.text = value;
			}
		}
	}
}
