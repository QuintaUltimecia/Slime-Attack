using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using System;

#if UNITY_EDITOR
public class SaveManagerEditor : EditorWindow
{
    [MenuItem("Game/Clear Saves")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("Saves is clear.");
    }

    [MenuItem("Game/Print Screen")]
    public static void PrintScreen()
    {
        string path = "Screenshots";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        DateTime time = DateTime.Now;

        string fileName = $"/{time.Year}.{time.Month}.{time.Day}-{time.Hour}.{time.Minute}.{time.Second}.png";

        ScreenCapture.CaptureScreenshot(path + fileName);
    }
}
#endif
