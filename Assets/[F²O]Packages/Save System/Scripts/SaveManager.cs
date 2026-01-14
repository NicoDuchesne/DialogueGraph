using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace F2O.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        //public static SaveManager Instance { get; private set; }

        public bool autoSaveEnabled;

        [Header("SLOT")]
        public GameObject _saveSlotParent;
        public GameObject _saveSlotPrefab;
        public string NameInput { get; set; }

        private void Awake()
        {
            //if(Instance == null)
            //    Instance = this;

            SaveSystem.InitDirectories();
            SaveSystem.InitAutoGameSave();
            SaveSystem.InitAutoSettingSave();

            InitSaveSlots();
        }

        private void Start()
        {
            //if (autoSaveEnabled)
            //    GameDataToLoad(SaveSystem.LoadGameSave(SaveSystem.AutoSaveLabel));
        }

        private void OnApplicationQuit()
        {
            if (autoSaveEnabled)
            {
                var data = SaveSystem.GetGameDataSave(SaveSystem.AutoSaveLabel);
                data.Label = SaveSystem.AutoSaveLabel;

                GameDataToSave(ref data);
                SaveSystem.GameSaveAs(ref data);
            }
        }

        private void InitSaveSlots()
        {
            var allSaveSort = SaveSystem.GetAllGameSaves()?.OrderByDescending(x => x.DateUpdated.ToString());

            foreach (var saveData in allSaveSort)
            {
                var newSaveSlot = Instantiate(_saveSlotPrefab, _saveSlotParent.transform);
                if (newSaveSlot.TryGetComponent(out SaveSlotController ssc))
                {
                    ssc.Init(saveData);
                }
            }
        }

        public void OnNewSave()
        {
            var newSave = Instantiate(_saveSlotPrefab, _saveSlotParent.transform);

            GameDataSave gsd = new(NameInput);
            GameDataToSave(ref gsd);

            if (newSave.TryGetComponent(out SaveSlotController ssc))
            {
                SaveSystem.CreateNewGameSave(gsd);
                ssc.Init(gsd);
            }
        }

        public static void GameDataToSave(ref GameDataSave data)
        {
            data._sliderValues = new();
            foreach (var slider in ExempleSaveValues.Instance._sdValue)
                data._sliderValues.Add(slider.value);

            data.labelPokemon = ExempleSaveValues.Instance.ifdPokemon.text;
            if (ExempleSaveValues.Instance.imgPokemon.sprite != null)
            {
                var path = $"Sprites/{ExempleSaveValues.Instance.imgPokemon.sprite?.name}";
                data.spritePath = path;
            }
        }

        public static void GameDataToLoad(GameDataSave data)
        {
            for (int i = 0; i < data._sliderValues?.Count; i++)
                ExempleSaveValues.Instance._sdValue[i].value = data._sliderValues[i];

            ExempleSaveValues.Instance.ifdPokemon.text = data.labelPokemon;
            //var obj = AssetDatabase.LoadAssetAtPath(data.spritePath, typeof(Sprite)) as Sprite;

            ExempleSaveValues.Instance.imgPokemon.sprite = Resources.Load<Sprite>(data.spritePath);
        }

        public void OnOpenFolder() => SaveSystem.OnOpenDirectory();
    }
}