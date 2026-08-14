using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class MakeTestUI
{
    public static void Run()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var imgGo = new GameObject("Image", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        imgGo.transform.SetParent(go.transform, false);

        var txtGo = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        txtGo.transform.SetParent(go.transform, false);

        var btnGo = new GameObject("Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnGo.transform.SetParent(go.transform, false);

        var esGo = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/_uitest.unity");
        UnityEngine.Debug.Log("UITEST_SAVED");
    }
}
