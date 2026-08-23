namespace I2.Loc
{
	public abstract class ILocalizeTarget
	{
		public abstract bool FindTarget(Localize cmp);

		public abstract void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		public abstract void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation);

		public abstract ILocalizeTarget Clone(Localize cmp);

		public abstract string GetName();

		public abstract bool CanLocalize(Localize cmp);

		public abstract bool CanUseSecondaryTerm();

		public abstract bool AllowMainTermToBeRTL();

		public abstract bool AllowSecondTermToBeRTL();

		public abstract bool HasTarget(Localize cmp);
	}
}
