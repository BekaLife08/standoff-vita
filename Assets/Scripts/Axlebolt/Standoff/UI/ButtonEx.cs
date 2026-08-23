using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ButtonEx : Button
	{
		private Text _text;

		protected override void Awake()
		{
			base.Awake();
			_text = GetComponentInChildren<Text>();
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			Color a;
			switch (state)
			{
			case SelectionState.Normal:
				a = base.colors.normalColor;
				break;
			case SelectionState.Highlighted:
				a = base.colors.highlightedColor;
				break;
			case SelectionState.Pressed:
				a = base.colors.pressedColor;
				break;
			case SelectionState.Disabled:
				a = base.colors.disabledColor;
				break;
			default:
				a = Color.black;
				break;
			}
			if (base.gameObject.activeInHierarchy)
			{
				Transition transition = base.transition;
				if (transition == Transition.ColorTint)
				{
					ColorTween(a * base.colors.colorMultiplier, instant);
				}
			}
		}

		private void ColorTween(Color targetColor, bool instant)
		{
			_text.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, ignoreTimeScale: true, useAlpha: true);
		}
	}
}
