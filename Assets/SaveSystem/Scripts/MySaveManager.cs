
using F2O.SaveSystem;
using System.Linq;
using UnityEngine;

public class MySaveManager : MonoBehaviour
{
    public GameObject _saveDisplayParent;
    public GameObject _saveDisplayPrefab;
    public TMPro.TMP_InputField _saveInputName;

    private string _saveName;
    public void OnNameInput()
    {
        _saveName = _saveInputName.text;
    }

    private void Awake()
    {
        MySaveSystem.InitDirectories();

        InitSaveSlots();
    }

    private void InitSaveSlots()
    {
        var allSaveSort = MySaveSystem.GetAllGameSaves()?.OrderByDescending(x => x.DateUpdated.ToString());

        foreach (var saveData in allSaveSort)
        {
            var newSaveSlot = Instantiate(_saveDisplayPrefab, _saveDisplayParent.transform);
            if (newSaveSlot.TryGetComponent(out SaveDisplay ssc))
            {
                ssc.Init(saveData);
            }
        }
    }

    public void OnNewSave()
    {
        var newSave = Instantiate(_saveDisplayPrefab, _saveDisplayParent.transform);

        MySaveData gsd = new(_saveName);
        GameDataToSave(ref gsd);

        if (newSave.TryGetComponent(out SaveDisplay ssc))
        {
            MySaveSystem.CreateNewGameSave(gsd);
            ssc.Init(gsd);
        }
    }

    public static void GameDataToSave(ref MySaveData data)
    {
        data._sliderValues = new();
        foreach (var slider in SaveValues.Instance._sdValue)
        {
            data._sliderValues.Add(slider.value);
        }
            
    }

    public static void GameDataToLoad(MySaveData data)
    {
        for (int i = 0; i < data._sliderValues?.Count; i++)
        {
            SaveValues.Instance._sdValue[i].value = data._sliderValues[i];
        }
    }


    public void OnOpenFolder() => MySaveSystem.OnOpenDirectory();

}
