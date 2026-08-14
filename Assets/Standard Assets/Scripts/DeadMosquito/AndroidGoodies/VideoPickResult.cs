using DeadMosquito.AndroidGoodies.Internal;
using MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public class VideoPickResult
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

		public string PreviewImagePath
		{
			get;
			private set;
		}

		public string PreviewImageThumbnailPath
		{
			get;
			private set;
		}

		public string PreviewImageSmallThumbnailPath
		{
			get;
			private set;
		}

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public int Orientation
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

		public Texture2D LoadPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImagePath);
		}

		public Texture2D LoadThumbnailPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImageThumbnailPath);
		}

		public Texture2D LoadSmallPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImageSmallThumbnailPath);
		}

		public static VideoPickResult FromJson(string json)
		{
			VideoPickResult videoPickResult = new VideoPickResult();
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			videoPickResult.OriginalPath = dictionary.GetStr("originalPath");
			videoPickResult.PreviewImagePath = dictionary.GetStr("previewImage");
			videoPickResult.PreviewImageThumbnailPath = dictionary.GetStr("previewImageThumbnail");
			videoPickResult.PreviewImageSmallThumbnailPath = dictionary.GetStr("previewImageThumbnailSmall");
			videoPickResult.DisplayName = dictionary.GetStr("displayName");
			videoPickResult.Width = (int)(long)dictionary["width"];
			videoPickResult.Height = (int)(long)dictionary["height"];
			videoPickResult.Orientation = (int)(long)dictionary["orientation"];
			videoPickResult.Size = (int)(long)dictionary["size"];
			videoPickResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long)dictionary["createdAt"]);
			return videoPickResult;
		}

		public override string ToString()
		{
			return $"[VideoPickResult: OriginalPath={OriginalPath}, DisplayName={DisplayName}, PreviewImagePath={PreviewImagePath}, PreviewImageThumbnailPath={PreviewImageThumbnailPath}, PreviewImageSmallThumbnailPath={PreviewImageSmallThumbnailPath}, Width={Width}, Height={Height}, Orientation={Orientation}, Size={Size}, CreatedAt={CreatedAt}]";
		}
	}
}
