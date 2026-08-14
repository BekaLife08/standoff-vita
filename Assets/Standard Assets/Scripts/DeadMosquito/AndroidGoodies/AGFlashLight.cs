using DeadMosquito.AndroidGoodies.Internal;
using System;
using UnityEngine;

namespace DeadMosquito.AndroidGoodies
{
	public static class AGFlashLight
	{
		private const string CameraParameters_FLASH_MODE_TORCH = "torch";

		private static AndroidJavaObject _camera;

		private static string _cameraId;

		public static bool HasFlashlight()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			return AGDeviceInfo.SystemFeatures.HasFlashlight;
		}

		public static void Enable()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (!HasFlashlight())
				{
					UnityEngine.Debug.LogWarning("This device does not have a flashlight");
				}
				else if (AGDeviceInfo.SDK_INT >= 23)
				{
					TurnOnNew();
				}
				else
				{
					TurnOnOld();
				}
			}
		}

		private static void TurnOnNew()
		{
			try
			{
				_cameraId = AGSystemService.CameraService.Call<string[]>("getCameraIdList", Array.Empty<object>())[0];
				AGSystemService.CameraService.Call("setTorchMode", _cameraId, true);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		private static void TurnOnOld()
		{
			if (_camera != null)
			{
				UnityEngine.Debug.LogWarning("Flashlight is already on");
			}
			else
			{
				try
				{
					AGUtils.RunOnUiThread(delegate
					{
						using (AndroidJavaClass ajc = new AndroidJavaClass("android.hardware.Camera"))
						{
							AndroidJavaObject androidJavaObject = ajc.CallStaticAJO("open");
							AndroidJavaObject androidJavaObject2 = androidJavaObject.CallAJO("getParameters");
							androidJavaObject2.Call("setFlashMode", "torch");
							androidJavaObject.Call("setParameters", androidJavaObject2);
							androidJavaObject.Call("startPreview");
							_camera = androidJavaObject;
						}
					});
				}
				catch (Exception ex)
				{
					if (Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log("Could not enable flashlight:" + ex.Message);
					}
				}
			}
		}

		public static void Disable()
		{
			if (!AGUtils.IsNotAndroidCheck())
			{
				if (AGDeviceInfo.SDK_INT >= 23)
				{
					TurnOffNew();
				}
				else
				{
					TurnOffOld();
				}
			}
		}

		private static void TurnOffNew()
		{
			try
			{
				AGSystemService.CameraService.Call("setTorchMode", _cameraId, false);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		private static void TurnOffOld()
		{
			if (_camera != null)
			{
				AGUtils.RunOnUiThread(delegate
				{
					_camera.Call("stopPreview");
					_camera.Call("release");
					_camera.Dispose();
					_camera = null;
				});
			}
		}
	}
}
