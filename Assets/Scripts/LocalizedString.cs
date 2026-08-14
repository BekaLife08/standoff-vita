using System;

namespace I2.Loc
{
	[Serializable]
	public struct LocalizedString
	{
		public string mTerm;

		public bool mRTL_IgnoreArabicFix;

		public int mRTL_MaxLineLength;

		public bool mRTL_ConvertNumbers;

		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		public static implicit operator LocalizedString(string term)
		{
			LocalizedString result = default(LocalizedString);
			result.mTerm = term;
			return result;
		}

		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(mTerm, !mRTL_IgnoreArabicFix, mRTL_MaxLineLength, !mRTL_ConvertNumbers);
			LocalizationManager.ApplyLocalizationParams(ref translation, null);
			return translation;
		}
	}
}
