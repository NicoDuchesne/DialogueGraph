using System;
using System.Collections.Generic;
using System.Linq;

//Une data de save basique, implémentant notre interface
[Serializable]
public struct MySaveData : ISaveData
{
    public static string PrefixID => "Save_";
    public string ID { get; private set; }
    public string Label { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public List<float> _sliderValues;


    //Constructor avec nom donné
    public MySaveData(string label)
    {
        var datas = MySaveSystem.GetAllGameSaves();

        datas.ForEach(x => UnityEngine.Debug.Log(x.ID));

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
    }

    //Même constructor avec les valeurs de sliders
    public MySaveData(string label, List<float> sliderValues)
    {
        this = new MySaveData(label);
        {
            _sliderValues = sliderValues;
        }
    }


    public override string ToString() => $"<b>{Label}</b> <i>({ID + MySaveSystem.Extension})</i>";
}
