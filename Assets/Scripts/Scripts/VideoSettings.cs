using System;

namespace Axlebolt.Standoff.Settings.Video
{
	[Serializable]
	public struct VideoSettings
	{
		public QualityLvl ShaderDetail;

		public QualityLvl ModelDetail;

		public QualityLvl TextureDetail;

		public QualityLvl EffectDetail;
	}
}
