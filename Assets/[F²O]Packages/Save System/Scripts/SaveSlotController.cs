using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace F2O.SaveSystem
{
    public class SaveSlotController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _txtSaveName;
        [SerializeField] private TMP_Text _txtDateCreated;
        [SerializeField] private TMP_Text _txtDateUpdated;

        [SerializeField] private TMP_InputField _ipfRename;
        private bool _renaming;

        private GameDataSave data;

        public void Init(GameDataSave data)
        {
            this.data = data;
            OnRefreshUI();
        }

        public void OnSaveAs()
        {
            //data = SaveSystem.GetGameDataSave(_txtSaveName.text);

            SaveManager.GameDataToSave(ref data);
            SaveSystem.GameSaveAs(ref data);

            OnRefreshUI();
        }

        public void OnLoad()
        {
            data = SaveSystem.LoadGameSave(_txtSaveName.text);
            SaveManager.GameDataToLoad(data);
        }

        public void OnRename()
        {
            _renaming = !_renaming;

            if (_renaming)
            {
                _ipfRename.text = data.Label;
            }
            else
            {
                data.Label = _ipfRename.text;

                SaveSystem.GameSaveAs(ref data);
                OnRefreshUI();
            }

            _ipfRename.gameObject.SetActive(_renaming);
        }


        public void OnDelete()
        {
            SaveSystem.DeleteGameSave(data);
            Destroy(gameObject);
        }

        private void OnRefreshUI()
        {
            _txtSaveName.text = data.Label;
            _txtDateCreated.text = $"Created : {data.DateCreated}";
            _txtDateUpdated.text = $"Updated : {data.DateUpdated}";
        }
    }
}
