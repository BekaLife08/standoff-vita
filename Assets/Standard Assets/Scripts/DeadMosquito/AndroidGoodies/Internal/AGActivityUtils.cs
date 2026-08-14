using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	public static class AGActivityUtils
	{
		public const string JarPackageName = "com.deadmosquitogames.";

		public const string AndroidGoodiesActivityClassSignature = "com.deadmosquitogames.AndroidGoodiesActivity";

		public const string PhotoPickerUtilsClassSignature = "com.deadmosquitogames.PhotoPickerUtils";

		private const string EXTRAS_PERMISSIONS = "EXTRAS_PERMISSIONS";

		private const string EXTRAS_PICKER_TYPE = "EXTRAS_PICKER_TYPE";

		private const string EXTRAS_MAX_SIZE = "EXTRAS_MAX_SIZE";

		private const string EXTRAS_GENERATE_THUMBNAILS = "EXTRAS_GENERATE_THUMBNAILS";

		private const string EXTRAS_GENERATE_PREVIEW_IMAGES = "EXTRAS_GENERATE_PREVIEW_IMAGES";

		private const string EXTRAS_MIME_TYPE = "EXTRAS_MIME_TYPE";

		private const int REQ_CODE_PICK_IMAGE = 3111;

		private const int REQ_CODE_TAKE_PHOTO = 4222;

		private const int REQ_CODE_PICK_CONTACT = 8666;

		private const int REQ_CODE_PICK_AUDIO = 9777;

		private const int REQ_CODE_PICK_VIDEO_DEVICE = 5333;

		private const int REQ_CODE_PICK_VIDEO_CAMERA = 6444;

		private const int REQ_CODE_PICK_FILE = 7555;

		private const int REQ_CODE_PERMISSIONS = 444;

		private static AndroidJavaClass _androidGoodiesActivityClass;

		public static AndroidJavaClass AndroidGoodiesActivityClass
		{
			get
			{
				if (_androidGoodiesActivityClass == null)
				{
					_androidGoodiesActivityClass = new AndroidJavaClass("com.deadmosquitogames.AndroidGoodiesActivity");
				}
				return _androidGoodiesActivityClass;
			}
		}

		public static void PickContact()
		{
			StartAndroidGoodiesActivity(8666, delegate
			{
			});
		}

		public static void PickPhotoFromGallery(ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			PickImage(3111, maxSize, shouldGenerateThumbnails);
		}

		public static void TakePhoto(ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			PickImage(4222, maxSize, shouldGenerateThumbnails);
		}

		private static void PickImage(int requestCode, ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			StartAndroidGoodiesActivity(requestCode, delegate(AndroidIntent intent)
			{
				intent.PutExtra("EXTRAS_MAX_SIZE", (int)maxSize);
				intent.PutExtra("EXTRAS_GENERATE_THUMBNAILS", shouldGenerateThumbnails);
			});
		}

		public static void PickAudio()
		{
			StartAndroidGoodiesActivity(9777, delegate
			{
			});
		}

		public static void PickVideoDevice(bool generatePreviewImages)
		{
			StartAndroidGoodiesActivity(5333, delegate(AndroidIntent intent)
			{
				intent.PutExtra("EXTRAS_GENERATE_PREVIEW_IMAGES", generatePreviewImages);
			});
		}

		public static void PickVideoCamera(bool generatePreviewImages)
		{
			StartAndroidGoodiesActivity(6444, delegate(AndroidIntent intent)
			{
				intent.PutExtra("EXTRAS_GENERATE_PREVIEW_IMAGES", generatePreviewImages);
			});
		}

		public static void PickFile(string mimeType)
		{
			StartAndroidGoodiesActivity(7555, delegate(AndroidIntent intent)
			{
				intent.PutExtra("EXTRAS_MIME_TYPE", mimeType);
			});
		}

		public static void RequestPermissions(string[] permissions)
		{
			StartAndroidGoodiesActivity(444, delegate(AndroidIntent intent)
			{
				intent.PutExtra("EXTRAS_PERMISSIONS", permissions);
			});
		}

		public static void StartAndroidGoodiesActivity(int pickerType, Action<AndroidIntent> configurator)
		{
			using (AndroidJavaObject next = AGUtils.ClassForName("com.deadmosquitogames.AndroidGoodiesActivity"))
			{
				using (AndroidIntent androidIntent = new AndroidIntent(AGUtils.Activity, next))
				{
					androidIntent.PutExtra("EXTRAS_PICKER_TYPE", pickerType);
					configurator(androidIntent);
					AGUtils.StartActivity(androidIntent.AJO);
				}
			}
		}
	}
}
