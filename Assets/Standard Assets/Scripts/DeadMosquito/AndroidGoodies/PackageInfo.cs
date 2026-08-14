using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public sealed class PackageInfo
	{
		public string PackageName
		{
			get;
			private set;
		}

		public int VersionCode
		{
			get;
			private set;
		}

		public string VersionName
		{
			get;
			private set;
		}

		public static PackageInfo FromJavaObj(AndroidJavaObject ajo)
		{
			using (ajo)
			{
				PackageInfo packageInfo = new PackageInfo();
				packageInfo.PackageName = ajo.Get<string>("packageName");
				packageInfo.VersionCode = ajo.Get<int>("versionCode");
				packageInfo.VersionName = ajo.Get<string>("versionName");
				return packageInfo;
			}
		}

		public override string ToString()
		{
			return $"PackageName: {PackageName}, VersionCode: {VersionCode}, VersionName: {VersionName}";
		}
	}
}
