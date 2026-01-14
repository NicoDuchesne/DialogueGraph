using System;
using System.Collections.Generic;
using UnityEngine;

namespace F2O.SaveSystem
{
    [Obsolete][Serializable]
    public class SaveData
    {
        //[Serializable]
        //public struct ProfilData
        //{
        //    public string name;
        //    public object sprite;

        //    public GameData gameData;
        //    public SettingData settingData;

        //    public ProfilData(string name, object sprite, GameData gameData, SettingData settingData)
        //    {
        //        this.name = string.IsNullOrEmpty(name) ? "New Player" : name;
        //        this.sprite = sprite;
        //        this.gameData = gameData;
        //        this.settingData = settingData;
        //    }
        //}

        //public List<ProfilData> ProfilDatas { get; private set; }

        public SaveData(string label = "New Save")
        {
            //ProfilData profil = new("", null, new(), new());
            //ProfilDatas = new();
            //ProfilDatas.Add(profil);
        }
    }
}