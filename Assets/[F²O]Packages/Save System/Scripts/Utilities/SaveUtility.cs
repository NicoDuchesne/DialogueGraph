using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace F2O.SaveSystem
{
    public static class SaveUtility
    {
        public static void OpenDirectory(this string path)
        {
            if (Directory.Exists(path))
                Process.Start(path);
        }

        //public static string GetGameDataSave(string labelSave) => GetAllGameSaves().FirstOrDefault(x => x.Label == labelSave);


        internal static void SaveFile<T>(this T data, string path, FileMode mode, Action<bool> callback = null) where T : IDataSavable
        {
            var procLab = mode == FileMode.Create ? "Edit" : "Create";

            callback += (_) => DebugInfo(_, $"{procLab} : {data} file", $"{procLab} : {data} file failed !");

            try
            {
                data.DateUpdated = DateTime.Now;

                FileStream stream = new(path, mode);
                new BinaryFormatter().Serialize(stream, data);
                stream.Close();
                callback?.Invoke(true);
            }
            catch
            {
                callback?.Invoke(false);
            }
        }
        internal static IDataSavable LoadFile(this string path, Action<bool> callback = null) => path.LoadFile<IDataSavable>(callback);
        internal static T LoadFile<T>(this string path, Action<bool> callback = null) where T : IDataSavable
        {
            T data = default;
            callback += (_) => DebugInfo(_, $"Prepare : {data} file", $"Prepare : {data} file failed !");

            try
            {
                FileStream stream = new(path, FileMode.Open);
                data = (T)new BinaryFormatter().Deserialize(stream);
                stream.Close();
                callback?.Invoke(true);
            }
            catch
            {
                callback?.Invoke(false);
            }

            return data;
        }


        private static void DebugInfo(bool result, string success, string fail) => UnityEngine.Debug.Log(result ? success : fail);

        private static string GetNameFile(this string path, bool withExtension) => withExtension ? path.Split('/')[^1] : path.Split('/')[^1].Split('.')[0];
    }
}
