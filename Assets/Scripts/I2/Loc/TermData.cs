using System;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class TermData
	{
		public string Term = string.Empty;

		public eTermType TermType;

		public string Description = string.Empty;

		public string[] Languages = new string[0];

		public string[] Languages_Touch = new string[0];

		public byte[] Flags = new byte[0];

		public string GetTranslation(int idx, eTransTag_Input input = eTransTag_Input.Any, eTransTag_Plural plural = eTransTag_Plural.Any)
		{
			if (IsTouchType())
			{
				return string.IsNullOrEmpty(Languages_Touch[idx]) ? Languages[idx] : Languages_Touch[idx];
			}
			return string.IsNullOrEmpty(Languages[idx]) ? Languages_Touch[idx] : Languages[idx];
		}

		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			if (IsTouch)
			{
				return (Flags[idx] & 2) > 0;
			}
			return (Flags[idx] & 1) > 0;
		}

		public bool HasTouchTranslations()
		{
			int i = 0;
			for (int num = Languages_Touch.Length; i < num; i++)
			{
				if (!string.IsNullOrEmpty(Languages_Touch[i]) && !string.IsNullOrEmpty(Languages[i]) && Languages_Touch[i] != Languages[i])
				{
					return true;
				}
			}
			return false;
		}

		public void Validate()
		{
			int num = Mathf.Max(Languages.Length, Mathf.Max(Languages_Touch.Length, Flags.Length));
			if (Languages.Length != num)
			{
				Array.Resize(ref Languages, num);
			}
			if (Languages_Touch.Length != num)
			{
				Array.Resize(ref Languages_Touch, num);
			}
			if (Flags.Length != num)
			{
				Array.Resize(ref Flags, num);
			}
		}

		public static bool IsTouchType()
		{
			return true;
		}

		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == Term;
			}
			return name == LanguageSource.GetKeyFromFullTerm(Term);
		}
	}
}
