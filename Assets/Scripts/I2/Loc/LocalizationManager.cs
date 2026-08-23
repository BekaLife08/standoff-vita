using ArabicSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		public delegate void OnLocalizeCallback();

		private static string mCurrentLanguage;

		private static string mLanguageCode;

		private static bool mChangeCultureInfo = false;

		public static bool IsRight2Left = false;

		public static List<LanguageSource> Sources = new List<LanguageSource>();

		public static string[] GlobalSources = new string[1]
		{
			"I2Languages"
		};

		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		private static bool mLocalizeIsScheduled = false;

		private static bool mLocalizeIsScheduledWithForcedValue = false;

		public static ILocalizeTarget[] mLocalizeTargets = new ILocalizeTarget[0];

		private static string[] LanguagesRTL = new string[20]
		{
			"ar-DZ",
			"ar",
			"ar-BH",
			"ar-EG",
			"ar-IQ",
			"ar-JO",
			"ar-KW",
			"ar-LB",
			"ar-LY",
			"ar-MA",
			"ar-OM",
			"ar-QA",
			"ar-SA",
			"ar-SY",
			"ar-TN",
			"ar-AE",
			"ar-YE",
			"he",
			"ur",
			"ji"
		};

		public static string CurrentLanguage
		{
			get
			{
				InitializeIfNeeded();
				return mCurrentLanguage;
			}
			set
			{
				InitializeIfNeeded();
				string supportedLanguage = GetSupportedLanguage(value);
				if (!string.IsNullOrEmpty(supportedLanguage) && mCurrentLanguage != supportedLanguage)
				{
					SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage));
				}
			}
		}

		public static string CurrentLanguageCode
		{
			get
			{
				InitializeIfNeeded();
				return mLanguageCode;
			}
			set
			{
				InitializeIfNeeded();
				if (mLanguageCode != value)
				{
					string languageFromCode = GetLanguageFromCode(value);
					if (!string.IsNullOrEmpty(languageFromCode))
					{
						SetLanguageAndCode(languageFromCode, value);
					}
				}
			}
		}

		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					text = text.Substring(num);
				}
				CurrentLanguage = text + "(" + value + ")";
			}
		}

		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				return (num >= 0) ? currentLanguageCode.Substring(num + 1) : string.Empty;
			}
			set
			{
				string text = CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				CurrentLanguageCode = text + "-" + value;
			}
		}

		public static event OnLocalizeCallback OnLocalizeEvent;

		private static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(mCurrentLanguage) || Sources.Count == 0)
			{
				UpdateSources();
				SelectStartupLanguage();
			}
		}

		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (mCurrentLanguage != LanguageName || mLanguageCode != LanguageCode || Force)
			{
				if (RememberLanguage)
				{
					PlayerPrefs.SetString("I2 Language", LanguageName);
				}
				mCurrentLanguage = LanguageName;
				mLanguageCode = LanguageCode;
				if (mChangeCultureInfo)
				{
					SetCurrentCultureInfo();
				}
				else
				{
					IsRight2Left = IsRTL(mLanguageCode);
				}
				LocalizeAll(Force);
			}
		}

		private static CultureInfo GetCulture(string code)
		{
			try
			{
				return CultureInfo.CreateSpecificCulture(code);
			}
			catch (Exception)
			{
				return CultureInfo.InvariantCulture;
			}
		}

		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!mChangeCultureInfo && bEnable)
			{
				SetCurrentCultureInfo();
			}
			mChangeCultureInfo = bEnable;
		}

		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = GetCulture(mLanguageCode);
			IsRight2Left = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
		}

		private static void SelectStartupLanguage()
		{
			string @string = PlayerPrefs.GetString("I2 Language", string.Empty);
			string text = Application.systemLanguage.ToString();
			if (text == "ChineseSimplified")
			{
				text = "Chinese (Simplified)";
			}
			if (text == "ChineseTraditional")
			{
				text = "Chinese (Traditional)";
			}
			if (HasLanguage(@string, AllowDiscartingRegion: true, Initialize: false))
			{
				SetLanguageAndCode(@string, GetLanguageCode(@string));
				return;
			}
			string supportedLanguage = GetSupportedLanguage(text);
			if (!string.IsNullOrEmpty(supportedLanguage))
			{
				SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage), RememberLanguage: false);
				return;
			}
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (Sources[i].mLanguages.Count <= 0)
				{
					continue;
				}
				for (int j = 0; j < Sources[i].mLanguages.Count; j++)
				{
					if (Sources[i].mLanguages[j].IsEnabled())
					{
						SetLanguageAndCode(Sources[i].mLanguages[j].Name, Sources[i].mLanguages[j].Code, RememberLanguage: false);
						return;
					}
				}
			}
		}

		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			string Translation = null;
			TryGetTranslation(Term, out Translation, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
			return Translation;
		}

		public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			Translation = null;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			InitializeIfNeeded();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage))
				{
					if (applyParameters)
					{
						ApplyLocalizationParams(ref Translation, localParametersRoot);
					}
					if (IsRight2Left && FixForRTL)
					{
						Translation = ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
					}
					return true;
				}
			}
			return false;
		}

		public static string GetAppName(string languageCode)
		{
			if (!string.IsNullOrEmpty(languageCode))
			{
				for (int i = 0; i < Sources.Count; i++)
				{
					if (string.IsNullOrEmpty(Sources[i].mTerm_AppName))
					{
						continue;
					}
					int languageIndexFromCode = Sources[i].GetLanguageIndexFromCode(languageCode, exactMatch: false);
					if (languageIndexFromCode < 0)
					{
						continue;
					}
					TermData termData = Sources[i].GetTermData(Sources[i].mTerm_AppName);
					if (termData != null)
					{
						string translation = termData.GetTranslation(languageIndexFromCode);
						if (!string.IsNullOrEmpty(translation))
						{
							return translation;
						}
					}
				}
			}
			return Application.productName;
		}

		private static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
		{
			tagStart = -1;
			tagEnd = -1;
			int length = line.Length;
			tagStart = iStart;
			while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{')
			{
				tagStart++;
			}
			if (tagStart == length)
			{
				return false;
			}
			bool flag = false;
			for (tagEnd = tagStart + 1; tagEnd < length; tagEnd++)
			{
				char c = line[tagEnd];
				if (c == ']' || c == ')' || c == '}')
				{
					if (flag)
					{
						return FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
					}
					return true;
				}
				if (c > 'ÿ')
				{
					flag = true;
				}
			}
			return false;
		}

		public static string ApplyRTLfix(string line)
		{
			return ApplyRTLfix(line, 0, ignoreNumbers: true);
		}

		public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
		{
			if (string.IsNullOrEmpty(line))
			{
				return line;
			}
			char c = line[0];
			if (c == '!' || c == '.' || c == '?')
			{
				line = line.Substring(1) + c;
			}
			int tagStart = -1;
			int num = 0;
			int num2 = 40000;
			num = 0;
			List<string> list = new List<string>();
			while (FindNextTag(line, num, out tagStart, out num))
			{
				string str = "@@" + (char)(num2 + list.Count) + "@@";
				list.Add(line.Substring(tagStart, num - tagStart + 1));
				line = line.Substring(0, tagStart) + str + line.Substring(num + 1);
				num = tagStart + 5;
			}
			if (maxCharacters <= 0)
			{
				line = ArabicFixer.Fix(line, true, !ignoreNumbers);
			}
			else
			{
				Regex regex = new Regex(".{0," + maxCharacters + "}(\\s+|$)", RegexOptions.Multiline);
				line = line.Replace("\r\n", "\n");
				line = regex.Replace(line, "$0\n");
				line = line.Replace("\n\n", "\n");
				line = line.TrimEnd('\n');
				string[] array = line.Split('\n');
				int i = 0;
				for (int num3 = array.Length; i < num3; i++)
				{
					array[i] = ArabicFixer.Fix(array[i], true, !ignoreNumbers);
				}
				line = string.Join("\n", array);
			}
			for (int j = 0; j < list.Count; j++)
			{
				int length = line.Length;
				for (int k = 0; k < length; k++)
				{
					if (line[k] == '@' && line[k + 1] == '@' && line[k + 2] >= num2 && line[k + 3] == '@' && line[k + 4] == '@')
					{
						int num4 = line[k + 2] - num2;
						num4 = ((num4 % 2 != 0) ? (num4 - 1) : (num4 + 1));
						if (num4 >= list.Count)
						{
							num4 = list.Count - 1;
						}
						line = line.Substring(0, k) + list[num4] + line.Substring(k + 5);
						break;
					}
				}
			}
			return line;
		}

		public static string ApplyRTLfix1(string line, int maxCharacters, bool ignoreNumbers)
		{
			string pattern = (!ignoreNumbers) ? "(\\s|[^\\x00-\\/:-\\xff])+" : "(\\s|[^\\x00-\\xff])+";
			Regex regex = new Regex(pattern);
			if (maxCharacters <= 0)
			{
				line = regex.Replace(line, (Match m) => ReverseText(ArabicFixer.Fix(m.Value)));
			}
			else
			{
				Regex regex2 = new Regex(".{0," + maxCharacters + "}(\\s+|$)", RegexOptions.Multiline);
				line = line.Replace("\r\n", "\n");
				line = regex2.Replace(line, "$0\n");
				line = line.Replace("\n\n", "\n");
				line = line.TrimEnd('\n');
				string[] array = line.Split('\n');
				int i = 0;
				for (int num = array.Length; i < num; i++)
				{
					array[i] = regex.Replace(array[i], (Match m) => ReverseText(ArabicFixer.Fix(m.Value)));
				}
				line = string.Join("\n", array);
			}
			return line;
		}

		internal static string ReverseText(string source)
		{
			return source;
		}

		public static string RemoveNonASCII(string text, bool allowCategory = false)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (allowCategory)
			{
				return Regex.Replace(text, "[^a-zA-Z0-9\\\\/_ ]+", " ");
			}
			return Regex.Replace(text, "[^a-zA-Z0-9_ ]+", " ");
		}

		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (IsRight2Left)
			{
				return ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		public static void LocalizeAll(bool Force = false)
		{
			if (!IsPlaying())
			{
				DoLocalizeAll(Force);
				return;
			}
			mLocalizeIsScheduledWithForcedValue |= Force;
			if (!mLocalizeIsScheduled)
			{
				CoroutineManager.Start(Coroutine_LocalizeAll());
			}
		}

		private static IEnumerator Coroutine_LocalizeAll()
		{
			mLocalizeIsScheduled = true;
			yield return null;
			mLocalizeIsScheduled = false;
			DoLocalizeAll(mLocalizeIsScheduledWithForcedValue);
			mLocalizeIsScheduledWithForcedValue = false;
		}

		private static void DoLocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				Localize localize = array[i];
				localize.OnLocalize(Force);
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
			ResourceManager.pInstance.CleanResourceCache();
		}

		internal static void ApplyLocalizationParams(ref string translation, GameObject root)
		{
			if (translation == null)
			{
				return;
			}
			Regex regex = new Regex("{\\[(.*?)\\]}");
			MatchCollection matchCollection = regex.Matches(translation);
			int i = 0;
			for (int count = matchCollection.Count; i < count; i++)
			{
				Match match = matchCollection[i];
				string value = match.Groups[match.Groups.Count - 1].Value;
				string localizationParam = GetLocalizationParam(value, root);
				if (localizationParam != null)
				{
					translation = translation.Replace(match.Value, localizationParam);
				}
			}
		}

		internal static string GetLocalizationParam(string ParamName, GameObject root)
		{
			string text = null;
			if ((bool)root)
			{
				MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
				int i = 0;
				for (int num = components.Length; i < num; i++)
				{
					ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
					if (localizationParamsManager != null)
					{
						text = localizationParamsManager.GetParameterValue(ParamName);
						if (text != null)
						{
							return text;
						}
					}
				}
			}
			int j = 0;
			for (int count = ParamManagers.Count; j < count; j++)
			{
				text = ParamManagers[j].GetParameterValue(ParamName);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		public static bool UpdateSources()
		{
			UnregisterDeletededSources();
			RegisterSourceInResources();
			RegisterSceneSources();
			return Sources.Count > 0;
		}

		private static void UnregisterDeletededSources()
		{
			for (int num = Sources.Count - 1; num >= 0; num--)
			{
				if (Sources[num] == null)
				{
					RemoveSource(Sources[num]);
				}
			}
		}

		private static void RegisterSceneSources()
		{
			LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				if (!Sources.Contains(array[i]))
				{
					AddSource(array[i]);
				}
			}
		}

		private static void RegisterSourceInResources()
		{
			string[] globalSources = GlobalSources;
			foreach (string name in globalSources)
			{
				GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>(name);
				LanguageSource languageSource = (!asset) ? null : asset.GetComponent<LanguageSource>();
				if ((bool)languageSource && !Sources.Contains(languageSource))
				{
					AddSource(languageSource);
				}
			}
		}

		internal static void AddSource(LanguageSource Source)
		{
			if (Sources.Contains(Source))
			{
				return;
			}
			Sources.Add(Source);
			if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Never)
			{
				Source.Import_Google_FromCache();
				if (Source.GoogleUpdateDelay > 0f)
				{
					CoroutineManager.pInstance.StartCoroutine(Delayed_Import_Google(Source, Source.GoogleUpdateDelay));
				}
				else
				{
					Source.Import_Google();
				}
			}
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(force: true);
			}
		}

		private static IEnumerator Delayed_Import_Google(LanguageSource source, float delay)
		{
			yield return new WaitForSeconds(delay);
			source.Import_Google();
		}

		internal static void RemoveSource(LanguageSource Source)
		{
			Sources.Remove(Source);
		}

		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf(GlobalSources, SourceName) >= 0;
		}

		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
		{
			if (Initialize)
			{
				InitializeIfNeeded();
			}
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
				{
					return true;
				}
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				for (int count2 = Sources.Count; j < count2; j++)
				{
					if (Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string GetSupportedLanguage(string Language)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndex = Sources[i].GetLanguageIndex(Language, AllowDiscartingRegion: false);
				if (languageIndex >= 0)
				{
					return Sources[i].mLanguages[languageIndex].Name;
				}
			}
			int j = 0;
			for (int count2 = Sources.Count; j < count2; j++)
			{
				int languageIndex2 = Sources[j].GetLanguageIndex(Language);
				if (languageIndex2 >= 0)
				{
					return Sources[j].mLanguages[languageIndex2].Name;
				}
			}
			return string.Empty;
		}

		public static string GetLanguageCode(string Language)
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndex = Sources[i].GetLanguageIndex(Language);
				if (languageIndex >= 0)
				{
					return Sources[i].mLanguages[languageIndex].Code;
				}
			}
			return string.Empty;
		}

		public static string GetLanguageFromCode(string Code, bool exactMatch = true)
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndexFromCode = Sources[i].GetLanguageIndexFromCode(Code, exactMatch);
				if (languageIndexFromCode >= 0)
				{
					return Sources[i].mLanguages[languageIndexFromCode].Name;
				}
			}
			return string.Empty;
		}

		public static List<string> GetAllLanguages(bool SkipDisabled = true)
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			List<string> Languages = new List<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				Languages.AddRange(from x in Sources[i].GetLanguages(SkipDisabled)
					where !Languages.Contains(x)
					select x);
			}
			return Languages;
		}

		public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
		{
			List<string> Languages = new List<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				Languages.AddRange(from x in Sources[i].GetLanguagesCode(allowRegions, SkipDisabled)
					where !Languages.Contains(x)
					select x);
			}
			return Languages;
		}

		public static bool IsLanguageEnabled(string Language)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (!Sources[i].IsLanguageEnabled(Language))
				{
					return false;
				}
			}
			return true;
		}

		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				Sources[i].GetCategories(false, list);
			}
			return list;
		}

		public static List<string> GetTermsList(string Category = null)
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			if (Sources.Count == 1)
			{
				return Sources[0].GetTermsList(Category);
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				hashSet.UnionWith(Sources[i].GetTermsList(Category));
			}
			return new List<string>(hashSet);
		}

		public static TermData GetTermData(string term)
		{
			InitializeIfNeeded();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				TermData termData = Sources[i].GetTermData(term);
				if (termData != null)
				{
					return termData;
				}
			}
			return null;
		}

		public static LanguageSource GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				int i = 0;
				for (int count = Sources.Count; i < count; i++)
				{
					if (Sources[i].GetTermData(term) != null)
					{
						return Sources[i];
					}
				}
			}
			return (!fallbackToFirst || Sources.Count <= 0) ? null : Sources[0];
		}

		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				UnityEngine.Object @object = Sources[i].FindAsset(value);
				if ((bool)@object)
				{
					return @object;
				}
			}
			return null;
		}

		public static string GetVersion()
		{
			return "2.8.0 b6";
		}

		public static int GetRequiredWebServiceVersion()
		{
			return 4;
		}

		public static string GetWebServiceURL(LanguageSource source = null)
		{
			if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
			{
				return source.Google_WebServiceURL;
			}
			InitializeIfNeeded();
			for (int i = 0; i < Sources.Count; i++)
			{
				if (Sources[i] != null && !string.IsNullOrEmpty(Sources[i].Google_WebServiceURL))
				{
					return Sources[i].Google_WebServiceURL;
				}
			}
			return string.Empty;
		}

		public static void RegisterTarget(ILocalizeTarget obj)
		{
			ILocalizeTarget[] array = mLocalizeTargets;
			foreach (ILocalizeTarget localizeTarget in array)
			{
				if (localizeTarget.GetType() == obj.GetType())
				{
					return;
				}
			}
			Array.Resize(ref mLocalizeTargets, mLocalizeTargets.Length + 1);
			mLocalizeTargets[mLocalizeTargets.Length - 1] = obj;
		}

		public static bool IsRTL(string Code)
		{
			return Array.IndexOf(LanguagesRTL, Code) >= 0;
		}

		public static bool IsPlaying()
		{
			if (Application.isPlaying)
			{
				return true;
			}
			return false;
		}
	}
}
