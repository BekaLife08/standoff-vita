using UnityEditor;
using UnityEngine;

public static class BuildScript
{
	public static void BuildWindows64()
	{
		string outPath = System.Environment.GetEnvironmentVariable("S2_BUILD_PATH");
		if (string.IsNullOrEmpty(outPath))
		{
			outPath = "build/Standoff2.exe";
		}
		Debug.Log("BuildScript: waiting for project to be ready");
		double start = EditorApplication.timeSinceStartup;
		while (EditorApplication.isCompiling || EditorApplication.isUpdating)
		{
			if (EditorApplication.timeSinceStartup - start > 300.0)
			{
				Debug.LogError("BuildScript: timeout waiting for project");
				EditorApplication.Exit(1);
				return;
			}
			System.Threading.Thread.Sleep(500);
		}
		Debug.Log("BuildScript: starting build to " + outPath);
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		string result = BuildPipeline.BuildPlayer(scenes, outPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
		Debug.Log("BuildScript: build result: " + result);
		EditorApplication.Exit(result == null ? 0 : 1);
	}

	public static void BuildVita()
	{
		string outPath = System.Environment.GetEnvironmentVariable("S2_VITA_BUILD_PATH");
		if (string.IsNullOrEmpty(outPath))
		{
			outPath = "build/Standoff2.vpk";
		}
		Debug.Log("BuildScript: waiting for project to be ready");
		double start = EditorApplication.timeSinceStartup;
		while (EditorApplication.isCompiling || EditorApplication.isUpdating)
		{
			if (EditorApplication.timeSinceStartup - start > 300.0)
			{
				Debug.LogError("BuildScript: timeout waiting for project");
				EditorApplication.Exit(1);
				return;
			}
			System.Threading.Thread.Sleep(500);
		}
		Debug.Log("BuildScript: starting vita build to " + outPath);
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		string result = BuildPipeline.BuildPlayer(scenes, outPath, BuildTarget.PSP2, BuildOptions.None);
		Debug.Log("BuildScript: vita build result: " + result);
		EditorApplication.Exit(result == null ? 0 : 1);
	}
}