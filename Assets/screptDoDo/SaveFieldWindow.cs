using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class SaveFieldWindow : EditorWindow
{
    Vector2 scroll;
    Dictionary<string, bool> fieldStates = new();

    [MenuItem("Save System/Save Fields Settings")]
    public static void ShowWindow()
    {
        GetWindow<SaveFieldWindow>("Save Fields");
    }

    void OnEnable()
    {
        Load();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
            Scan();

        GUILayout.Space(5);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var key in new List<string>(fieldStates.Keys))
        {
            fieldStates[key] = EditorGUILayout.ToggleLeft(key, fieldStates[key]);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Save Selection"))
            Save();
    }

    void Scan()
    {
        fieldStates.Clear();

        foreach (var mono in FindObjectsOfType<MonoBehaviour>())
        {
            var fields = mono.GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<SaveField>();
                if (attr == null) continue;

                string key = GetKey(mono, field);
                if (!fieldStates.ContainsKey(key))
                    fieldStates[key] = IsEnabled(key);
            }
        }
    }

    string GetKey(MonoBehaviour mono, FieldInfo field)
    {
        return $"{mono.gameObject.name}/{mono.GetType().Name}/{field.Name}";
    }

    void Save()
    {
        foreach (var kv in fieldStates)
            EditorPrefs.SetBool(kv.Key, kv.Value);
    }

    void Load()
    {
        Scan();
    }

    bool IsEnabled(string key)
    {
        return EditorPrefs.GetBool(key, true);
    }

    public static bool IsFieldEnabled(string key)
    {
        return EditorPrefs.GetBool(key, true);
    }
}
