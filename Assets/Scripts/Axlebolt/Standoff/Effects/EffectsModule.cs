using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Effects
{
	public static class EffectsModule
	{
		private static readonly Log Log = Log.Create(typeof(EffectsModule));

		public static void Init()
		{
			Log.Debug("Effects initializing");
			Singleton<SurfaceImpactsEmitter>.Instance.Init();
			Singleton<CharacterImpactsEmitter>.Instance.Init();
			Singleton<MuzzleFlashEmitter>.Instance.Init();
			Singleton<BulletTraceEmitter>.Instance.Init();
			Log.Debug("Effects successfully initialized");
		}
	}
}
