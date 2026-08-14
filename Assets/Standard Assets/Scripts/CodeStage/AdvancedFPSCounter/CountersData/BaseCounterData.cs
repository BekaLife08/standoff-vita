using CodeStage.AdvancedFPSCounter.Labels;
using System;
using System.Text;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	[Serializable]
	[AddComponentMenu("")]
	public abstract class BaseCounterData
	{
		protected const string BOLD_START = "<b>";

		protected const string BOLD_END = "</b>";

		protected const string ITALIC_START = "<i>";

		protected const string ITALIC_END = "</i>";

		internal StringBuilder text;

		internal bool dirty;

		protected AFPSCounter main;

		protected string colorCached;

		protected bool inited;

		[SerializeField]
		protected bool enabled = true;

		[Tooltip("Current anchoring label for the counter output.\nRefreshes both previous and specified label when switching anchor.")]
		[SerializeField]
		protected LabelAnchor anchor;

		[Tooltip("Regular color of the counter output.")]
		[SerializeField]
		protected Color color;

		[Tooltip("Controls text style.")]
		[SerializeField]
		protected FontStyle style;

		[Tooltip("Additional text to append to the end of the counter in normal Operation Mode.")]
		protected string extraText;

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				if (enabled != value && Application.isPlaying)
				{
					enabled = value;
					if (enabled)
					{
						Activate();
					}
					else
					{
						Deactivate();
					}
					main.UpdateTexts();
				}
			}
		}

		public LabelAnchor Anchor
		{
			get
			{
				return anchor;
			}
			set
			{
				if (anchor != value && Application.isPlaying)
				{
					LabelAnchor labelAnchor = anchor;
					anchor = value;
					if (enabled)
					{
						dirty = true;
						main.MakeDrawableLabelDirty(labelAnchor);
						main.UpdateTexts();
					}
				}
			}
		}

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				if (!(color == value) && Application.isPlaying)
				{
					color = value;
					if (enabled)
					{
						CacheCurrentColor();
						Refresh();
					}
				}
			}
		}

		public FontStyle Style
		{
			get
			{
				return style;
			}
			set
			{
				if (style != value && Application.isPlaying)
				{
					style = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public string ExtraText
		{
			get
			{
				return extraText;
			}
			set
			{
				if (!(extraText == value) && Application.isPlaying)
				{
					extraText = value;
					if (enabled)
					{
						Refresh();
					}
				}
			}
		}

		public void Refresh()
		{
			if (enabled && Application.isPlaying)
			{
				UpdateValue(force: true);
				main.UpdateTexts();
			}
		}

		internal virtual void UpdateValue()
		{
			UpdateValue(force: false);
		}

		internal abstract void UpdateValue(bool force);

		internal void Init(AFPSCounter reference)
		{
			main = reference;
		}

		internal void Dispose()
		{
			main = null;
			if (text != null)
			{
				text.Remove(0, text.Length);
				text = null;
			}
		}

		internal virtual void Activate()
		{
			if (enabled && main.OperationMode != 0 && HasData())
			{
				if (text == null)
				{
					text = new StringBuilder(500);
				}
				else
				{
					text.Length = 0;
				}
				if (main.OperationMode == OperationMode.Normal && colorCached == null)
				{
					CacheCurrentColor();
				}
				PerformActivationActions();
				if (!inited)
				{
					PerformInitActions();
					inited = true;
				}
				UpdateValue();
			}
		}

		internal virtual void Deactivate()
		{
			if (inited)
			{
				if (text != null)
				{
					text.Remove(0, text.Length);
				}
				main.MakeDrawableLabelDirty(anchor);
				PerformDeActivationActions();
				inited = false;
			}
		}

		protected virtual void PerformInitActions()
		{
		}

		protected virtual void PerformActivationActions()
		{
		}

		protected virtual void PerformDeActivationActions()
		{
		}

		protected abstract bool HasData();

		protected abstract void CacheCurrentColor();

		protected void ApplyTextStyles()
		{
			if (text.Length > 0)
			{
				switch (style)
				{
				case FontStyle.Bold:
					text.Insert(0, "<b>");
					text.Append("</b>");
					break;
				case FontStyle.Italic:
					text.Insert(0, "<i>");
					text.Append("</i>");
					break;
				case FontStyle.BoldAndItalic:
					text.Insert(0, "<b>");
					text.Append("</b>");
					text.Insert(0, "<i>");
					text.Append("</i>");
					break;
				default:
					throw new ArgumentOutOfRangeException();
				case FontStyle.Normal:
					break;
				}
			}
			if (!string.IsNullOrEmpty(extraText))
			{
				text.Append('\n').Append(extraText);
			}
		}
	}
}
