using System;

//Interface de nos save data, ce sont donc les informations les plus basiques que toutes les data doivent avoir
public interface ISaveData
{
    public static string PrefixID { get; }
    public string ID { get; }
    public string Label { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
