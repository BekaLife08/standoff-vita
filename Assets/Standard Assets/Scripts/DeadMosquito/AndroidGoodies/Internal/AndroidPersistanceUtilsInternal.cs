using System;
using System.IO;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AndroidPersistanceUtilsInternal
	{
		private const string FileProviderClass = "android.support.v4.content.FileProvider";

		private const string GoodiesFileFolder = "android-goodies";

		private const string GoodiesShareImageFileName = "android-goodies-share-image.png";

		public static AndroidJavaObject SaveShareImageToExternalStorage(Texture2D tex2D)
		{
			string text = SaveImageToPictures(tex2D, "android-goodies-share-image.png", "android-goodies");
			if (AGDeviceInfo.SDK_INT >= 24)
			{
				using (AndroidJavaClass ajc = new AndroidJavaClass("android.support.v4.content.FileProvider"))
				{
					string text2 = AGDeviceInfo.GetApplicationPackage() + ".multipicker.fileprovider";
					return ajc.CallStaticAJO("getUriForFile", AGUtils.Activity, text2, AGUtils.NewJavaFile(text));
				}
			}
			return AndroidUri.FromFile(text);
		}

		public static string SaveImageToPictures(Texture2D tex2D, string fileName, string directory = null, ImageFormat format = ImageFormat.PNG)
		{
			byte[] buffer = tex2D.Encode(format);
			fileName += ((format != 0) ? ".jpeg" : ".png");
			string directory2 = (!string.IsNullOrEmpty(directory)) ? Path.Combine(AGEnvironment.DirectoryPictures, directory) : AGEnvironment.DirectoryPictures;
			string text = SaveFileToExternalStorage(buffer, fileName, directory2);
			RefreshGallery(text);
			return text;
		}

		public static string SaveFileToExternalStorage(byte[] buffer, string fileName, string directory = null)
		{
			string text = AGEnvironment.ExternalStorageDirectoryPath;
			if (!string.IsNullOrEmpty(directory))
			{
				text = Path.Combine(text, directory);
				Directory.CreateDirectory(text);
			}
			string text2 = Path.Combine(text, fileName);
			try
			{
				FileStream fileStream = File.Open(text2, FileMode.OpenOrCreate);
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(buffer);
				fileStream.Close();
				return text2;
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError("Android Goodies failed to save file " + fileName + " to external storage");
				UnityEngine.Debug.LogException(exception);
				return text2;
			}
		}

		public static void RefreshGallery(string filePath)
		{
			if (AGDeviceInfo.SDK_INT >= 19)
			{
				ScanFile(filePath, null);
				return;
			}
			AndroidJavaObject uri = AndroidUri.FromFile(filePath);
			AndroidIntent androidIntent = new AndroidIntent("android.intent.action.MEDIA_MOUNTED", uri);
			AGUtils.SendBroadcast(androidIntent.AJO);
		}

		public static string InsertImage(Texture2D texture2D, string title, string description)
		{
			using (AndroidJavaClass ajc = new AndroidJavaClass("android.provider.MediaStore$Images$Media"))
			{
				using (AndroidJavaObject androidJavaObject2 = AGUtils.ContentResolver)
				{
					AndroidJavaObject androidJavaObject = AGUtils.Texture2DToAndroidBitmap(texture2D);
					return ajc.CallStaticStr("insertImage", androidJavaObject2, androidJavaObject, title, description);
				}
			}
		}

		public static void ScanFile(string filePath, Action<string, AndroidJavaObject> onScanCompleted)
		{
			OnScanCompletedListener onScanCompletedListener = (onScanCompleted != null) ? new OnScanCompletedListener(onScanCompleted) : null;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.media.MediaScannerConnection"))
			{
				androidJavaClass.CallStatic("scanFile", AGUtils.Activity, new string[1]
				{
					filePath
				}, null, onScanCompletedListener);
			}
		}
	}
}
