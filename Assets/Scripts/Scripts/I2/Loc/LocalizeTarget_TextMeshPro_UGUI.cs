using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		public bool mAlignmentWasRTL;

		public bool mInitializeAlignment = true;

		static LocalizeTarget_TextMeshPro_UGUI()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_TextMeshPro_UGUI());
		}

		public override string GetName()
		{
			return "TextMeshPro UGUI";
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
			TextMeshProUGUI target = GetTarget(cmp);
			primaryTerm = target.text;
			secondaryTerm = ((!(target.font != null)) ? string.Empty : target.font.name);
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TextMeshProUGUI target = GetTarget(cmp);
			TMP_FontAsset secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (target.font != secondaryTranslatedObj)
				{
					target.font = secondaryTranslatedObj;
				}
			}
			else
			{
				Material secondaryTranslatedObj2 = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj2 != null && target.fontMaterial != secondaryTranslatedObj2)
				{
					if (!secondaryTranslatedObj2.name.StartsWith(target.font.name, StringComparison.Ordinal))
					{
						secondaryTranslatedObj = LocalizeTarget_TextMeshPro_TMPLabel.GetTMPFontFromMaterial(cmp, (!secondaryTranslation.EndsWith(secondaryTranslatedObj2.name, StringComparison.Ordinal)) ? secondaryTranslatedObj2.name : secondaryTranslation);
						if (secondaryTranslatedObj != null)
						{
							target.font = secondaryTranslatedObj;
						}
					}
					target.fontSharedMaterial = secondaryTranslatedObj2;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_TMPLabel.InitAlignment_TMPro(mAlignmentWasRTL, target.alignment, out mAlignment_LTR, out mAlignment_RTL);
			}
			else
			{
				LocalizeTarget_TextMeshPro_TMPLabel.InitAlignment_TMPro(mAlignmentWasRTL, target.alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL);
				if ((mAlignmentWasRTL && mAlignment_RTL != alignRTL) || (!mAlignmentWasRTL && mAlignment_LTR != alignLTR))
				{
					mAlignment_LTR = alignLTR;
					mAlignment_RTL = alignRTL;
				}
				mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation == null || !(target.text != mainTranslation))
			{
				return;
			}
			if (cmp.CorrectAlignmentForRTL)
			{
				target.alignment = ((!LocalizationManager.IsRight2Left) ? mAlignment_LTR : mAlignment_RTL);
				target.isRightToLeftText = LocalizationManager.IsRight2Left;
				if (LocalizationManager.IsRight2Left)
				{
					mainTranslation = LocalizeTarget_TextMeshPro_TMPLabel.ReverseText(mainTranslation);
				}
			}
			target.text = mainTranslation;
		}
	}
}
