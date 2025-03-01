using System;
using UnityEngine;
using Object = System.Object;

public interface ISavable
{
    string Name {get; set;}
    public void Save();
    public void Load(out object State);
    public event Action<object, string> SaveState;
    public event Func<string, Type, object> LoadState;
}
