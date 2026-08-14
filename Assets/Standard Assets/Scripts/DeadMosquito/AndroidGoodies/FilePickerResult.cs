using DeadMosquito.AndroidGoodies.Internal;
using MiniJSON;
using System;
using System.Collections.Generic;

namespace DeadMosquito.AndroidGoodies
{
	public class FilePickerResult
	{
		public string OriginalPath
		{
			get;
			private set;
		}

		public string DisplayName
		{
			get;
			private set;
		}

		public int Size
		{
			get;
			private set;
		}

		public DateTime CreatedAt
		{
			get;
			private set;
		}

		public static FilePickerResult FromJson(string json)
		{
			FilePickerResult filePickerResult = new FilePickerResult();
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			filePickerResult.OriginalPath = dictionary.GetStr("originalPath");
			filePickerResult.DisplayName = dictionary.GetStr("displayName");
			filePickerResult.Size = (int)(long)dictionary["size"];
			filePickerResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long)dictionary["createdAt"]);
			return filePickerResult;
		}

		public override string ToString()
		{
			return $"[FilePickerResult: OriginalPath={OriginalPath}, DisplayName={DisplayName}, Size={Size}, CreatedAt={CreatedAt}]";
		}
	}
}
