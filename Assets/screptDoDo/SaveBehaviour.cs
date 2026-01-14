using System;
using System.Collections.Generic;

[Serializable]
public class SaveEntry
{
    public string objectName;
    public string componentType;
    public Dictionary<string, string> values = new();
}

[Serializable]
public class SaveFile
{
    public List<SaveEntry> entries = new();
}