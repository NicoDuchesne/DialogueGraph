using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace F2O.SaveSystem
{
    public static class SaveSystem
    {
        public static string DefaultSettingLabel = "Default Setting";
        public static string DefaultProfilName = "DefaultProfil";
        public static string AutoSaveLabel = "Auto Save";
        public static string Extension = ".save";
        
        private static string _currentProfil = "";
        public static string CurrentProfil => string.IsNullOrEmpty(_currentProfil) ? DefaultProfilName : _currentProfil;

        private static string _fileName = "";
        public static string GameFileName => string.IsNullOrEmpty(_fileName) ? GameDataSave.PrefixID + 0 : _fileName;
        public static string SettingFileName => string.IsNullOrEmpty(_fileName) ? SettingDataSave.PrefixID + 0 : _fileName;

        private static string RootDirectoyPath => $"{Application.persistentDataPath}/Saves";
        private static string ProfilDirectoryPath => $"{RootDirectoyPath}/{CurrentProfil}";
        private static string SettingsDirectoryPath => $"{RootDirectoyPath}/Settings";
        private static string GamesDirectoryPath => $"{ProfilDirectoryPath}/Games";

        private static string SettingSavePath => $"{SettingsDirectoryPath}/{SettingFileName + Extension}";
        private static string GameSavePath => $"{GamesDirectoryPath}/{GameFileName + Extension}";

        public static void OnOpenDirectory() => SaveUtility.OpenDirectory(RootDirectoyPath);

        #region INIT
        public static void InitDirectories()
        {
            if (!Directory.Exists(RootDirectoyPath))
            {
                Directory.CreateDirectory(RootDirectoyPath);
                Directory.CreateDirectory(ProfilDirectoryPath);
                Directory.CreateDirectory(GamesDirectoryPath);
                Directory.CreateDirectory(SettingsDirectoryPath);
            }
        }

        public static GameDataSave InitAutoGameSave()
        {
            GameDataSave autoSave;

            if (!File.Exists(GameSavePath))
            {
                autoSave = new(AutoSaveLabel);
                CreateNewGameSave(autoSave);
            }
            else
                autoSave = LoadGameSave(AutoSaveLabel);

            return autoSave;
        }

        public static SettingDataSave InitAutoSettingSave()
        {
            SettingDataSave autoSave;

            if (!File.Exists(SettingSavePath))
            {
                autoSave = new(DefaultSettingLabel);
                CreateNewSettingSave(autoSave);
            }
            else
                autoSave = LoadSettingSave(DefaultSettingLabel);

            return autoSave;
        }

        //public static void InitGameSave()
        //{
        //    if (File.Exists(GameSavePath))
        //        return;

        //    new GameDataSave().SaveFile(GameSavePath, FileMode.Create);

        //    Debug.Log($"Init game save <b>{_fileName}</b> in <i>{GamesDirectoryPath}</i>");
        //}
        #endregion

        #region GET
        public static string[] GetAllGameSavePaths => Directory.GetFiles(GamesDirectoryPath);
        public static string[] GetAllSettingSavePaths => Directory.GetFiles(SettingsDirectoryPath);

        private static List<GameDataSave> _allGameSaves = new();
        public static List<GameDataSave> GetAllGameSaves()
        {
            if(GetAllGameSavePaths.Length == _allGameSaves.Count)
                return _allGameSaves;

            _allGameSaves.Clear();
            if (Directory.Exists(GamesDirectoryPath))
            {
                foreach (var sp in GetAllGameSavePaths)
                    _allGameSaves.Add(sp.LoadFile<GameDataSave>());
            }
            else
            {
                Directory.CreateDirectory(GamesDirectoryPath);
                _allGameSaves.Add(InitAutoGameSave());
            }
            
            return _allGameSaves;
        }

        private static List<SettingDataSave> _allSettingSaves = new();
        public static List<SettingDataSave> GetAllSettingSaves()
        {
            if (GetAllSettingSavePaths.Length == _allSettingSaves.Count)
                return _allSettingSaves;

            _allSettingSaves.Clear();
            if (Directory.Exists(SettingsDirectoryPath))
            {
                foreach (var sp in GetAllSettingSavePaths)
                    _allSettingSaves.Add(sp.LoadFile<SettingDataSave>());
            }
            else
            {
                Directory.CreateDirectory(SettingsDirectoryPath);
                _allSettingSaves.Add(InitAutoSettingSave());
            }

            return _allSettingSaves;
        }

        public static GameDataSave GetGameDataSave(string labelSave) => GetAllGameSaves().FirstOrDefault(x => x.Label == labelSave);
        public static SettingDataSave GetSettingDataSave(string labelSave) => GetAllSettingSaves().FirstOrDefault(x => x.Label == labelSave);
        #endregion

        #region SAVE
        public static void CreateNewGameSave(GameDataSave data)
        {
            _fileName = data.ID;
            data.SaveFile(GameSavePath, FileMode.CreateNew);

            _allGameSaves.Add(data);

            Debug.Log($"Create new Game Save {data} in <i>{GamesDirectoryPath}</i>");
        }

        public static void CreateNewSettingSave(SettingDataSave data)
        {
            _fileName = data.ID;
            data.SaveFile(SettingSavePath, FileMode.CreateNew);

            _allSettingSaves.Add(data);

            Debug.Log($"Create new Setting Save {data} in <i>{GamesDirectoryPath}</i>");
        }

        public static void GameSaveAs(ref GameDataSave data)
        {
            _fileName = data.ID;
            data.SaveFile(GameSavePath, FileMode.Create);

            var id = _allGameSaves.FindIndex((x) => x.ID == _fileName);
            _allGameSaves[id] = data;

            Debug.Log($"Save game as {data} in <i>{GamesDirectoryPath}</i>");
        }

        public static void SettingSaveAs(ref SettingDataSave data)
        {
            _fileName = data.ID;
            data.SaveFile(SettingSavePath, FileMode.Create);


            var id = _allSettingSaves.FindIndex((x) => x.ID == _fileName);
            _allSettingSaves[id] = data;

            Debug.Log($"Save setting as {data} in <i>{GamesDirectoryPath}</i>");
        }
        #endregion

        #region LOAD
        //private static GameDataSave LoadGameSave(string path)
        //{
        //    GameDataSave data;

        //    if (File.Exists(path))
        //    {
        //        data = path.LoadFile<GameDataSave>();

        //        if (data.Equals(default))
        //            data.Label = "Cheater";

        //        Debug.Log($"Load {data} game successful");
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"Game save file not found in <b>{path}</b>");
        //        data = InitAutoGameSave();
        //    }

        //    return data;
        //}
        
        public static GameDataSave LoadGameSave(string name = "")
        {
            GameDataSave data = GetGameDataSave(name);

            if (data.Equals(default))
            {
                data = new(name);
                Debug.LogWarning($"Game save file {data} not found in <i>{GamesDirectoryPath}</i>");
                CreateNewGameSave(data);
            }

            Debug.Log($"Load game {data} successful");

            return data;
        }

        public static SettingDataSave LoadSettingSave(string name = "")
        {
            SettingDataSave data = GetSettingDataSave(name);

            if (data.Equals(default))
            {
                data = new(name);
                Debug.LogWarning($"Setting save file {data} not found in <i>{SettingsDirectoryPath}</i>");
                CreateNewSettingSave(data);
            }

            Debug.Log($"Load setting {data} successful");

            return data;
        }

        #endregion

        #region RESET/DELETE

        public static void ResetGameSave(ref GameDataSave data)
        {
            _fileName = data.ID;

            data = new(data.Label);
            data.SaveFile(GameSavePath, FileMode.Create);

            Debug.Log($"Reset game {data} save successful");
        }

        public static void ResetSettingSave(ref SettingDataSave data)
        {
            _fileName = data.ID;

            data = new(data.Label);
            data.SaveFile(SettingSavePath, FileMode.Create);

            Debug.Log($"Reset setting {data} save successful");
        }

        public static void DeleteGameSave(GameDataSave data)
        {
            _fileName = data.ID;

            if (File.Exists(GameSavePath))
            {
                File.Delete(GameSavePath);
                Debug.Log($"Delete game {data} successful");
            }
        }

        public static void DeleteSettingSave(SettingDataSave data)
        {
            _fileName = data.ID;

            if (File.Exists(SettingSavePath))
            {
                File.Delete(SettingSavePath);
                Debug.Log($"Delete setting {data} successful");
            }
        }
        #endregion


#if UNITY_EDITOR
        public static int GetLocalId(UnityEngine.Object obj)
        {
            PropertyInfo inspectorModeInfo =
            typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(obj);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp =
                serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

            return localIdProp.intValue;
        }
#endif
    }
}