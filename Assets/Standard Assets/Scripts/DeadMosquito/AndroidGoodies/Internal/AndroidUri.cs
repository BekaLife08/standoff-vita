using UnityEngine;

namespace DeadMosquito.AndroidGoodies.Internal
{
	internal static class AndroidUri
	{
		public static AndroidJavaObject Parse(string uriString)
		{
			using (AndroidJavaClass ajc = new AndroidJavaClass("android.net.Uri"))
			{
				return ajc.CallStaticAJO("parse", uriString);
			}
		}

		public static AndroidJavaObject FromFile(string filePath)
		{
			using (AndroidJavaClass ajc = new AndroidJavaClass("android.net.Uri"))
			{
				return ajc.CallStaticAJO("fromFile", AGUtils.NewJavaFile(filePath));
			}
		}
	}
}
