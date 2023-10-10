using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class EditorDeveloperWindow : EditorWindow
{
    [MenuItem("Game Developer/Developer")]
    public static void ShowWindow()
    {
        GetWindow<EditorDeveloperWindow>(false, "Developer", true);
    }

    public void OnGUI()
    {
        GUILayout.Label("Road");
        GetIcons("Road");
        GUILayout.Label("Landspace");
        GetIcons("Landspace");
        GUILayout.Label("Boosts");
        GetIcons("Boosts");
    }

    public void GetIcons(string path)
    {
        Object[] values = Resources.LoadAll(path);
        float size = 100f;

        int count = Directory.GetFiles(Application.dataPath + $"/_Slime Attack io/Resources/{path}").Length / 2;

        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < count; i++)
        {
            if (i % 4 == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            Texture texture = AssetPreview.GetAssetPreview(values[i]);
            CreateButton(size, texture, values[i], path);
        }

        EditorGUILayout.EndHorizontal();
    }

    public void CreateButton(float size, Texture texture, Object value, string path)
    {
        if (GUILayout.Button(texture,
            GUILayout.MaxWidth(size),
            GUILayout.MaxHeight(size),
            GUILayout.MinWidth(size),
            GUILayout.MinHeight(size)))
        {
            CreateItem(value, path);
        }
    }

    public void CreateItem(Object value, string path)
    {
        Object prefab = PrefabUtility.InstantiatePrefab(value, GameObject.Find(path).transform);
        //PrefabUtility.ApplyAddedGameObject(prefab.GameObject(), PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(GameObject.Find("World")), InteractionMode.AutomatedAction);
        PrefabUtility.GetIconForGameObject(prefab.GameObject());

        var view = SceneView.lastActiveSceneView;
        Camera cam = view.camera;

        prefab.GameObject().transform.localPosition = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);
        Selection.activeObject = prefab.GameObject();
    }
}
#endif