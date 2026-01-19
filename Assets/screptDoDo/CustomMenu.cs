using UnityEditor;
using UnityEngine;

public class CustomMenu
{
    [MenuItem("Save System/Save Fields Settings")]
    public static void Open()
    {
        SaveFieldWindow.ShowWindow();
    }
}
