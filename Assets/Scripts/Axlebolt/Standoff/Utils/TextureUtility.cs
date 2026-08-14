using System.Threading;
using UnityEngine;

namespace Axlebolt.Standoff.Utils
{
	public class TextureUtility
	{
		public class ThreadData
		{
			public int start;

			public int end;

			public ThreadData(int s, int e)
			{
				start = s;
				end = e;
			}
		}

		private static Color[] _texColors;

		private static Color[] _newColors;

		private static int _w;

		private static float _ratioX;

		private static float _ratioY;

		private static int _w2;

		private static int _finishCount;

		private static Mutex _mutex;

		public static void Point(Texture2D tex, int newWidth, int newHeight)
		{
			ThreadedScale(tex, newWidth, newHeight, useBilinear: false);
		}

		public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
		{
			ThreadedScale(tex, newWidth, newHeight, useBilinear: true);
		}

		private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
		{
			_texColors = tex.GetPixels();
			_newColors = new Color[newWidth * newHeight];
			if (useBilinear)
			{
				_ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
				_ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
			}
			else
			{
				_ratioX = (float)tex.width / (float)newWidth;
				_ratioY = (float)tex.height / (float)newHeight;
			}
			_w = tex.width;
			_w2 = newWidth;
			int num = Mathf.Min(SystemInfo.processorCount, newHeight);
			int num2 = newHeight / num;
			_finishCount = 0;
			if (_mutex == null)
			{
				_mutex = new Mutex(initiallyOwned: false);
			}
			if (num > 1)
			{
				int num3 = 0;
				ThreadData parameter;
				for (num3 = 0; num3 < num - 1; num3++)
				{
					parameter = new ThreadData(num2 * num3, num2 * (num3 + 1));
					ParameterizedThreadStart start = (!useBilinear) ? new ParameterizedThreadStart(PointScale) : new ParameterizedThreadStart(BilinearScale);
					Thread thread = new Thread(start);
					thread.Start(parameter);
				}
				parameter = new ThreadData(num2 * num3, newHeight);
				if (useBilinear)
				{
					BilinearScale(parameter);
				}
				else
				{
					PointScale(parameter);
				}
				while (_finishCount < num)
				{
					Thread.Sleep(1);
				}
			}
			else
			{
				ThreadData obj = new ThreadData(0, newHeight);
				if (useBilinear)
				{
					BilinearScale(obj);
				}
				else
				{
					PointScale(obj);
				}
			}
			tex.Resize(newWidth, newHeight);
			tex.SetPixels(_newColors);
			tex.Apply();
			_texColors = null;
			_newColors = null;
		}

		private static void BilinearScale(object obj)
		{
			ThreadData threadData = (ThreadData)obj;
			for (int i = threadData.start; i < threadData.end; i++)
			{
				int num = (int)Mathf.Floor((float)i * _ratioY);
				int num2 = num * _w;
				int num3 = (num + 1) * _w;
				int num4 = i * _w2;
				for (int j = 0; j < _w2; j++)
				{
					int num5 = (int)Mathf.Floor((float)j * _ratioX);
					float value = (float)j * _ratioX - (float)num5;
					_newColors[num4 + j] = ColorLerpUnclamped(ColorLerpUnclamped(_texColors[num2 + num5], _texColors[num2 + num5 + 1], value), ColorLerpUnclamped(_texColors[num3 + num5], _texColors[num3 + num5 + 1], value), (float)i * _ratioY - (float)num);
				}
			}
			_mutex.WaitOne();
			_finishCount++;
			_mutex.ReleaseMutex();
		}

		private static void PointScale(object obj)
		{
			ThreadData threadData = (ThreadData)obj;
			for (int i = threadData.start; i < threadData.end; i++)
			{
				int num = (int)(_ratioY * (float)i) * _w;
				int num2 = i * _w2;
				for (int j = 0; j < _w2; j++)
				{
					_newColors[num2 + j] = _texColors[(int)((float)num + _ratioX * (float)j)];
				}
			}
			_mutex.WaitOne();
			_finishCount++;
			_mutex.ReleaseMutex();
		}

		private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
		{
			return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
		}

		public static Texture2D FromBytes(byte[] bytes)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);
			texture2D.LoadImage(bytes);
			return texture2D;
		}
	}
}
