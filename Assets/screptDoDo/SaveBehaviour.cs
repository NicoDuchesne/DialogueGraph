using UnityEngine;
using System.Reflection;

public class SaveBehaviour : MonoBehaviour
{
    public void Save()
    {
        var fields = GetType().GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic
        );

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<SaveField>();
            if (attribute == null) continue;

            string key = attribute.key ?? field.Name;
            object value = field.GetValue(this);

            if (value is int)
                PlayerPrefs.SetInt(key, (int)value);
            else if (value is float)
                PlayerPrefs.SetFloat(key, (float)value);
            else if (value is string)
                PlayerPrefs.SetString(key, (string)value);
            else if (value is bool)
                PlayerPrefs.SetInt(key, (bool)value ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void Load()
    {
        var fields = GetType().GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic
        );

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<SaveField>();
            if (attribute == null) continue;

            string key = attribute.key ?? field.Name;

            if (field.FieldType == typeof(int))
                field.SetValue(this, PlayerPrefs.GetInt(key));
            else if (field.FieldType == typeof(float))
                field.SetValue(this, PlayerPrefs.GetFloat(key));
            else if (field.FieldType == typeof(string))
                field.SetValue(this, PlayerPrefs.GetString(key));
            else if (field.FieldType == typeof(bool))
                field.SetValue(this, PlayerPrefs.GetInt(key) == 1);
        }
    }
}