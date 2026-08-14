using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGFileUtils
	{
		public static string SaveImageToGallery(Texture2D texture2D, string title, string folder = null, ImageFormat imageFormat = ImageFormat.PNG)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			if (texture2D == null)
			{
				throw new ArgumentNullException("texture2D", "Image to save cannot be null");
			}
			return AndroidPersistanceUtilsInternal.SaveImageToPictures(texture2D, title, folder, imageFormat);
		}

		public static Texture2D ImageUriToTexture2D(string imageUri)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}
			Check.Argument.IsStrNotNullOrEmpty(imageUri, "imageUri");
			return AGUtils.TextureFromUriInternal(imageUri);
		}
	}
}
