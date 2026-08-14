using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal class OnScanCompletedListener : AndroidJavaProxy
	{
		private const string InterfaceSignature = "android.media.MediaScannerConnection$OnScanCompletedListener";

		private readonly Action<string, AndroidJavaObject> _onScanCompleted;

		public OnScanCompletedListener(Action<string, AndroidJavaObject> onScanCompleted)
			: base("android.media.MediaScannerConnection$OnScanCompletedListener")
		{
			_onScanCompleted = onScanCompleted;
		}

		public void onScanCompleted(string path, AndroidJavaObject uri)
		{
			if (uri.IsJavaNull())
			{
				UnityEngine.Debug.LogWarning("Scannning file " + path + " failed");
			}
			if (_onScanCompleted != null)
			{
				GoodiesSceneHelper.Queue(delegate
				{
					_onScanCompleted(path, uri);
				});
			}
		}
	}
}
