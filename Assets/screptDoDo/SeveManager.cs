using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor.U2D.Tooling.Analyzer;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveSlot currentSlot;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    string GetPath(SaveSlot slot)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{slot}.json");
    }

    // ================= SAVE =================
    public void Save()
    {
        SaveFile file = new SaveFile();

        foreach (var mono in FindObjectsOfType<MonoBehaviour>())
        {
            var fields = mono.GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            SaveEntry entry = new SaveEntry
            {
                objectName = mono.gameObject.name,
                componentType = mono.GetType().AssemblyQualifiedName
            };

            bool hasData = false;

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<SaveField>();
                if (attr == null) continue;

                string key = attr.key ?? field.Name;
                object value = field.GetValue(mono);

                entry.values[key] = value.ToString();
                hasData = true;
            }

            if (hasData)
                file.entries.Add(entry);
        }

        File.WriteAllText(GetPath(currentSlot),
            JsonUtility.ToJson(file, true));
    }

    // ================= LOAD =================
    public void Load()
    {
        string path = GetPath(currentSlot);
        if (!File.Exists(path)) return;

        SaveFile file = JsonUtility.FromJson<SaveFile>(
            File.ReadAllText(path));

        foreach (var entry in file.entries)
        {
            GameObject go = GameObject.Find(entry.objectName);
            if (go == null) continue;

            var type = System.Type.GetType(entry.componentType);
            if (type == null) continue;

            var mono = go.GetComponent(type);
            if (mono == null) continue;

            foreach (var field in type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic))
            {
                var attr = field.GetCustomAttribute<SaveField>();
                if (attr == null) continue;

                string key = attr.key ?? field.Name;
                if (!entry.values.ContainsKey(key)) continue;

                string value = entry.values[key];

                if (field.FieldType == typeof(int))
                    field.SetValue(mono, int.Parse(value));
                else if (field.FieldType == typeof(float))
                    field.SetValue(mono, float.Parse(value));
                else if (field.FieldType == typeof(bool))
                    field.SetValue(mono, bool.Parse(value));
                else if (field.FieldType == typeof(string))
                    field.SetValue(mono, value);
            }
        }
    }

    public bool SlotExists(SaveSlot slot)
    {
        return File.Exists(GetPath(slot));
    }

    public void OpenSaveFolder()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}
