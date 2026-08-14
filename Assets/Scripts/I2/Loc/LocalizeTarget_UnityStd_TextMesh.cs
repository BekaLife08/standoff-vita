using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStd_TextMesh : LocalizeTarget<TextMesh>
	{
		public TextAlignment mAlignment_RTL = TextAlignment.Right;

		public TextAlignment mAlignment_LTR;

		public bool mAlignmentWasRTL;

		public bool mInitializeAlignment = true;

		static LocalizeTarget_UnityStd_TextMesh()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_TextMesh());
		}

		public override string GetName()
		{
			return "TextMesh";
		}

		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			TextMesh target = GetTarget(cmp);
			primaryTerm = target.text;
			secondaryTerm = ((!string.IsNullOrEmpty(Secondary) || !(target.font != null)) ? null : target.font.name);
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TextMesh target = GetTarget(cmp);
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && target.font != secondaryTranslatedObj)
			{
				target.font = secondaryTranslatedObj;
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mAlignment_LTR = (mAlignment_RTL = target.alignment);
				if (LocalizationManager.IsRight2Left && mAlignment_RTL == TextAlignment.Right)
				{
					mAlignment_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && mAlignment_LTR == TextAlignment.Left)
				{
					mAlignment_RTL = TextAlignment.Right;
				}
			}
			if (mainTranslation != null && target.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && target.alignment != TextAlignment.Center)
				{
					target.alignment = ((!LocalizationManager.IsRight2Left) ? mAlignment_LTR : mAlignment_RTL);
				}
				target.text = mainTranslation;
			}
		}
	}
}
