using System.Diagnostics;
using System.IO;
using UnityEngine;

public class MyButtonFunctions : MonoBehaviour
{
    public void OnClickOpenSaveDirectory()
    {
        string saveDirectoyPath = $"{Application.persistentDataPath}/Saves";
        SaveUtils.OpenDirectory(saveDirectoyPath);
    }

    public void Testing()
    {
        MySaveData mySaveData = new MySaveData("ouue");
        MySaveSystem.CreateNewGameSave(mySaveData);
    }
}
