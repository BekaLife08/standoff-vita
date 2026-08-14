using CodeStage.AdvancedFPSCounter.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	[Serializable]
	[AddComponentMenu("")]
	public class FPSCounterData : UpdatebleCounterData
	{
		private const string COLOR_TEXT_START = "<color=#{0}>";

		private const string COLOR_TEXT_END = "</color>";

		private const string FPS_TEXT_START = "<color=#{0}>FPS: ";

		private const string MS_TEXT_START = " <color=#{0}>[";

		private const string MS_TEXT_END = " MS]</color>";

		private const string MIN_TEXT_START = "<color=#{0}>MIN: ";

		private const string MAX_TEXT_START = "<color=#{0}>MAX: ";

		private const string AVG_TEXT_START = "<color=#{0}>AVG: ";

		private const string RENDER_TEXT_START = "<color=#{0}>REN: ";

		public int warningLevelValue = 50;

		public int criticalLevelValue = 20;

		[Tooltip("Average FPS counter accumulative data will be reset on new scene load if enabled.")]
		public bool resetAverageOnNewScene;

		[Tooltip("Minimum and maximum FPS readouts will be reset on new scene load if enabled")]
		public bool resetMinMaxOnNewScene;

		[Tooltip("Amount of update intervals to skip before recording minimum and maximum FPS.\nUse it to skip initialization performance spikes and drops.")]
		[Range(0f, 10f)]
		public int minMaxIntervalsToSkip = 3;

		internal float newValue;

		private string colorCachedMs;

		private string colorCachedMin;

		private string colorCachedMax;

		private string colorCachedAvg;

		private string colorCachedRender;

		private string colorWarningCached;

		private string colorWarningCachedMs;

		private string colorWarningCachedMin;

		private string colorWarningCachedMax;

		private string colorWarningCachedAvg;

		private string colorCriticalCached;

		private string colorCriticalCachedMs;

		private string colorCriticalCachedMin;

		private string colorCriticalCachedMax;

		private string colorCriticalCachedAvg;

		private int currentAverageSamples;

		private float currentAverageRaw;

		private float[] accumulatedAverageSamples;

		private int minMaxIntervalsSkipped;

		private float renderTimeBank;

		private int previousFrameCount;

		[Tooltip("Shows time in milliseconds spent to render 1 frame.")]
		[SerializeField]
		private bool milliseconds;

		[Tooltip("Shows Average FPS calculated from specified Samples amount or since game / scene start, depending on Samples value and 'Reset On Load' toggle.")]
		[SerializeField]
		private bool average;

		[Tooltip("Shows time in milliseconds for the average FPS.")]
		[SerializeField]
		private bool averageMilliseconds;

		[Tooltip("Controls placing Average on the new line.")]
		[SerializeField]
		private bool averageNewLine;

		[Tooltip("Amount of last samples to get average from. Set 0 to get average from all samples since startup or level load.\nOne Sample recorded per one Interval.")]
		[Range(0f, 100f)]
		[SerializeField]
		private int averageSamples = 50;

		[Tooltip("Shows minimum and maximum FPS readouts since game / scene start, depending on 'Reset On Load' toggle.")]
		[SerializeField]
		private bool minMax;

		[Tooltip("Shows time in milliseconds for the Min Max FPS.")]
		[SerializeField]
		private bool minMaxMilliseconds;

		[Tooltip("Controls placing Min Max on the new line.")]
		[SerializeField]
		private bool minMaxNewLine;

		[Tooltip("Check to place Min Max on two separate lines. Otherwise they will be placed on a single line.")]
		[SerializeField]
		private bool minMaxTwoLines;

		[Tooltip("Shows time spent on Camera.Render excluding Image Effects. Add AFPSRenderRecorder to the cameras you wish to count.")]
		[SerializeField]
		private bool render;

		[Tooltip("Controls placing Render on the new line.")]
		[SerializeField]
		private bool renderNewLine;

		[Tooltip("Check to automatically add AFPSRenderRecorder to the Main Camera if present.")]
		[SerializeField]
		private bool renderAutoAdd = true;

		[Tooltip("Color of the FPS counter while FPS is between Critical and Warning levels.")]
		[SerializeField]
		private Color colorWarning = new Color32(236, 224, 88, byte.MaxValue);

		[Tooltip("Color of the FPS counter while FPS is below Critical level.")]
		[SerializeField]
		private Color colorCritical = new Color32(249, 91, 91, byte.MaxValue);

		[Tooltip("Color of the Render Time output.")]
		[SerializeField]
		protected Color colorRender;

		public bool Milliseconds
		{
			get
			{
				return milliseconds;
			}
			set
			{
				if (milliseconds != value && Application.isPlaying)
				{
					milliseconds = value;
					if (!milliseconds)
					{
						LastMillisecondsValue = 0f;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool Average
		{
			get
			{
				return average;
			}
			set
			{
				if (average != value && Application.isPlaying)
				{
					average = value;
					if (!average)
					{
						ResetAverage();
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool AverageMilliseconds
		{
			get
			{
				return averageMilliseconds;
			}
			set
			{
				if (averageMilliseconds != value && Application.isPlaying)
				{
					averageMilliseconds = value;
					if (!averageMilliseconds)
					{
						LastAverageMillisecondsValue = 0f;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool AverageNewLine
		{
			get
			{
				return averageNewLine;
			}
			set
			{
				if (averageNewLine != value && Application.isPlaying)
				{
					averageNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public int AverageSamples
		{
			get
			{
				return averageSamples;
			}
			set
			{
				if (averageSamples == value || !Application.isPlaying)
				{
					return;
				}
				averageSamples = value;
				if (!enabled)
				{
					return;
				}
				if (averageSamples > 0)
				{
					if (accumulatedAverageSamples == null)
					{
						accumulatedAverageSamples = new float[averageSamples];
					}
					else if (accumulatedAverageSamples.Length != averageSamples)
					{
						Array.Resize(ref accumulatedAverageSamples, averageSamples);
					}
				}
				else
				{
					accumulatedAverageSamples = null;
				}
				ResetAverage();
				Refresh();
			}
		}

		public bool MinMax
		{
			get
			{
				return minMax;
			}
			set
			{
				if (minMax != value && Application.isPlaying)
				{
					minMax = value;
					if (!minMax)
					{
						ResetMinMax();
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool MinMaxMilliseconds
		{
			get
			{
				return minMaxMilliseconds;
			}
			set
			{
				if (minMaxMilliseconds != value && Application.isPlaying)
				{
					minMaxMilliseconds = value;
					if (!minMaxMilliseconds)
					{
						LastMinMillisecondsValue = 0f;
						LastMaxMillisecondsValue = 0f;
					}
					else
					{
						LastMinMillisecondsValue = 1000f / (float)LastMinimumValue;
						LastMaxMillisecondsValue = 1000f / (float)LastMaximumValue;
					}
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool MinMaxNewLine
		{
			get
			{
				return minMaxNewLine;
			}
			set
			{
				if (minMaxNewLine != value && Application.isPlaying)
				{
					minMaxNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool MinMaxTwoLines
		{
			get
			{
				return minMaxTwoLines;
			}
			set
			{
				if (minMaxTwoLines != value && Application.isPlaying)
				{
					minMaxTwoLines = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool Render
		{
			get
			{
				return render;
			}
			set
			{
				if (render == value || !Application.isPlaying)
				{
					return;
				}
				render = value;
				if (!render)
				{
					if (renderAutoAdd)
					{
						TryToRemoveRenderRecorder();
					}
					return;
				}
				previousFrameCount = Time.frameCount;
				if (renderAutoAdd)
				{
					TryToAddRenderRecorder();
				}
				Refresh();
			}
		}

		public bool RenderNewLine
		{
			get
			{
				return renderNewLine;
			}
			set
			{
				if (renderNewLine != value && Application.isPlaying)
				{
					renderNewLine = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public bool RenderAutoAdd
		{
			get
			{
				return renderAutoAdd;
			}
			set
			{
				if (renderAutoAdd != value && Application.isPlaying)
				{
					renderAutoAdd = value;
					if (enabled)
					{
						TryToAddRenderRecorder();
						Refresh();
					}
				}
			}
		}

		public Color ColorWarning
		{
			get
			{
				return colorWarning;
			}
			set
			{
				if (!(colorWarning == value) && Application.isPlaying)
				{
					colorWarning = value;
					if (enabled)
					{
						CacheWarningColor();
						Refresh();
					}
				}
			}
		}

		public Color ColorCritical
		{
			get
			{
				return colorCritical;
			}
			set
			{
				if (!(colorCritical == value) && Application.isPlaying)
				{
					colorCritical = value;
					if (enabled)
					{
						CacheCriticalColor();
						Refresh();
					}
				}
			}
		}

		public Color ColorRender
		{
			get
			{
				return colorRender;
			}
			set
			{
				if (!(colorRender == value) && Application.isPlaying)
				{
					colorRender = value;
					if (enabled)
					{
						CacheCurrentColor();
						Refresh();
					}
				}
			}
		}

		public int LastValue
		{
			get;
			private set;
		}

		public float LastMillisecondsValue
		{
			get;
			private set;
		}

		public float LastRenderValue
		{
			get;
			private set;
		}

		public int LastAverageValue
		{
			get;
			private set;
		}

		public float LastAverageMillisecondsValue
		{
			get;
			private set;
		}

		public int LastMinimumValue
		{
			get;
			private set;
		}

		public int LastMaximumValue
		{
			get;
			private set;
		}

		public float LastMinMillisecondsValue
		{
			get;
			private set;
		}

		public float LastMaxMillisecondsValue
		{
			get;
			private set;
		}

		public FPSLevel CurrentFpsLevel
		{
			get;
			private set;
		}

		public event Action<FPSLevel> OnFPSLevelChange;

		internal FPSCounterData()
		{
			color = new Color32(85, 218, 102, byte.MaxValue);
			colorRender = new Color32(167, 110, 209, byte.MaxValue);
			style = FontStyle.Bold;
			milliseconds = true;
			render = false;
			renderNewLine = true;
			average = true;
			averageMilliseconds = true;
			averageNewLine = true;
			resetAverageOnNewScene = true;
			minMax = true;
			minMaxNewLine = true;
			resetMinMaxOnNewScene = true;
		}

		public void ResetAverage()
		{
			if (Application.isPlaying)
			{
				LastAverageValue = 0;
				currentAverageSamples = 0;
				currentAverageRaw = 0f;
				if (averageSamples > 0 && accumulatedAverageSamples != null)
				{
					Array.Clear(accumulatedAverageSamples, 0, accumulatedAverageSamples.Length);
				}
			}
		}

		public void ResetMinMax(bool withoutUpdate = false)
		{
			if (Application.isPlaying)
			{
				LastMinimumValue = -1;
				LastMaximumValue = -1;
				minMaxIntervalsSkipped = 0;
				UpdateValue(force: true);
				dirty = true;
			}
		}

		internal void OnLevelLoadedCallback()
		{
			if (minMax && resetMinMaxOnNewScene)
			{
				ResetMinMax();
			}
			if (average && resetAverageOnNewScene)
			{
				ResetAverage();
			}
			if (render && renderAutoAdd)
			{
				TryToAddRenderRecorder();
			}
		}

		internal void AddRenderTime(float time)
		{
			if (enabled && inited)
			{
				renderTimeBank += time;
			}
		}

		internal override void UpdateValue(bool force)
		{
			if (!enabled)
			{
				return;
			}
			int num = (int)newValue;
			if (LastValue != num || force)
			{
				LastValue = num;
				dirty = true;
			}
			if (LastValue <= criticalLevelValue)
			{
				if (LastValue != 0 && CurrentFpsLevel != FPSLevel.Critical)
				{
					CurrentFpsLevel = FPSLevel.Critical;
					if (this.OnFPSLevelChange != null)
					{
						this.OnFPSLevelChange(CurrentFpsLevel);
					}
				}
			}
			else if (LastValue < warningLevelValue)
			{
				if (LastValue != 0 && CurrentFpsLevel != FPSLevel.Warning)
				{
					CurrentFpsLevel = FPSLevel.Warning;
					if (this.OnFPSLevelChange != null)
					{
						this.OnFPSLevelChange(CurrentFpsLevel);
					}
				}
			}
			else if (LastValue != 0 && CurrentFpsLevel != 0)
			{
				CurrentFpsLevel = FPSLevel.Normal;
				if (this.OnFPSLevelChange != null)
				{
					this.OnFPSLevelChange(CurrentFpsLevel);
				}
			}
			if (dirty && milliseconds)
			{
				LastMillisecondsValue = 1000f / newValue;
			}
			if (render && renderTimeBank > 0f)
			{
				int frameCount = Time.frameCount;
				int num2 = frameCount - previousFrameCount;
				if (num2 == 0)
				{
					num2 = 1;
				}
				float num3 = renderTimeBank / (float)num2;
				if (num3 != LastRenderValue || force)
				{
					LastRenderValue = num3;
					dirty = true;
				}
				previousFrameCount = frameCount;
				renderTimeBank = 0f;
			}
			int num4 = 0;
			if (average)
			{
				if (averageSamples == 0)
				{
					currentAverageSamples++;
					currentAverageRaw += ((float)LastValue - currentAverageRaw) / (float)currentAverageSamples;
				}
				else
				{
					if (accumulatedAverageSamples == null)
					{
						accumulatedAverageSamples = new float[averageSamples];
						ResetAverage();
					}
					accumulatedAverageSamples[currentAverageSamples % averageSamples] = LastValue;
					currentAverageSamples++;
					currentAverageRaw = GetAverageFromAccumulatedSamples();
				}
				num4 = Mathf.RoundToInt(currentAverageRaw);
				if (LastAverageValue != num4 || force)
				{
					LastAverageValue = num4;
					dirty = true;
					if (averageMilliseconds)
					{
						LastAverageMillisecondsValue = 1000f / (float)LastAverageValue;
					}
				}
			}
			if (minMax)
			{
				if (minMaxIntervalsSkipped <= minMaxIntervalsToSkip)
				{
					if (!force)
					{
						minMaxIntervalsSkipped++;
					}
				}
				else if (LastMinimumValue == -1)
				{
					dirty = true;
				}
				if (minMaxIntervalsSkipped > minMaxIntervalsToSkip && dirty)
				{
					if (LastMinimumValue == -1)
					{
						LastMinimumValue = LastValue;
						if (minMaxMilliseconds)
						{
							LastMinMillisecondsValue = 1000f / (float)LastMinimumValue;
						}
					}
					else if (LastValue < LastMinimumValue)
					{
						LastMinimumValue = LastValue;
						if (minMaxMilliseconds)
						{
							LastMinMillisecondsValue = 1000f / (float)LastMinimumValue;
						}
					}
					if (LastMaximumValue == -1)
					{
						LastMaximumValue = LastValue;
						if (minMaxMilliseconds)
						{
							LastMaxMillisecondsValue = 1000f / (float)LastMaximumValue;
						}
					}
					else if (LastValue > LastMaximumValue)
					{
						LastMaximumValue = LastValue;
						if (minMaxMilliseconds)
						{
							LastMaxMillisecondsValue = 1000f / (float)LastMaximumValue;
						}
					}
				}
			}
			if (!dirty || main.OperationMode != OperationMode.Normal)
			{
				return;
			}
			string value = (LastValue >= warningLevelValue) ? colorCached : ((LastValue > criticalLevelValue) ? colorWarningCached : colorCriticalCached);
			text.Length = 0;
			text.Append(value).Append(LastValue).Append("</color>");
			if (milliseconds)
			{
				value = ((LastValue >= warningLevelValue) ? colorCachedMs : ((LastValue > criticalLevelValue) ? colorWarningCachedMs : colorCriticalCachedMs));
				text.Append(value).Append(LastMillisecondsValue.ToString("F")).Append(" MS]</color>");
			}
			if (average)
			{
				text.Append((!averageNewLine) ? ' ' : '\n');
				value = ((num4 >= warningLevelValue) ? colorCachedAvg : ((num4 > criticalLevelValue) ? colorWarningCachedAvg : colorCriticalCachedAvg));
				text.Append(value).Append(num4);
				if (averageMilliseconds)
				{
					text.Append(" [").Append(LastAverageMillisecondsValue.ToString("F")).Append(" MS]");
				}
				text.Append("</color>");
			}
			if (minMax)
			{
				text.Append((!minMaxNewLine) ? ' ' : '\n');
				value = ((LastMinimumValue >= warningLevelValue) ? colorCachedMin : ((LastMinimumValue > criticalLevelValue) ? colorWarningCachedMin : colorCriticalCachedMin));
				text.Append(value).Append(LastMinimumValue);
				if (minMaxMilliseconds)
				{
					text.Append(" [").Append(LastMinMillisecondsValue.ToString("F")).Append(" MS]");
				}
				text.Append("</color>");
				text.Append((!minMaxTwoLines) ? ' ' : '\n');
				value = ((LastMaximumValue >= warningLevelValue) ? colorCachedMax : ((LastMaximumValue > criticalLevelValue) ? colorWarningCachedMax : colorCriticalCachedMax));
				text.Append(value).Append(LastMaximumValue);
				if (minMaxMilliseconds)
				{
					text.Append(" [").Append(LastMaxMillisecondsValue.ToString("F")).Append(" MS]");
				}
				text.Append("</color>");
			}
			if (render)
			{
				text.Append((!renderNewLine) ? ' ' : '\n').Append(colorCachedRender).Append(LastRenderValue.ToString("F"))
					.Append(" MS")
					.Append("</color>");
			}
			ApplyTextStyles();
		}

		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();
			LastValue = 0;
			LastMinimumValue = -1;
			if (render)
			{
				previousFrameCount = Time.frameCount;
				if (renderAutoAdd)
				{
					TryToAddRenderRecorder();
				}
			}
			if (main.OperationMode == OperationMode.Normal)
			{
				if (colorWarningCached == null)
				{
					CacheWarningColor();
				}
				if (colorCriticalCached == null)
				{
					CacheCriticalColor();
				}
				text.Append(colorCriticalCached).Append("0").Append("</color>");
				ApplyTextStyles();
				dirty = true;
			}
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();
			ResetMinMax(withoutUpdate: true);
			ResetAverage();
			LastValue = 0;
			CurrentFpsLevel = FPSLevel.Normal;
		}

		protected override IEnumerator UpdateCounter()
		{
			while (true)
			{
				float previousUpdateTime = Time.unscaledTime;
				int previousUpdateFrames = Time.frameCount;
				cachedWaitForSecondsUnscaled.Reset();
				yield return cachedWaitForSecondsUnscaled;
				float timeElapsed = Time.unscaledTime - previousUpdateTime;
				int framesChanged = Time.frameCount - previousUpdateFrames;
				newValue = (float)framesChanged / timeElapsed;
				UpdateValue(force: false);
				main.UpdateTexts();
			}
		}

		protected override bool HasData()
		{
			return true;
		}

		protected override void CacheCurrentColor()
		{
			string arg = AFPSCounter.Color32ToHex(color);
			colorCached = $"<color=#{arg}>FPS: ";
			colorCachedMs = $" <color=#{arg}>[";
			colorCachedMin = $"<color=#{arg}>MIN: ";
			colorCachedMax = $"<color=#{arg}>MAX: ";
			colorCachedAvg = $"<color=#{arg}>AVG: ";
			string arg2 = AFPSCounter.Color32ToHex(colorRender);
			colorCachedRender = $"<color=#{arg2}>REN: ";
		}

		protected void CacheWarningColor()
		{
			string arg = AFPSCounter.Color32ToHex(colorWarning);
			colorWarningCached = $"<color=#{arg}>FPS: ";
			colorWarningCachedMs = $" <color=#{arg}>[";
			colorWarningCachedMin = $"<color=#{arg}>MIN: ";
			colorWarningCachedMax = $"<color=#{arg}>MAX: ";
			colorWarningCachedAvg = $"<color=#{arg}>AVG: ";
		}

		protected void CacheCriticalColor()
		{
			string arg = AFPSCounter.Color32ToHex(colorCritical);
			colorCriticalCached = $"<color=#{arg}>FPS: ";
			colorCriticalCachedMs = $" <color=#{arg}>[";
			colorCriticalCachedMin = $"<color=#{arg}>MIN: ";
			colorCriticalCachedMax = $"<color=#{arg}>MAX: ";
			colorCriticalCachedAvg = $"<color=#{arg}>AVG: ";
		}

		private float GetAverageFromAccumulatedSamples()
		{
			float num = 0f;
			for (int i = 0; i < averageSamples; i++)
			{
				num += accumulatedAverageSamples[i];
			}
			if (currentAverageSamples < averageSamples)
			{
				return num / (float)currentAverageSamples;
			}
			return num / (float)averageSamples;
		}

		private static void TryToAddRenderRecorder()
		{
			Camera main = Camera.main;
			if (!(main == null) && main.GetComponent<AFPSRenderRecorder>() == null)
			{
				main.gameObject.AddComponent<AFPSRenderRecorder>();
			}
		}

		private static void TryToRemoveRenderRecorder()
		{
			Camera main = Camera.main;
			if (!(main == null))
			{
				AFPSRenderRecorder component = main.GetComponent<AFPSRenderRecorder>();
				if (component != null)
				{
					UnityEngine.Object.Destroy(component);
				}
			}
		}
	}
}
