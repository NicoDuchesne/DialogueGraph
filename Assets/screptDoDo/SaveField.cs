using UnityEngine;

public class SaveField : PropertyAttribute
{
    public string key;

    public SaveField(string key = null)
    {
        this.key = key;
    }
}
