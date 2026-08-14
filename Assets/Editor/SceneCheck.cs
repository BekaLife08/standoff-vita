using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneCheck
{
	public static void OpenMain()
	{
		UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
		Debug.Log("SceneCheck: opened " + scene.name + " rootCount=" + scene.rootCount);
		EditorApplication.Exit(0);
	}
}