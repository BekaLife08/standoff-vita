using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class MakeAllUI
{
    public static void Run()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var canvas = new GameObject("Canvas", typeof(Canvas));
        canvas.transform.SetParent(null, false);

        System.Type[] types = new System.Type[]
        {
            typeof(Image), typeof(RawImage), typeof(Text), typeof(Button),
            typeof(ScrollRect), typeof(Scrollbar), typeof(Slider), typeof(Toggle),
            typeof(ToggleGroup), typeof(Dropdown), typeof(InputField),
            typeof(Mask), typeof(RectMask2D), typeof(Outline), typeof(Shadow),
            typeof(LayoutElement), typeof(ContentSizeFitter), typeof(AspectRatioFitter),
            typeof(VerticalLayoutGroup), typeof(HorizontalLayoutGroup), typeof(GridLayoutGroup),
            typeof(CanvasScaler), typeof(GraphicRaycaster),
            typeof(EventSystem), typeof(StandaloneInputModule),
        };

        foreach (var t in types)
        {
            var go = new GameObject(t.Name, t);
            go.transform.SetParent(canvas.transform, false);
            UnityEngine.Debug.Log("ADDED " + t.FullName);
        }

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/_uitest_all.unity");
        UnityEngine.Debug.Log("ALL_UI_SAVED");
    }
}
