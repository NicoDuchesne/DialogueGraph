using System;
using System.Collections.Generic;

//Simple data created for each monobehavior script, that will store the SaveFields in dictionnary values
[Serializable]
public class SaveEntry
{
    public string objectName;
    public string componentType;
    public Dictionary<string, string> values = new();
}