using F2O.SaveSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MySaveSystem
{
    //File name and paths
    public static string Extension = ".save";
    private static string _fileName = "";
    public static string GameFileName => string.IsNullOrEmpty(_fileName) ? MySaveData.PrefixID + 0 : _fileName;
    private static string RootDirectoyPath => $"{Application.persistentDataPath}/Saves";
    private static string GameSavePath => $"{RootDirectoyPath}/{GameFileName + Extension}";

    //All game saves
    private static List<MySaveData> _allGameSaves = new();
    public static string[] GetAllGameSavePaths => Directory.GetFiles(RootDirectoyPath);

    
    //Get and return all the game saves
    public static List<MySaveData> GetAllGameSaves()
    {
        if (GetAllGameSavePaths.Length == _allGameSaves.Count)
        {
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

    //Create the .save file from a SaveData
    public static void CreateNewGameSave(MySaveData data)
    {
        _fileName = data.ID;
        data.SaveFile(GameSavePath, FileMode.CreateNew);

        _allGameSaves.Add(data);
    }

    //Get SaveData by Label
    public static MySaveData GetGameDataSave(string labelSave) => GetAllGameSaves().FirstOrDefault(x => x.Label == labelSave);

    //Get then check the data, then returns it
    public static MySaveData LoadGameSave(string name = "")
    {
        MySaveData data = GetGameDataSave(name);

        if (data.Equals(default))
        {
            data = new(name);
            Debug.LogWarning($"Game save file {data} not found in <i>{RootDirectoyPath}</i>");
            CreateNewGameSave(data);
        }

        return data;
    }

    
    //Update et enregistre une save data déjà existante
    public static void GameSaveAs(ref MySaveData data)
    {
        _fileName = data.ID;
        data.SaveFile(GameSavePath, FileMode.Create);

        var id = _allGameSaves.FindIndex((x) => x.ID == _fileName);
        _allGameSaves[id] = data;
    }

    //Delete a save file by giving its Save Data
    public static void DeleteGameSave(MySaveData data)
    {
        _fileName = data.ID;

        if (File.Exists(GameSavePath))
        {
            File.Delete(GameSavePath);
        }
    }


    //Check and create the save directory
    public static void InitDirectories()
    {
        if (!Directory.Exists(RootDirectoyPath))
        {
            Directory.CreateDirectory(RootDirectoyPath);
        }
    }

    public static void OnOpenDirectory() => SaveUtility.OpenDirectory(RootDirectoyPath);


}
