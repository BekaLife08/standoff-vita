using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TestMainCount
{
    public static void Run()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
        var counts = new System.Collections.Generic.Dictionary<string, int>();
        int nulls = 0, total = 0;
        foreach (var go in scene.GetRootGameObjects())
        {
            foreach (var comp in go.GetComponentsInChildren<Component>(true))
            {
                total++;
                if (comp == null) { nulls++; continue; }
                var n = comp.GetType().Name;
                if (counts.ContainsKey(n)) counts[n]++; else counts[n] = 1;
            }
        }
        UnityEngine.Debug.Log("COUNT_TEST total=" + total + " nulls=" + nulls);
        foreach (var kv in counts)
            UnityEngine.Debug.Log("COUNT_TEST " + kv.Key + "=" + kv.Value);
    }
}
