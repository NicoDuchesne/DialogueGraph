using System;

//Real simple attribute, we know if our field has to be saved or not
[AttributeUsage(AttributeTargets.Field)]
public class SaveField : Attribute
{
    public string key;

    public SaveField(string key = null)
    {
        this.key = key;
    }
}
