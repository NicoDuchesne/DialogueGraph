using System;
using UnityEngine;

public interface ISaveData
{
    public static string PrefixID { get; }
    public string ID { get; }
    public string Label { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
