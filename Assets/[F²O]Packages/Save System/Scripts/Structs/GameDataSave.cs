using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace F2O.SaveSystem
{

    [Serializable]
    public struct GameDataSave : IDataSavable
    {
        public static string PrefixID => "GDS_";
        public string ID { get ; private set; }
        public string Label { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public List<float> _sliderValues;
        public string labelPokemon;
        public string spritePath;

        public GameDataSave(string label)
        {
            var datas = SaveSystem.GetAllGameSaves();

            datas.ForEach(x => UnityEngine.Debug.Log(x.ID));
            if (label == SaveSystem.AutoSaveLabel)
                ID = PrefixID + 0;
            else
            {
                var IDs = datas.Select(x => int.Parse(x.ID.Split('_')[1])).Max();
                ID = PrefixID + ++IDs;
            }

            Label = string.IsNullOrEmpty(label) ? ID : label;
            DateCreated = DateTime.Now;
            DateUpdated = DateCreated;

            _sliderValues = new();
            labelPokemon = string.Empty;
            spritePath = null;
        }

        public GameDataSave(string label, List<float> sliderValues, string imgPath)
        {
            this = new GameDataSave(label);
            {
                _sliderValues = sliderValues;
                labelPokemon = label;
                spritePath = imgPath;
            }
        }

        public override string ToString() => $"<b>{Label}</b> <i>({ID+SaveSystem.Extension})</i>";
    }
}
