using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MakeTMP
{
    public static void Run()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var go = new GameObject("TMPTest");
        var t = System.Type.GetType("TMPro.TextMeshProUGUI, TextMeshPro-1.0.55.2017.1.0b12");
        if (t == null)
        {
            UnityEngine.Debug.Log("TMP_TYPE_NOT_FOUND");
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
                UnityEngine.Debug.Log("ASM " + asm.GetName().Name);
        }
        else
        {
            go.AddComponent(t);
            UnityEngine.Debug.Log("TMP_ADDED " + t.FullName);
        }
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/_tmphst.unity");
        UnityEngine.Debug.Log("TMP_SAVED");
    }
}