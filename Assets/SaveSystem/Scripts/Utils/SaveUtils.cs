using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveUtils
{
    public static void OpenDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Process.Start(path);
        }
    }

    public static ISaveData LoadFile(this string path, Action<bool> callback = null) => path.LoadFile<ISaveData>(callback);
    public static T LoadFile<T>(this string path, Action<bool> callback = null) where T : ISaveData
    {
        T data = default;
        callback += (_) => DebugInfo(_, $"Prepare : {data} file", $"Prepare : {data} file failed !");


        try
        {
            FileStream stream = new(path, FileMode.Open);
            UnityEngine.Debug.Log("File opened OK!");
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

    public static void SaveFile<T>(this T data, string path, FileMode mode, Action<bool> callback = null) where T : ISaveData
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

    private static void DebugInfo(bool result, string success, string fail) => UnityEngine.Debug.Log(result ? success : fail);

}