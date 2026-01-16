using F2O.SaveSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MySaveSystem
{
    public static string Extension = ".save";
    private static string _fileName = "";
    public static string GameFileName => string.IsNullOrEmpty(_fileName) ? MySaveData.PrefixID + 0 : _fileName;
    private static string GameSavePath => $"{RootDirectoyPath}/{GameFileName + Extension}";

    private static List<MySaveData> _allGameSaves = new();
    private static string RootDirectoyPath => $"{Application.persistentDataPath}/Saves";
    public static string[] GetAllGameSavePaths => Directory.GetFiles(RootDirectoyPath);

    public static void OnOpenDirectory() => SaveUtility.OpenDirectory(RootDirectoyPath);

    public static List<MySaveData> GetAllGameSaves()
    {
        if (GetAllGameSavePaths.Length == _allGameSaves.Count)
        {
            Debug.Log("Return Early");
            return _allGameSaves;
        }

        _allGameSaves.Clear();
        if (Directory.Exists(RootDirectoyPath))
        {
            foreach (var path in GetAllGameSavePaths)
            {
                _allGameSaves.Add(path.LoadFile<MySaveData>());
            }
        } else
        {
            Directory.CreateDirectory(RootDirectoyPath);
        }

        return _allGameSaves;
    }

    public static void CreateNewGameSave(MySaveData data)
    {
        _fileName = data.ID;
        data.SaveFile(GameSavePath, FileMode.CreateNew);

        _allGameSaves.Add(data);

        Debug.Log($"Create new Game Save {data} in <i>{RootDirectoyPath}</i>");
    }

    public static MySaveData GetGameDataSave(string labelSave) => GetAllGameSaves().FirstOrDefault(x => x.Label == labelSave);
    public static MySaveData LoadGameSave(string name = "")
    {
        MySaveData data = GetGameDataSave(name);

        if (data.Equals(default))
        {
            data = new(name);
            Debug.LogWarning($"Game save file {data} not found in <i>{RootDirectoyPath}</i>");
            CreateNewGameSave(data);
        }

        Debug.Log($"Load game {data} successful");

        return data;
    }

    public static void InitDirectories()
    {
        if (!Directory.Exists(RootDirectoyPath))
        {
            Directory.CreateDirectory(RootDirectoyPath);
        }
    }

    public static void GameSaveAs(ref MySaveData data)
    {
        _fileName = data.ID;
        data.SaveFile(GameSavePath, FileMode.Create);

        var id = _allGameSaves.FindIndex((x) => x.ID == _fileName);
        _allGameSaves[id] = data;

        Debug.Log($"Save game as {data} in <i>{RootDirectoyPath}</i>");
    }

    public static void DeleteGameSave(MySaveData data)
    {
        _fileName = data.ID;

        if (File.Exists(GameSavePath))
        {
            File.Delete(GameSavePath);
            Debug.Log($"Delete game {data} successful");
        }
    }


}
