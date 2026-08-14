using UnityEditor;
using UnityEngine;

public static class MakeTMP4
{
    public static void Run()
    {
        try
        {
            var so = ScriptableObject.CreateInstance("TMPro.TMP_FontAsset");
            AssetDatabase.CreateAsset(so, "Assets/_t2.font.asset");
            UnityEngine.Debug.Log("FONT2_DONE");
        }
        catch (System.Exception e) { UnityEngine.Debug.Log("FONT2_FAIL " + e.Message); }
    }
}