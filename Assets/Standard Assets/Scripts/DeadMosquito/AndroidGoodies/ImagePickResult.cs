using DeadMosquito.AndroidGoodies.Internal;
using MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public class ImagePickResult
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

		public string ThumbnailPath
		{
			get;
			private set;
		}

		public string SmallThumbnailPath
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

		public Texture2D LoadTexture2D()
		{
			return AGUtils.Texture2DFromFile(OriginalPath);
		}

		public Texture2D LoadThumbnailTexture2D()
		{
			return CommonUtils.TextureFromFile(ThumbnailPath);
		}

		public Texture2D LoadSmallThumbnailTexture2D()
		{
			return CommonUtils.TextureFromFile(SmallThumbnailPath);
		}

		public static ImagePickResult FromJson(string json)
		{
			ImagePickResult imagePickResult = new ImagePickResult();
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			imagePickResult.OriginalPath = dictionary.GetStr("originalPath");
			imagePickResult.ThumbnailPath = dictionary.GetStr("thumbnailPath");
			imagePickResult.SmallThumbnailPath = dictionary.GetStr("thumbnailSmallPath");
			imagePickResult.DisplayName = dictionary.GetStr("displayName");
			imagePickResult.Width = (int)(long)dictionary["width"];
			imagePickResult.Height = (int)(long)dictionary["height"];
			imagePickResult.Size = (int)(long)dictionary["size"];
			imagePickResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long)dictionary["createdAt"]);
			return imagePickResult;
		}

		public override string ToString()
		{
			return $"[ImagePickResult: OriginalPath={OriginalPath}, DisplayName={DisplayName}, ThumbnailPath={ThumbnailPath}, SmallThumbnailPath={SmallThumbnailPath}, Width={Width}, Height={Height}, Size={Size}]";
		}
	}
}
