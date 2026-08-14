using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGGallery
	{
		private static Action<ImagePickResult> _onSuccessAction;

		private static Action<string> _onCancelAction;

		public static void PickImageFromGallery(Action<ImagePickResult> onSuccess, Action<string> onError, ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				_onSuccessAction = onSuccess;
				_onCancelAction = onError;
				AGActivityUtils.PickPhotoFromGallery(maxSize, shouldGenerateThumbnails);
			}
		}

		internal static void OnSuccessTrigger(string imageCallbackJson)
		{
			if (_onSuccessAction != null)
			{
				ImagePickResult obj = ImagePickResult.FromJson(imageCallbackJson);
				_onSuccessAction(obj);
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

		public static void SaveImageToGallery(Texture2D texture2D, string title, string folder = null, ImageFormat imageFormat = ImageFormat.PNG)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				AGFileUtils.SaveImageToGallery(texture2D, title, folder, imageFormat);
			}
		}
	}
}
