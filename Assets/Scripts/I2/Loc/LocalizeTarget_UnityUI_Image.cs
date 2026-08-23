using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		static LocalizeTarget_UnityUI_Image()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_UnityUI_Image());
		}

		public override string GetName()
		{
			return "Image";
		}

		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			Image target = GetTarget(cmp);
			primaryTerm = ((!target.mainTexture) ? string.Empty : target.mainTexture.name);
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Image target = GetTarget(cmp);
			Sprite sprite = target.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				target.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
