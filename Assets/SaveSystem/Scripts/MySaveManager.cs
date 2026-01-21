using System.Linq;
using System.Reflection;
using UnityEngine;

public class MySaveManager : MonoBehaviour
{
    //Display saves
    public GameObject _saveDisplayParent;
    public GameObject _saveDisplayPrefab;

    //Save Name Input
    public TMPro.TMP_InputField _saveInputName;
    private string _saveName;
    public void OnNameInput()
    {
        _saveName = _saveInputName.text;
    }

    //Initializations
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

    //Create New Save
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


    //Add all the values into the given SaveData
    public static void GameDataToSave(ref MySaveData data)
    {
        Debug.Log("Add data to save " + data.Label);

        //Sliders values
        data._sliderValues = new();
        foreach (var slider in SaveValues.Instance._sdValue)
        {
            data._sliderValues.Add(slider.value);
        }

        //Input field value
        data._inputValue = "";
        data._inputValue = SaveValues.Instance.InputValue;

        //SaveFields values
        foreach (var mono in FindObjectsOfType<MonoBehaviour>()) //In each monobehavior
        {
            //Get the fields inside
            var fields = mono.GetType().GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic
            );

            //Create SaveEntry for this monobehavior
            SaveEntry entry = new SaveEntry
            {
                objectName = mono.gameObject.name,
                componentType = mono.GetType().AssemblyQualifiedName
            };

            bool hasData = false;

            foreach (var field in fields)//In each field
            {
                //We are only intersted in fields with the SaveField attribute
                var attr = field.GetCustomAttribute<SaveField>();
                if (attr == null) continue;

                string editorKey =
                    $"{mono.gameObject.name}/{mono.GetType().Name}/{field.Name}";

                //We check with the menu and window, if we want to save that specific SaveField
#if UNITY_EDITOR
                if (!SaveFieldWindow.IsFieldEnabled(editorKey))
                    continue;
#endif

                //Finally, the field is added in the SaveEntry
                string key = attr.key ?? field.Name;
                object value = field.GetValue(mono);

                entry.values[key] = value.ToString();
                hasData = true;

                Debug.Log($"[SAVE] {mono.gameObject.name} | {mono.GetType().Name} | {key} = {value}");
            }

            //If there is wanted SaveFields inside the monobehavior, add the SaveEntry in list of entries
            if (hasData)
            {
                //check if we have to create or update the entry
                var existing = data._entries.FirstOrDefault(e => e.objectName == entry.objectName);

                if (existing != null)
                {
                    int index = data._entries.IndexOf(existing);
                    data._entries[index] = entry;
                }
                else
                {
                    data._entries.Add(entry);
                }
            }
        }


    }

    public static void GameDataToLoad(MySaveData data)
    {
        Debug.Log("Load data in save " + data.Label);

        //Slider Values put in the scene sliders
        for (int i = 0; i < data._sliderValues?.Count; i++)
        {
            SaveValues.Instance._sdValue[i].value = data._sliderValues[i];
        }

        //Input Value put in the scene input
        SaveValues.Instance._inptValue.text = data._inputValue;

        //SaveFields Values :
        //we print them in DebugLog to check what we have in the List Entries
        foreach (var entry in data._entries)
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
                if (!entry.values.TryGetValue(key, out string value))
                    continue;

                //Un peu faible, on ne gère que quatre types actuellement de manière dure
                if (field.FieldType == typeof(int))
                    field.SetValue(mono, int.Parse(value));
                else if (field.FieldType == typeof(float))
                    field.SetValue(mono, float.Parse(value));
                else if (field.FieldType == typeof(bool))
                    field.SetValue(mono, bool.Parse(value));
                else if (field.FieldType == typeof(string))
                    field.SetValue(mono, value);

                Debug.Log($"[LOAD] {entry.objectName} | {type.Name} | {key} = {value}");
            }


        }
    }


    public void OnOpenFolder() => MySaveSystem.OnOpenDirectory();

}
