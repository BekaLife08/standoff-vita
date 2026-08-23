using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Source")]
	public class LanguageSource : MonoBehaviour
	{
		public enum eGoogleUpdateFrequency
		{
			Always,
			Never,
			Daily,
			Weekly,
			Monthly,
			OnlyOnce
		}

		public enum eInputSpecialization
		{
			PC,
			Touch,
			Controller
		}

		public enum MissingTranslationAction
		{
			Empty,
			Fallback,
			ShowWarning
		}

		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		public eGoogleUpdateFrequency GoogleUpdateFrequency = eGoogleUpdateFrequency.Weekly;

		public float GoogleUpdateDelay = 5f;

		public static string EmptyCategory = "Default";

		public static char[] CategorySeparators = "/\\".ToCharArray();

		public List<TermData> mTerms = new List<TermData>();

		public List<LanguageData> mLanguages = new List<LanguageData>();

		public bool CaseInsensitiveTerms;

		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		public UnityEngine.Object[] Assets;

		public bool NeverDestroy = true;

		public bool UserAgreesToHaveItOnTheScene;

		public bool UserAgreesToHaveItInsideThePluginsFolder;

		public MissingTranslationAction OnMissingTranslation = MissingTranslationAction.Fallback;

		public string mTerm_AppName;

		public event Action<LanguageSource, bool, string> Event_OnSourceUpdateFromGoogle;

		public string Export_I2CSV(string Category, char Separator = ',')
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Key[*]Type[*]Desc");
			foreach (LanguageData mLanguage in mLanguages)
			{
				stringBuilder.Append("[*]");
				if (!mLanguage.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				stringBuilder.Append(GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code));
			}
			stringBuilder.Append("[ln]");
			int count = mLanguages.Count;
			bool flag = true;
			foreach (TermData mTerm in mTerms)
			{
				string term;
				if (string.IsNullOrEmpty(Category) || (Category == EmptyCategory && mTerm.Term.IndexOfAny(CategorySeparators) < 0))
				{
					term = mTerm.Term;
				}
				else
				{
					if (!mTerm.Term.StartsWith(Category + "/") || !(Category != mTerm.Term))
					{
						continue;
					}
					term = mTerm.Term.Substring(Category.Length + 1);
				}
				if (!flag)
				{
					stringBuilder.Append("[ln]");
				}
				else
				{
					flag = false;
				}
				AppendI2Term(stringBuilder, count, term, mTerm, string.Empty, mTerm.Languages, mTerm.Languages_Touch, Separator, 1, 2);
				if (mTerm.HasTouchTranslations())
				{
					if (!flag)
					{
						stringBuilder.Append("[ln]");
					}
					else
					{
						flag = false;
					}
					AppendI2Term(stringBuilder, count, term, mTerm, "[touch]", mTerm.Languages_Touch, null, Separator, 2, 1);
				}
			}
			return stringBuilder.ToString();
		}

		private static void AppendI2Term(StringBuilder Builder, int nLanguages, string Term, TermData termData, string postfix, string[] aLanguages, string[] aSecLanguages, char Separator, byte FlagBitMask, byte SecFlagBitMask)
		{
			Builder.Append(Term);
			Builder.Append(postfix);
			Builder.Append("[*]");
			Builder.Append(termData.TermType.ToString());
			Builder.Append("[*]");
			Builder.Append(termData.Description);
			for (int i = 0; i < Mathf.Min(nLanguages, aLanguages.Length); i++)
			{
				Builder.Append("[*]");
				string value = aLanguages[i];
				if (string.IsNullOrEmpty(value) && aSecLanguages != null)
				{
					value = aSecLanguages[i];
				}
				Builder.Append(value);
			}
		}

		public string Export_CSV(string Category, char Separator = ',')
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc", Separator);
			foreach (LanguageData mLanguage in mLanguages)
			{
				stringBuilder.Append(Separator);
				if (!mLanguage.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code), Separator);
			}
			stringBuilder.Append("\n");
			for (int i = 0; i < mTerms.Count - 1; i++)
			{
				for (int j = i + 1; j < mTerms.Count; j++)
				{
					if (string.CompareOrdinal(mTerms[i].Term, mTerms[j].Term) > 0)
					{
						TermData value = mTerms[i];
						mTerms[i] = mTerms[j];
						mTerms[j] = value;
					}
				}
			}
			foreach (TermData mTerm in mTerms)
			{
				string term;
				if (string.IsNullOrEmpty(Category) || (Category == EmptyCategory && mTerm.Term.IndexOfAny(CategorySeparators) < 0))
				{
					term = mTerm.Term;
				}
				else
				{
					if (!mTerm.Term.StartsWith(Category + "/") || !(Category != mTerm.Term))
					{
						continue;
					}
					term = mTerm.Term.Substring(Category.Length + 1);
				}
				AppendTerm(stringBuilder, count, term, mTerm, null, mTerm.Languages, mTerm.Languages_Touch, Separator, 1, 2);
				if (mTerm.HasTouchTranslations())
				{
					AppendTerm(stringBuilder, count, term, mTerm, "[touch]", mTerm.Languages_Touch, null, Separator, 2, 1);
				}
			}
			return stringBuilder.ToString();
		}

		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string prefix, string[] aLanguages, string[] aSecLanguages, char Separator, byte FlagBitMask, byte SecFlagBitMask)
		{
			AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(prefix))
			{
				Builder.Append(prefix);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			AppendString(Builder, termData.Description, Separator);
			for (int i = 0; i < Mathf.Min(nLanguages, aLanguages.Length); i++)
			{
				Builder.Append(Separator);
				string text = aLanguages[i];
				if (string.IsNullOrEmpty(text) && aSecLanguages != null)
				{
					text = aSecLanguages[i];
				}
				AppendTranslation(Builder, text, Separator, string.Empty);
			}
			Builder.Append("\n");
		}

		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				Text = Text.Replace("\\n", "\n");
				if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
				{
					Text = Text.Replace("\"", "\"\"");
					Builder.AppendFormat("\"{0}\"", Text);
				}
				else
				{
					Builder.Append(Text);
				}
			}
		}

		private static void AppendTranslation(StringBuilder Builder, string Text, char Separator, string tags)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				Text = Text.Replace("\\n", "\n");
				if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
				{
					Text = Text.Replace("\"", "\"\"");
					Builder.AppendFormat("\"{0}{1}\"", tags, Text);
				}
				else
				{
					Builder.Append(tags);
					Builder.Append(Text);
				}
			}
		}

		public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string value = Export_Google_CreateData();
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("key", Google_SpreadsheetKey);
			wWWForm.AddField("action", "SetLanguageSource");
			wWWForm.AddField("data", value);
			wWWForm.AddField("updateMode", UpdateMode.ToString());
			return new WWW(LocalizationManager.GetWebServiceURL(this), wWWForm);
		}

		private string Export_Google_CreateData()
		{
			List<string> categories = GetCategories(OnlyMainCategory: true);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string item in categories)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("<I2Loc>");
				}
				string value = Export_I2CSV(item);
				stringBuilder.Append(item);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> cSV = LocalizationReader.ReadCSV(CSVstring, Separator);
			return Import_CSV(Category, cSV, UpdateMode);
		}

		public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> cSV = LocalizationReader.ReadI2CSV(I2CSVstring);
			return Import_CSV(Category, cSV, UpdateMode);
		}

		public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string[] array = CSV[0];
			int num = 1;
			int num2 = -1;
			int num3 = -1;
			string[] texts = new string[1]
			{
				"Key"
			};
			string[] texts2 = new string[1]
			{
				"Type"
			};
			string[] texts3 = new string[2]
			{
				"Desc",
				"Description"
			};
			if (array.Length > 1 && ArrayContains(array[0], texts))
			{
				if (UpdateMode == eSpreadsheetUpdateMode.Replace)
				{
					ClearAllData();
				}
				if (array.Length > 2)
				{
					if (ArrayContains(array[1], texts2))
					{
						num2 = 1;
						num = 2;
					}
					if (ArrayContains(array[1], texts3))
					{
						num3 = 1;
						num = 2;
					}
				}
				if (array.Length > 3)
				{
					if (ArrayContains(array[2], texts2))
					{
						num2 = 2;
						num = 3;
					}
					if (ArrayContains(array[2], texts3))
					{
						num3 = 2;
						num = 3;
					}
				}
				int num4 = Mathf.Max(array.Length - num, 0);
				int[] array2 = new int[num4];
				for (int i = 0; i < num4; i++)
				{
					if (string.IsNullOrEmpty(array[i + num]))
					{
						array2[i] = -1;
						continue;
					}
					string text = array[i + num];
					bool flag = true;
					if (text.StartsWith("$"))
					{
						flag = false;
						text = text.Substring(1);
					}
					GoogleLanguages.UnPackCodeFromLanguageName(text, out string Language, out string code);
					int num5 = -1;
					num5 = (string.IsNullOrEmpty(code) ? GetLanguageIndex(Language) : GetLanguageIndexFromCode(code));
					if (num5 < 0)
					{
						LanguageData languageData = new LanguageData();
						languageData.Name = Language;
						languageData.Code = code;
						languageData.Flags = (byte)(0 | ((!flag) ? 1 : 0));
						mLanguages.Add(languageData);
						num5 = mLanguages.Count - 1;
					}
					array2[i] = num5;
				}
				num4 = mLanguages.Count;
				int j = 0;
				for (int count = mTerms.Count; j < count; j++)
				{
					TermData termData = mTerms[j];
					if (termData.Languages.Length < num4)
					{
						Array.Resize(ref termData.Languages, num4);
						Array.Resize(ref termData.Languages_Touch, num4);
						Array.Resize(ref termData.Flags, num4);
					}
				}
				int k = 1;
				for (int count2 = CSV.Count; k < count2; k++)
				{
					array = CSV[k];
					string Term = (!string.IsNullOrEmpty(Category)) ? (Category + "/" + array[0]) : array[0];
					bool flag2 = false;
					if (Term.EndsWith("[touch]"))
					{
						Term = Term.Remove(Term.Length - "[touch]".Length);
						flag2 = true;
					}
					ValidateFullTerm(ref Term);
					if (string.IsNullOrEmpty(Term))
					{
						continue;
					}
					TermData termData2 = GetTermData(Term);
					if (termData2 == null)
					{
						termData2 = new TermData();
						termData2.Term = Term;
						termData2.Languages = new string[mLanguages.Count];
						termData2.Languages_Touch = new string[mLanguages.Count];
						termData2.Flags = new byte[mLanguages.Count];
						for (int l = 0; l < mLanguages.Count; l++)
						{
							termData2.Languages[l] = (termData2.Languages_Touch[l] = string.Empty);
						}
						mTerms.Add(termData2);
						mDictionary.Add(Term, termData2);
					}
					else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
					{
						continue;
					}
					if (num2 > 0)
					{
						termData2.TermType = GetTermType(array[num2]);
					}
					if (num3 > 0)
					{
						termData2.Description = array[num3];
					}
					for (int m = 0; m < array2.Length && m < array.Length - num; m++)
					{
						if (string.IsNullOrEmpty(array[m + num]))
						{
							continue;
						}
						int num6 = array2[m];
						if (num6 >= 0)
						{
							string text2 = array[m + num];
							if (flag2)
							{
								termData2.Languages_Touch[num6] = text2;
								termData2.Flags[num6] &= 253;
							}
							else
							{
								termData2.Languages[num6] = text2;
								termData2.Flags[num6] &= 254;
							}
						}
					}
				}
				return string.Empty;
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
		}

		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			for (int num = texts.Length; i < num; i++)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		public static eTermType GetTermType(string type)
		{
			int i = 0;
			for (int num = 8; i <= num; i++)
			{
				eTermType eTermType = (eTermType)i;
				if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
			}
			return eTermType.Text;
		}

		public static void FreeUnusedLanguages()
		{
			LanguageSource languageSource = LocalizationManager.Sources[0];
			int languageIndex = languageSource.GetLanguageIndex(LocalizationManager.CurrentLanguage);
			for (int i = 0; i < languageSource.mTerms.Count; i++)
			{
				TermData termData = languageSource.mTerms[i];
				for (int j = 0; j < termData.Languages.Length; j++)
				{
					if (j != languageIndex)
					{
						termData.Languages[j] = (termData.Languages_Touch[j] = null);
					}
				}
			}
		}

		public void Import_Google_FromCache()
		{
			if (GoogleUpdateFrequency == eGoogleUpdateFrequency.Never || !LocalizationManager.IsPlaying())
			{
				return;
			}
			string sourcePlayerPrefName = GetSourcePlayerPrefName();
			string text = PlayerPrefs.GetString("I2Source_" + sourcePlayerPrefName, null);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("[i2e]", StringComparison.Ordinal))
			{
				text = StringObfucator.Decode(text.Substring(5, text.Length - 5));
			}
			bool flag = false;
			string text2 = Google_LastUpdatedVersion;
			if (PlayerPrefs.HasKey("I2SourceVersion_" + sourcePlayerPrefName))
			{
				text2 = PlayerPrefs.GetString("I2SourceVersion_" + sourcePlayerPrefName, Google_LastUpdatedVersion);
				flag = IsNewerVersion(Google_LastUpdatedVersion, text2);
			}
			if (!flag)
			{
				PlayerPrefs.DeleteKey("I2Source_" + sourcePlayerPrefName);
				PlayerPrefs.DeleteKey("I2SourceVersion_" + sourcePlayerPrefName);
				return;
			}
			if (text2.Length > 19)
			{
				text2 = string.Empty;
			}
			Google_LastUpdatedVersion = text2;
			UnityEngine.Debug.Log("[I2Loc] Using Saved (PlayerPref) data in 'I2Source_" + sourcePlayerPrefName + "'");
			Import_Google_Result(text, eSpreadsheetUpdateMode.Replace);
		}

		private bool IsNewerVersion(string currentVersion, string newVersion)
		{
			if (string.IsNullOrEmpty(newVersion))
			{
				return false;
			}
			if (string.IsNullOrEmpty(currentVersion))
			{
				return true;
			}
			if (!long.TryParse(newVersion, out long result) || !long.TryParse(currentVersion, out long result2))
			{
				return true;
			}
			return result > result2;
		}

		public void Import_Google(bool ForceUpdate = false)
		{
			if (ForceUpdate || GoogleUpdateFrequency != eGoogleUpdateFrequency.Never)
			{
				string sourcePlayerPrefName = GetSourcePlayerPrefName();
				if (!ForceUpdate && GoogleUpdateFrequency != 0)
				{
					string @string = PlayerPrefs.GetString("LastGoogleUpdate_" + sourcePlayerPrefName, string.Empty);
					try
					{
						if (DateTime.TryParse(@string, out DateTime result))
						{
							double totalDays = (DateTime.Now - result).TotalDays;
							switch (GoogleUpdateFrequency)
							{
							case eGoogleUpdateFrequency.OnlyOnce:
								return;
							case eGoogleUpdateFrequency.Daily:
								if (totalDays < 1.0)
								{
									return;
								}
								break;
							case eGoogleUpdateFrequency.Weekly:
								if (totalDays < 8.0)
								{
									return;
								}
								break;
							case eGoogleUpdateFrequency.Monthly:
								if (totalDays < 31.0)
								{
									return;
								}
								break;
							}
						}
					}
					catch (Exception)
					{
					}
				}
				PlayerPrefs.SetString("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
				CoroutineManager.pInstance.StartCoroutine(Import_Google_Coroutine());
			}
		}

		private string GetSourcePlayerPrefName()
		{
			if (Array.IndexOf(LocalizationManager.GlobalSources, base.name) >= 0)
			{
				return base.name;
			}
			return SceneManager.GetActiveScene().name + "_" + base.name;
		}

		private IEnumerator Import_Google_Coroutine()
		{
			WWW www = Import_Google_CreateWWWcall();
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			bool notError = string.IsNullOrEmpty(www.error);
			string wwwText = null;
			if (notError)
			{
				byte[] bytes = www.bytes;
				wwwText = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			}
			if (notError && !string.IsNullOrEmpty(wwwText) && wwwText != "\"\"")
			{
				string value = Import_Google_Result(wwwText, eSpreadsheetUpdateMode.Replace, saveInPlayerPrefs: true);
				if (string.IsNullOrEmpty(value))
				{
					if (this.Event_OnSourceUpdateFromGoogle != null)
					{
						this.Event_OnSourceUpdateFromGoogle(this, true, www.error);
					}
					LocalizationManager.LocalizeAll(Force: true);
					UnityEngine.Debug.Log("Done Google Sync");
				}
				else
				{
					if (this.Event_OnSourceUpdateFromGoogle != null)
					{
						this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
					}
					UnityEngine.Debug.Log("Done Google Sync: source was up-to-date");
				}
			}
			else
			{
				if (this.Event_OnSourceUpdateFromGoogle != null)
				{
					this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
				}
				UnityEngine.Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			}
		}

		public WWW Import_Google_CreateWWWcall(bool ForceUpdate = false)
		{
			if (!HasGoogleSpreadsheet())
			{
				return null;
			}
			string text = PlayerPrefs.GetString("I2SourceVersion_" + GetSourcePlayerPrefName(), Google_LastUpdatedVersion);
			if (text.Length > 19)
			{
				text = string.Empty;
			}
			if (IsNewerVersion(text, Google_LastUpdatedVersion))
			{
				Google_LastUpdatedVersion = text;
			}
			string url = string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", LocalizationManager.GetWebServiceURL(this), Google_SpreadsheetKey, (!ForceUpdate) ? Google_LastUpdatedVersion : "0");
			return new WWW(url);
		}

		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this)) && !string.IsNullOrEmpty(Google_SpreadsheetKey) && !string.IsNullOrEmpty(Google_SpreadsheetName);
		}

		public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode, bool saveInPlayerPrefs = false)
		{
			try
			{
				string empty = string.Empty;
				if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
				{
					return empty;
				}
				int num = JsonString.IndexOf("version=", StringComparison.Ordinal);
				int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
				if (num < 0 || num2 < 0)
				{
					return "Invalid Response from Google, Most likely the WebService needs to be updated";
				}
				num += "version=".Length;
				num2 += "script_version=".Length;
				string text = JsonString.Substring(num, JsonString.IndexOf(",", num, StringComparison.Ordinal) - num);
				int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2, StringComparison.Ordinal) - num2));
				if (text.Length > 19)
				{
					text = string.Empty;
				}
				if (num3 != LocalizationManager.GetRequiredWebServiceVersion())
				{
					return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
				}
				if (saveInPlayerPrefs && !IsNewerVersion(Google_LastUpdatedVersion, text))
				{
					return "LanguageSource is up-to-date";
				}
				if (saveInPlayerPrefs)
				{
					string sourcePlayerPrefName = GetSourcePlayerPrefName();
					PlayerPrefs.SetString("I2Source_" + sourcePlayerPrefName, "[i2e]" + StringObfucator.Encode(JsonString));
					PlayerPrefs.SetString("I2SourceVersion_" + sourcePlayerPrefName, text);
					PlayerPrefs.Save();
				}
				Google_LastUpdatedVersion = text;
				if (UpdateMode == eSpreadsheetUpdateMode.Replace)
				{
					ClearAllData();
				}
				int num4 = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
				while (num4 > 0)
				{
					num4 += "[i2category]".Length;
					int num5 = JsonString.IndexOf("[/i2category]", num4, StringComparison.Ordinal);
					string category = JsonString.Substring(num4, num5 - num4);
					num5 += "[/i2category]".Length;
					int num6 = JsonString.IndexOf("[/i2csv]", num5, StringComparison.Ordinal);
					string i2CSVstring = JsonString.Substring(num5, num6 - num5);
					num4 = JsonString.IndexOf("[i2category]", num6, StringComparison.Ordinal);
					Import_I2CSV(category, i2CSVstring, UpdateMode);
					if (UpdateMode == eSpreadsheetUpdateMode.Replace)
					{
						UpdateMode = eSpreadsheetUpdateMode.Merge;
					}
				}
				return empty;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(ex);
				return ex.ToString();
			}
		}

		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
			{
				Categories = new List<string>();
			}
			foreach (TermData mTerm in mTerms)
			{
				string categoryFromFullTerm = GetCategoryFromFullTerm(mTerm.Term, OnlyMainCategory);
				if (!Categories.Contains(categoryFromFullTerm))
				{
					Categories.Add(categoryFromFullTerm);
				}
			}
			Categories.Sort();
			return Categories;
		}

		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
			return (num >= 0) ? FullTerm.Substring(num + 1) : FullTerm;
		}

		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
			return (num >= 0) ? FullTerm.Substring(0, num) : EmptyCategory;
		}

		public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
			if (num < 0)
			{
				Category = EmptyCategory;
				Key = FullTerm;
			}
			else
			{
				Category = FullTerm.Substring(0, num);
				Key = FullTerm.Substring(num + 1);
			}
		}

		public static eInputSpecialization GetCurrentInputType()
		{
			return eInputSpecialization.Touch;
		}

		private void Awake()
		{
			if (NeverDestroy)
			{
				if (ManagerHasASimilarSource())
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				}
			}
			LocalizationManager.AddSource(this);
			UpdateDictionary();
		}

		public void UpdateDictionary(bool force = false)
		{
			if (!force && mDictionary != null && mDictionary.Count == mTerms.Count)
			{
				return;
			}
			StringComparer stringComparer = (!CaseInsensitiveTerms) ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
			if (mDictionary.Comparer != stringComparer)
			{
				mDictionary = new Dictionary<string, TermData>(stringComparer);
			}
			else
			{
				mDictionary.Clear();
			}
			int i = 0;
			for (int count = mTerms.Count; i < count; i++)
			{
				ValidateFullTerm(ref mTerms[i].Term);
				if (mTerms[i].Languages_Touch == null || mTerms[i].Languages_Touch.Length != mTerms[i].Languages.Length)
				{
					mTerms[i].Languages_Touch = new string[mTerms[i].Languages.Length];
				}
				mDictionary[mTerms[i].Term] = mTerms[i];
				mTerms[i].Validate();
			}
		}

		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while ((bool)parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			return text;
		}

		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
		{
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if ((!SkipDisabled || mLanguages[i].IsEnabled()) && string.Compare(mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			if (AllowDiscartingRegion)
			{
				int num = -1;
				int num2 = 0;
				int j = 0;
				for (int count2 = mLanguages.Count; j < count2; j++)
				{
					if (!SkipDisabled || mLanguages[j].IsEnabled())
					{
						int commonWordInLanguageNames = GetCommonWordInLanguageNames(mLanguages[j].Name, language);
						if (commonWordInLanguageNames > num2)
						{
							num2 = commonWordInLanguageNames;
							num = j;
						}
					}
				}
				if (num >= 0)
				{
					return num;
				}
			}
			return -1;
		}

		public int GetLanguageIndexFromCode(string Code, bool exactMatch = true)
		{
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (string.Compare(mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
			}
			if (!exactMatch)
			{
				int j = 0;
				for (int count2 = mLanguages.Count; j < count2; j++)
				{
					if (string.Compare(mLanguages[j].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return j;
					}
				}
			}
			return -1;
		}

		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
			{
				return 0;
			}
			char[] separator = "( )-/\\".ToCharArray();
			string[] array = Language1.Split(separator);
			string[] array2 = Language2.Split(separator);
			int num = 0;
			string[] array3 = array;
			foreach (string value in array3)
			{
				if (!string.IsNullOrEmpty(value) && array2.Contains(value))
				{
					num++;
				}
			}
			string[] array4 = array2;
			foreach (string value2 in array4)
			{
				if (!string.IsNullOrEmpty(value2) && array.Contains(value2))
				{
					num++;
				}
			}
			return num;
		}

		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = GetLanguageWithoutRegion(Language1);
			Language2 = GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (GetLanguageIndex(LanguageName, AllowDiscartingRegion: false) < 0)
			{
				LanguageData languageData = new LanguageData();
				languageData.Name = LanguageName;
				languageData.Code = LanguageCode;
				mLanguages.Add(languageData);
				int count = mLanguages.Count;
				int i = 0;
				for (int count2 = mTerms.Count; i < count2; i++)
				{
					Array.Resize(ref mTerms[i].Languages, count);
					Array.Resize(ref mTerms[i].Languages_Touch, count);
					Array.Resize(ref mTerms[i].Flags, count);
				}
			}
		}

		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = GetLanguageIndex(LanguageName);
			if (languageIndex < 0)
			{
				return;
			}
			int count = mLanguages.Count;
			int i = 0;
			for (int count2 = mTerms.Count; i < count2; i++)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					mTerms[i].Languages[j - 1] = mTerms[i].Languages[j];
					mTerms[i].Languages_Touch[j - 1] = mTerms[i].Languages_Touch[j];
					mTerms[i].Flags[j - 1] = mTerms[i].Flags[j];
				}
				Array.Resize(ref mTerms[i].Languages, count - 1);
				Array.Resize(ref mTerms[i].Languages_Touch, count - 1);
				Array.Resize(ref mTerms[i].Flags, count - 1);
			}
			mLanguages.RemoveAt(languageIndex);
		}

		public List<string> GetLanguages(bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (!skipDisabled || mLanguages[i].IsEnabled())
				{
					list.Add(mLanguages[i].Name);
				}
			}
			return list;
		}

		public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (!skipDisabled || mLanguages[i].IsEnabled())
				{
					string text = mLanguages[i].Code;
					if (!allowRegions && text != null && text.Length > 2)
					{
						text = text.Substring(0, 2);
					}
					if (!string.IsNullOrEmpty(text) && !list.Contains(text))
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		public bool IsLanguageEnabled(string Language)
		{
			int languageIndex = GetLanguageIndex(Language, AllowDiscartingRegion: false);
			return languageIndex >= 0 && mLanguages[languageIndex].IsEnabled();
		}

		public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages)
		{
			int count = mTerms.Count;
			int count2 = mLanguages.Count;
			bool flag = TermData.IsTouchType();
			if (UnloadOtherLanguages)
			{
				for (int i = 0; i < count2; i++)
				{
					if (i != languageIndex && mLanguages[i].IsLoaded() && mLanguages[i].CanBeUnloaded())
					{
						mLanguages[i].SetLoaded(loaded: false);
						for (int j = 0; j < count; j++)
						{
							mTerms[j].Languages[i] = (mTerms[j].Languages_Touch[i] = null);
						}
					}
				}
			}
			if (mLanguages[languageIndex].IsLoaded())
			{
				return;
			}
			string sourcePlayerPrefName = GetSourcePlayerPrefName();
			TextAsset textAsset = Resources.Load<TextAsset>(sourcePlayerPrefName);
			if (textAsset == null)
			{
				return;
			}
			string[] separator = new string[1]
			{
				"[$i2$]"
			};
			string[] array = textAsset.text.Split(separator, StringSplitOptions.None);
			for (int k = 0; k < array.Length; k += 2)
			{
				string key = array[k];
				string text = array[k + 1];
				if (mDictionary.TryGetValue(key, out TermData value))
				{
					string[] array2 = (!flag) ? value.Languages : value.Languages_Touch;
					array2[languageIndex] = text;
				}
			}
			mLanguages[languageIndex].SetLoaded(loaded: true);
		}

		public string GetTranslation(string term)
		{
			if (TryGetTranslation(term, out string Translation))
			{
				return Translation;
			}
			return string.Empty;
		}

		public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null)
		{
			int languageIndex = GetLanguageIndex((overrideLanguage != null) ? overrideLanguage : LocalizationManager.CurrentLanguage, AllowDiscartingRegion: true, SkipDisabled: false);
			if (languageIndex >= 0)
			{
				TermData termData = GetTermData(term);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex);
					if (Translation == "---")
					{
						Translation = string.Empty;
						return true;
					}
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
					Translation = null;
				}
				if (OnMissingTranslation == MissingTranslationAction.ShowWarning)
				{
					Translation = $"<!-Missing Translation [{term}]-!>";
					return false;
				}
				if (OnMissingTranslation == MissingTranslationAction.Fallback && termData != null)
				{
					for (int i = 0; i < mLanguages.Count; i++)
					{
						if (i != languageIndex && mLanguages[i].IsEnabled())
						{
							Translation = termData.GetTranslation(i);
							if (!string.IsNullOrEmpty(Translation))
							{
								return true;
							}
						}
					}
				}
			}
			Translation = null;
			return false;
		}

		public TermData AddTerm(string term)
		{
			return AddTerm(term, eTermType.Text);
		}

		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			if (mDictionary.Count == 0)
			{
				UpdateDictionary();
			}
			if (mDictionary.TryGetValue(term, out TermData value))
			{
				return value;
			}
			TermData termData = null;
			if (allowCategoryMistmatch)
			{
				string keyFromFullTerm = GetKeyFromFullTerm(term);
				{
					foreach (KeyValuePair<string, TermData> item in mDictionary)
					{
						if (item.Value.IsTerm(keyFromFullTerm, allowCategoryMistmatch: true))
						{
							if (termData != null)
							{
								return null;
							}
							termData = item.Value;
						}
					}
					return termData;
				}
			}
			return termData;
		}

		public bool ContainsTerm(string term)
		{
			return GetTermData(term) != null;
		}

		public List<string> GetTermsList(string Category = null)
		{
			if (mDictionary.Count != mTerms.Count)
			{
				UpdateDictionary();
			}
			if (string.IsNullOrEmpty(Category))
			{
				return new List<string>(mDictionary.Keys);
			}
			List<string> list = new List<string>();
			for (int i = 0; i < mTerms.Count; i++)
			{
				TermData termData = mTerms[i];
				if (GetCategoryFromFullTerm(termData.Term) == Category)
				{
					list.Add(termData.Term);
				}
			}
			return list;
		}

		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (mLanguages.Count == 0)
			{
				AddLanguage("English", "en");
			}
			TermData termData = GetTermData(NewTerm);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[mLanguages.Count];
				termData.Languages_Touch = new string[mLanguages.Count];
				termData.Flags = new byte[mLanguages.Count];
				mTerms.Add(termData);
				mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		public void RemoveTerm(string term)
		{
			int num = 0;
			int count = mTerms.Count;
			while (true)
			{
				if (num < count)
				{
					if (mTerms[num].Term == term)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			mTerms.RemoveAt(num);
			mDictionary.Remove(term);
		}

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(EmptyCategory, StringComparison.Ordinal) && Term.Length > EmptyCategory.Length && Term[EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(EmptyCategory.Length + 1);
			}
			Term = LocalizationManager.RemoveNonASCII(Term, allowCategory: true);
		}

		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (Source.GetLanguageIndex(mLanguages[i].Name) < 0)
				{
					return false;
				}
			}
			if (Source.mTerms.Count != mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < mTerms.Count; j++)
			{
				if (Source.GetTermData(mTerms[j].Term) == null)
				{
					return false;
				}
			}
			return true;
		}

		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			for (int count = LocalizationManager.Sources.Count; i < count; i++)
			{
				LanguageSource languageSource = LocalizationManager.Sources[i];
				if (languageSource != null && languageSource.IsEqualTo(this) && languageSource != this)
				{
					return true;
				}
			}
			return false;
		}

		public void ClearAllData()
		{
			mTerms.Clear();
			mLanguages.Clear();
			mDictionary.Clear();
		}

		public UnityEngine.Object FindAsset(string Name)
		{
			if (Assets != null)
			{
				int i = 0;
				for (int num = Assets.Length; i < num; i++)
				{
					if (Assets[i] != null && Name.EndsWith(Assets[i].name, StringComparison.OrdinalIgnoreCase))
					{
						return Assets[i];
					}
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			return Array.IndexOf(Assets, Obj) >= 0;
		}

		public void AddAsset(UnityEngine.Object Obj)
		{
			Array.Resize(ref Assets, Assets.Length + 1);
			Assets[Assets.Length - 1] = Obj;
		}
	}
}
