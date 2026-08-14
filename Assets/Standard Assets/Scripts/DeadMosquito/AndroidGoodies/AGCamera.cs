using DeadMosquito.AndroidGoodies.Internal;
using System;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGCamera
	{
		private static Action<ImagePickResult> _onSuccessAction;

		private static Action<string> _onCancelAction;

		private static Action<VideoPickResult> _onVideoSuccessAction;

		private static Action<string> _onVideoCancelAction;

		public static bool DeviceHasCamera()
		{
			return AGDeviceInfo.SystemFeatures.HasSystemFeature("android.hardware.camera");
		}

		public static bool DeviceHasFrontalCamera()
		{
			return AGDeviceInfo.SystemFeatures.HasSystemFeature("android.hardware.camera.front");
		}

		public static bool DeviceHasCameraWithAutoFocus()
		{
			return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature("android.hardware.camera.autofocus");
		}

		public static bool DeviceHasCameraWithFlashlight()
		{
			return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature("android.hardware.camera.flash");
		}

		public static void TakePhoto(Action<ImagePickResult> onSuccess, Action<string> onError, ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (onSuccess == null)
				{
					throw new ArgumentNullException("onSuccess", "Success callback cannot be null");
				}
				_onSuccessAction = onSuccess;
				_onCancelAction = onError;
				AGUtils.RunOnUiThread(delegate
				{
					AGActivityUtils.TakePhoto(maxSize, shouldGenerateThumbnails);
				});
			}
		}

		internal static void OnSuccessTrigger(string imageCallbackJson)
		{
			if (_onSuccessAction != null)
			{
				ImagePickResult obj = ImagePickResult.FromJson(imageCallbackJson);
				_onSuccessAction(obj);
				_onCancelAction = null;
			}
		}

		internal static void OnErrorTrigger(string errorMessage)
		{
			if (_onCancelAction != null)
			{
				_onCancelAction(errorMessage);
				_onCancelAction = null;
			}
		}

		public static void RecordVideo(Action<VideoPickResult> onSuccess, Action<string> onError, bool generatePreviewImages = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				Check.Argument.IsNotNull(onError, "onError", string.Empty);
				_onVideoSuccessAction = onSuccess;
				_onVideoCancelAction = onError;
				AGActivityUtils.PickVideoCamera(generatePreviewImages);
			}
		}

		public static void OnVideoSuccessTrigger(string json)
		{
			if (_onVideoSuccessAction != null)
			{
				VideoPickResult obj = VideoPickResult.FromJson(json);
				_onVideoSuccessAction(obj);
				_onVideoSuccessAction = null;
			}
		}

		public static void OnVideoErrorTrigger(string message)
		{
			if (_onVideoCancelAction != null)
			{
				_onVideoCancelAction(message);
				_onVideoCancelAction = null;
			}
		}
	}
}
