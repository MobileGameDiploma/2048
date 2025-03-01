using System;

public abstract class ASaveLoadSystem
{
    public abstract void Save(object saveData, string name);
    public abstract object Load(string name, Type type);
}