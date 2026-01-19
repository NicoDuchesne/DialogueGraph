using System;
using System.Collections.Generic;
using System.Linq;

//Une data de save basique, implémentant notre interface
[Serializable]
public struct MySaveData : ISaveData
{
    //Données implémentées par l'interface
    public static string PrefixID => "Save_";
    public string ID { get; private set; }
    public string Label { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    //Slider Values in scene
    public List<float> _sliderValues;

    //Input Value in scene
    public string _inputValue;

    public List<SaveEntry> _entries;

    //Constructor avec lable, le plus utilisé
    public MySaveData(string label)
    {
        var datas = MySaveSystem.GetAllGameSaves();

        //datas.ForEach(x => UnityEngine.Debug.Log(x.ID));

        //ID
        if (datas.Count ==0) //1ere save
        {
            ID = PrefixID + 0;
        }
        else //save supplémentaires
        {
            var IDs = datas.Select(x => int.Parse(x.ID.Split('_')[1])).Max();
            ID = PrefixID + ++IDs;
        }

        Label = string.IsNullOrEmpty(label) ? ID : label; //le label est l'input ou l'ID si input null
        DateCreated = DateTime.Now;
        DateUpdated = DateCreated;

        _sliderValues = new();
        _inputValue = "";
        _entries = new();
    }

    #region Constructors
    //Constructors supplémentaires
    public MySaveData(string label, List<float> sliderValues)
    {
        this = new MySaveData(label);
        {
            _sliderValues = sliderValues;
        }
    }
    public MySaveData(string label, List<float> sliderValues, string inputValue)
    {
        this = new MySaveData(label, sliderValues);
        {
            _inputValue = inputValue;
        }
    }
    public MySaveData(string label, List<float> sliderValues, string inputValue, List<SaveEntry> entries)
    {
        this = new MySaveData(label, sliderValues, inputValue);
        {
            _entries = entries;
        }
    }
    #endregion

    public override string ToString() => $"<b>{Label}</b> <i>({ID + MySaveSystem.Extension})</i>"; //ToString
}
