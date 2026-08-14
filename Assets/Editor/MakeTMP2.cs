using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MakeTMP2
{
    public static void Run()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var canvas = new GameObject("Canvas", typeof(Canvas));

        var t = System.Type.GetType("TMPro.TextMeshProUGUI, TextMeshPro-1.0.55.2017.1.0b12");
        var go = new GameObject("TMPText", t);
        go.transform.SetParent(canvas.transform, false);

        var s = System.Type.GetType("TMPro.TMP_SubMeshUI, TextMeshPro-1.0.55.2017.1.0b12");
        if (s != null)
        {
            try
            {
                var go2 = new GameObject("TMPSub", s);
                go2.transform.SetParent(canvas.transform, false);
                UnityEngine.Debug.Log("SUBMESH_ADDED");
            }
            catch (System.Exception e) { UnityEngine.Debug.Log("SUBMESH_FAIL " + e.Message); }
        }
        else UnityEngine.Debug.Log("SUBMESH_TYPE_NOT_FOUND");

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/_tmphst2.unity");
        UnityEngine.Debug.Log("TMP2_SAVED");
    }
}