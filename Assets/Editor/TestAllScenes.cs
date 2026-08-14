using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TestAllScenes
{
    public static void Run()
    {
        string[] scenes = {
            "Assets/Scenes/Welcome.unity",
            "Assets/Scenes/Main.unity",
            "Assets/Scenes/Game.unity",
            "Assets/Scenes/GameView.unity",
            "Assets/Levels/Zone 9/Zone 9.unity",
            "Assets/Levels/Dust2/ShortDust2.unity",
            "Assets/Levels/TrainingOutside/TrainingOutside2.unity",
            "Assets/Levels/Province/Province.unity",
            "Assets/Scenes/Medals/Medals.unity",
            "Assets/Scenes/InventoryController.unity",
            "Assets/Scenes/MessagesController.unity",
            "Assets/Scenes/PlayController.unity",
            "Assets/Scenes/ProfileController.unity",
            "Assets/Scenes/ClanController.unity",
            "Assets/Scenes/FriendsController.unity",
            "Assets/Scenes/Controls.unity",
            "Assets/Scenes/Settings.unity"
        };
        foreach (var s in scenes)
        {
            int total = 0, nulls = 0, ui = 0;
            var counts = new System.Collections.Generic.Dictionary<string, int>();
            try
            {
                var scene = EditorSceneManager.OpenScene(s, OpenSceneMode.Single);
                foreach (var go in scene.GetRootGameObjects())
                {
                    foreach (var comp in go.GetComponentsInChildren<Component>(true))
                    {
                        total++;
                        if (comp == null) { nulls++; continue; }
                        var n = comp.GetType().Name;
                        if (n == "Image" || n == "Text" || n == "Button" || n == "RawImage" ||
                            n == "Slider" || n == "Toggle" || n == "Dropdown" || n == "InputField" ||
                            n == "ScrollRect" || n == "Scrollbar" || n == "Mask" || n == "RectMask2D" ||
                            n == "EventSystem" || n == "StandaloneInputModule" || n == "CanvasScaler" ||
                            n == "GraphicRaycaster" || n == "LayoutElement" || n == "ContentSizeFitter" ||
                            n == "VerticalLayoutGroup" || n == "HorizontalLayoutGroup" || n == "GridLayoutGroup" ||
                            n == "Shadow" || n == "Outline" || n == "TextMeshProUGUI" || n == "TMP_SubMeshUI") ui++;
                        if (counts.ContainsKey(n)) counts[n]++; else counts[n] = 1;
                    }
                }
                UnityEngine.Debug.Log("SCENE_TEST " + s + " total=" + total + " nulls=" + nulls + " UI=" + ui);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log("SCENE_TEST " + s + " FAILED " + e.Message);
            }
        }
        UnityEngine.Debug.Log("SCENE_TEST ALL_DONE");
    }
}