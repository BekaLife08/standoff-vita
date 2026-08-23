using Axlebolt.Standoff.UI;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Settings.Video
{
	public class QualityLvlField : RadioButtonGroup<QualityLvl>
	{
		[CompilerGenerated]
		private static Func<QualityLvl, string> _003C_003Ef__mg_0024cache0;

		public void SetInterval(QualityLvl from, QualityLvl to)
		{
			if (from > to)
			{
				throw new ArgumentException("From can't be more To");
			}
			SetFormatter(Localize);
			List<QualityLvl> list = new List<QualityLvl>();
			for (QualityLvl qualityLvl = from; qualityLvl <= to; qualityLvl++)
			{
				list.Add(qualityLvl);
			}
			base.Models = list.ToArray();
		}

		private static string Localize(QualityLvl lvl)
		{
			switch (lvl)
			{
			case QualityLvl.VeryHigh:
				return ScriptLocalization.Settings.VeryHigh;
			case QualityLvl.High:
				return ScriptLocalization.Settings.High;
			case QualityLvl.Medium:
				return ScriptLocalization.Settings.Medium;
			case QualityLvl.Low:
				return ScriptLocalization.Settings.Low;
			case QualityLvl.VeryLow:
				return ScriptLocalization.Settings.VeryLow;
			case QualityLvl.Disabled:
				return ScriptLocalization.Settings.Disabled;
			default:
				throw new ArgumentException($"{lvl} not supported");
			}
		}
	}
}
