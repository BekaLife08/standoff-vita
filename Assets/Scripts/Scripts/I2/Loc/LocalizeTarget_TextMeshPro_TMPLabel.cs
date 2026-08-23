using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_TMPLabel : LocalizeTarget<TextMeshPro>
	{
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		public bool mAlignmentWasRTL;

		public bool mInitializeAlignment = true;

		static LocalizeTarget_TextMeshPro_TMPLabel()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_TextMeshPro_TMPLabel());
		}

		public override string GetName()
		{
			return "TextMeshPro Label";
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
			TextMeshPro target = GetTarget(cmp);
			primaryTerm = target.text;
			secondaryTerm = ((!(target.font != null)) ? string.Empty : target.font.name);
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TextMeshPro target = GetTarget(cmp);
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
						secondaryTranslatedObj = GetTMPFontFromMaterial(cmp, (!secondaryTranslation.EndsWith(secondaryTranslatedObj2.name, StringComparison.Ordinal)) ? secondaryTranslatedObj2.name : secondaryTranslation);
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
				InitAlignment_TMPro(mAlignmentWasRTL, target.alignment, out mAlignment_LTR, out mAlignment_RTL);
			}
			else
			{
				InitAlignment_TMPro(mAlignmentWasRTL, target.alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL);
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
					mainTranslation = LocalizationManager.ReverseText(mainTranslation);
				}
			}
			target.text = mainTranslation;
		}

		internal static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
		{
			string text = " .\\/-[]()";
			int num = matName.Length - 1;
			while (num > 0)
			{
				while (num > 0 && text.IndexOf(matName[num]) >= 0)
				{
					num--;
				}
				if (num <= 0)
				{
					break;
				}
				string translation = matName.Substring(0, num + 1);
				TMP_FontAsset @object = cmp.GetObject<TMP_FontAsset>(translation);
				if (@object != null)
				{
					return @object;
				}
				while (num > 0 && text.IndexOf(matName[num]) < 0)
				{
					num--;
				}
			}
			return null;
		}

		internal static void InitAlignment_TMPro(bool isRTL, TextAlignmentOptions alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL)
		{
			alignLTR = (alignRTL = alignment);
			if (isRTL)
			{
				switch (alignment)
				{
				case TextAlignmentOptions.TopRight:
					alignLTR = TextAlignmentOptions.TopLeft;
					break;
				case TextAlignmentOptions.Right:
					alignLTR = TextAlignmentOptions.Left;
					break;
				case TextAlignmentOptions.BottomRight:
					alignLTR = TextAlignmentOptions.BottomLeft;
					break;
				case TextAlignmentOptions.BaselineRight:
					alignLTR = TextAlignmentOptions.BaselineLeft;
					break;
				case TextAlignmentOptions.MidlineRight:
					alignLTR = TextAlignmentOptions.MidlineLeft;
					break;
				case TextAlignmentOptions.CaplineRight:
					alignLTR = TextAlignmentOptions.CaplineLeft;
					break;
				case TextAlignmentOptions.TopLeft:
					alignLTR = TextAlignmentOptions.TopRight;
					break;
				case TextAlignmentOptions.Left:
					alignLTR = TextAlignmentOptions.Right;
					break;
				case TextAlignmentOptions.BottomLeft:
					alignLTR = TextAlignmentOptions.BottomRight;
					break;
				case TextAlignmentOptions.BaselineLeft:
					alignLTR = TextAlignmentOptions.BaselineRight;
					break;
				case TextAlignmentOptions.MidlineLeft:
					alignLTR = TextAlignmentOptions.MidlineRight;
					break;
				case TextAlignmentOptions.CaplineLeft:
					alignLTR = TextAlignmentOptions.CaplineRight;
					break;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAlignmentOptions.TopRight:
					alignRTL = TextAlignmentOptions.TopLeft;
					break;
				case TextAlignmentOptions.Right:
					alignRTL = TextAlignmentOptions.Left;
					break;
				case TextAlignmentOptions.BottomRight:
					alignRTL = TextAlignmentOptions.BottomLeft;
					break;
				case TextAlignmentOptions.BaselineRight:
					alignRTL = TextAlignmentOptions.BaselineLeft;
					break;
				case TextAlignmentOptions.MidlineRight:
					alignRTL = TextAlignmentOptions.MidlineLeft;
					break;
				case TextAlignmentOptions.CaplineRight:
					alignRTL = TextAlignmentOptions.CaplineLeft;
					break;
				case TextAlignmentOptions.TopLeft:
					alignRTL = TextAlignmentOptions.TopRight;
					break;
				case TextAlignmentOptions.Left:
					alignRTL = TextAlignmentOptions.Right;
					break;
				case TextAlignmentOptions.BottomLeft:
					alignRTL = TextAlignmentOptions.BottomRight;
					break;
				case TextAlignmentOptions.BaselineLeft:
					alignRTL = TextAlignmentOptions.BaselineRight;
					break;
				case TextAlignmentOptions.MidlineLeft:
					alignRTL = TextAlignmentOptions.MidlineRight;
					break;
				case TextAlignmentOptions.CaplineLeft:
					alignRTL = TextAlignmentOptions.CaplineRight;
					break;
				}
			}
		}

		internal static string ReverseText(string source)
		{
			int length = source.Length;
			char[] array = new char[length];
			for (int i = 0; i < length; i++)
			{
				array[length - 1 - i] = source[i];
			}
			return new string(array);
		}
	}
}
