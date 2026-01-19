using UnityEditor;

//Petit Menu pour ouvrir notre window
public class CustomMenu
{
    [MenuItem("Save System/Save Fields Settings")]
    public static void Open()
    {
        SaveFieldWindow.ShowWindow();
    }
}
