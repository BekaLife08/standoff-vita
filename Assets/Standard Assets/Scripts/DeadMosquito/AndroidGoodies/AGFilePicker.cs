using DeadMosquito.AndroidGoodies.Internal;
using System;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGFilePicker
	{
		private static Action<AudioPickResult> _onAudioSuccessAction;

		private static Action<string> _onAudioCancelAction;

		private static Action<VideoPickResult> _onVideoSuccessAction;

		private static Action<string> _onVideoCancelAction;

		private static Action<FilePickerResult> _onFileSuccessAction;

		private static Action<string> _onFileCancelAction;

		public static void PickAudio(Action<AudioPickResult> onSuccess, Action<string> onError)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				Check.Argument.IsNotNull(onError, "onError", string.Empty);
				_onAudioSuccessAction = onSuccess;
				_onAudioCancelAction = onError;
				AGActivityUtils.PickAudio();
			}
		}

		public static void OnAudioSuccessTrigger(string json)
		{
			if (_onAudioSuccessAction != null)
			{
				AudioPickResult obj = AudioPickResult.FromJson(json);
				_onAudioSuccessAction(obj);
				_onAudioSuccessAction = null;
			}
		}

		public static void OnAudioErrorTrigger(string message)
		{
			if (_onAudioCancelAction != null)
			{
				_onAudioCancelAction(message);
				_onAudioCancelAction = null;
			}
		}

		public static void PickVideo(Action<VideoPickResult> onSuccess, Action<string> onError, bool generatePreviewImages = true)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				Check.Argument.IsNotNull(onError, "onError", string.Empty);
				_onVideoSuccessAction = onSuccess;
				_onVideoCancelAction = onError;
				AGActivityUtils.PickVideoDevice(generatePreviewImages);
			}
		}

		public static void OnVideoSuccessTrigger(string json)
		{
			if (_onVideoSuccessAction != null)
			{
				VideoPickResult obj = VideoPickResult.FromJson(json);
				_onVideoSuccessAction(obj);
				_onVideoSuccessAction = null;
			}
		}

		public static void OnVideoErrorTrigger(string message)
		{
			if (_onVideoCancelAction != null)
			{
				_onVideoCancelAction(message);
				_onVideoCancelAction = null;
			}
		}

		public static void PickFile(Action<FilePickerResult> onSuccess, Action<string> onError, string mimeType)
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				Check.Argument.IsNotNull(onSuccess, "onSuccess", string.Empty);
				Check.Argument.IsNotNull(onError, "onError", string.Empty);
				_onFileSuccessAction = onSuccess;
				_onFileCancelAction = onError;
				AGActivityUtils.PickFile(mimeType);
			}
		}

		public static void OnFileSuccessTrigger(string json)
		{
			if (_onFileSuccessAction != null)
			{
				FilePickerResult obj = FilePickerResult.FromJson(json);
				_onFileSuccessAction(obj);
				_onFileSuccessAction = null;
			}
		}

		public static void OnFileErrorTrigger(string message)
		{
			if (_onFileCancelAction != null)
			{
				_onFileCancelAction(message);
				_onFileCancelAction = null;
			}
		}
	}
}
