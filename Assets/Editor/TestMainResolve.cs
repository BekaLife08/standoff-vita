using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class TestMainResolve
{
    public static void Run()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
        int total = 0, hasImage = 0, hasText = 0, hasButton = 0, hasEventSys = 0, hasSim = 0, hasMono = 0;
        foreach (var go in scene.GetRootGameObjects())
        {
            foreach (var comp in go.GetComponentsInChildren<Component>(true))
            {
                if (comp == null) continue;
                total++;
                var t = comp.GetType().Name;
                if (t == "Image") hasImage++;
                if (t == "Text") hasText++;
                if (t == "Button") hasButton++;
                if (t == "EventSystem") hasEventSys++;
                if (t == "StandaloneInputModule") hasSim++;
                if (comp is MonoBehaviour) hasMono++;
            }
        }
        UnityEngine.Debug.Log("RESOLVE_TEST total=" + total + " Image=" + hasImage + " Text=" + hasText +
            " Button=" + hasButton + " EventSystem=" + hasEventSys + " StandaloneInputModule=" + hasSim + " Mono=" + hasMono);

        var bg = GameObject.Find("Background");
        if (bg != null)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var c in bg.GetComponents<Component>())
                sb.Append(c == null ? "NULL" : c.GetType().Name).Append(";");
            UnityEngine.Debug.Log("RESOLVE_TEST Background=" + sb);
        }
        var ts = GameObject.Find("Title");
        if (ts != null)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var c in ts.GetComponents<Component>())
                sb.Append(c == null ? "NULL" : c.GetType().Name).Append(";");
            UnityEngine.Debug.Log("RESOLVE_TEST Title=" + sb);
        }
    }
}
