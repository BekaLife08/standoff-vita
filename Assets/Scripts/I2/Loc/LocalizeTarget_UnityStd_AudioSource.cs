using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStd_AudioSource : LocalizeTarget<AudioSource>
	{
		static LocalizeTarget_UnityStd_AudioSource()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_AudioSource());
		}

		public override string GetName()
		{
			return "AudioSource";
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
			AudioSource target = GetTarget(cmp);
			primaryTerm = ((!target.clip) ? string.Empty : target.clip.name);
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			AudioSource target = GetTarget(cmp);
			bool isPlaying = target.isPlaying;
			AudioClip clip = target.clip;
			AudioClip audioClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
			if (clip != audioClip)
			{
				target.clip = audioClip;
			}
			if (isPlaying && (bool)target.clip)
			{
				target.Play();
			}
		}
	}
}
