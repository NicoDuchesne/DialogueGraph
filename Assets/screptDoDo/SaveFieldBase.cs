using System;

[AttributeUsage(AttributeTargets.Field)]
public abstract class SaveFieldBase : Attribute
{
    public string key;

    protected SaveFieldBase(string key = null)
    {
        this.key = key;
    }
}


public class SaveFieldA : SaveFieldBase
{
    public SaveFieldA(string key = null) : base(key) { }
}

public class SaveFieldB : SaveFieldBase
{
    public SaveFieldB(string key = null) : base(key) { }
}

public class SaveFieldC : SaveFieldBase
{
    public SaveFieldC(string key = null) : base(key) { }
}

public class SaveFieldD : SaveFieldBase
{
    public SaveFieldD(string key = null) : base(key) { }
}