using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct MySaveData : ISaveData
{
    public static string PrefixID => "GDS_";
    public string ID { get; private set; }
    public string Label { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public List<float> _sliderValues;


    public MySaveData(string label)
    {
        var datas = MySaveSystem.GetAllGameSaves();

        datas.ForEach(x => UnityEngine.Debug.Log(x.ID));

        if (datas.Count ==0)
        {
            ID = PrefixID + 0;
        }
        else
        {
            var IDs = datas.Select(x => int.Parse(x.ID.Split('_')[1])).Max();
            ID = PrefixID + ++IDs;
        }

        Label = string.IsNullOrEmpty(label) ? ID : label;
        DateCreated = DateTime.Now;
        DateUpdated = DateCreated;

        _sliderValues = new();
    }

    public MySaveData(string label, List<float> sliderValues)
    {
        this = new MySaveData(label);
        {
            _sliderValues = sliderValues;
        }
    }

    public override string ToString() => $"<b>{Label}</b> <i>({ID + MySaveSystem.Extension})</i>";
}
