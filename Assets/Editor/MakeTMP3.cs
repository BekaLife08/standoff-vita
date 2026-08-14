using UnityEditor;
using UnityEngine;

public static class MakeTMP3
{
    public static void Run()
    {
        var dll = "TextMeshPro-1.0.55.2017.1.0b12";

        var fontType = System.Type.GetType("TMPro.TMP_FontAsset, " + dll);
        if (fontType != null)
        {
            try
            {
                var arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
                var mi = fontType.GetMethod("CreateFontAsset", new[] { typeof(Font) });
                var asset = mi.Invoke(null, new object[] { arial });
                AssetDatabase.CreateAsset((ScriptableObject)asset, "Assets/_t.font.asset");
                UnityEngine.Debug.Log("FONT_DONE");
            }
            catch (System.Exception e) { UnityEngine.Debug.Log("FONT_FAIL " + e.Message); }
        }
        else UnityEngine.Debug.Log("FONT_TYPE_NOT_FOUND");

        TryCreate("TMPro.TMP_SpriteAsset", "Assets/_t.sprite.asset", "SPRITE");
        TryCreate("TMPro.TMP_StyleSheet", "Assets/_t.style.asset", "STYLE");
        TryCreate("TMPro.TMP_Settings", "Assets/_t.settings.asset", "SETTINGS");

        UnityEngine.Debug.Log("TMP3_DONE");
    }

    static void TryCreate(string typeName, string path, string tag)
    {
        try
        {
            var so = ScriptableObject.CreateInstance(typeName);
            AssetDatabase.CreateAsset(so, path);
            UnityEngine.Debug.Log(tag + "_DONE");
        }
        catch (System.Exception e) { UnityEngine.Debug.Log(tag + "_FAIL " + e.Message); }
    }
}