using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		static LocalizeTarget_UnityUI_RawImage()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_UnityUI_RawImage());
		}

		public override string GetName()
		{
			return "RawImage";
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
			RawImage target = GetTarget(cmp);
			primaryTerm = ((!target.mainTexture) ? string.Empty : target.mainTexture.name);
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			RawImage target = GetTarget(cmp);
			Texture texture = target.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				target.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}
