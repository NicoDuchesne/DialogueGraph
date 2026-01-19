using System;

[AttributeUsage(AttributeTargets.Field)]
public class SaveField : Attribute
{
    public string key;

    public SaveField(string key = null)
    {
        this.key = key;
    }
}
