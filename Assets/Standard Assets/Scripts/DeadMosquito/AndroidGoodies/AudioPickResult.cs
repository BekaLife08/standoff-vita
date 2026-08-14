using DeadMosquito.AndroidGoodies.Internal;
using MiniJSON;
using System;
using System.Collections.Generic;

namespace DeadMosquito.AndroidGoodies
{
	public class AudioPickResult
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

		public string MimeType
		{
			get;
			private set;
		}

		public DateTime CreatedAt
		{
			get;
			private set;
		}

		public static AudioPickResult FromJson(string json)
		{
			AudioPickResult audioPickResult = new AudioPickResult();
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			audioPickResult.OriginalPath = dictionary.GetStr("originalPath");
			audioPickResult.DisplayName = dictionary.GetStr("displayName");
			audioPickResult.MimeType = dictionary.GetStr("mimeType");
			audioPickResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long)dictionary["createdAt"]);
			audioPickResult.Size = (int)(long)dictionary["size"];
			return audioPickResult;
		}

		public override string ToString()
		{
			return $"[AudioPickResult: OriginalPath={OriginalPath}, DisplayName={DisplayName}, Size={Size}, MimeType={MimeType}, CreatedAt={CreatedAt}]";
		}
	}
}
