using System.Reflection;
using UnityEngine;

public static class Extentions
{
    public static void ToConsole<T>(this T[] array)
    {
        foreach (T var in array)
        {
            Debug.Log(var);
        }
    }

    public static void ToConsole(FieldInfo[] array)
    {
        foreach (FieldInfo info in array)
        {
            Debug.Log(info.Name + " : " + info.GetValue(null));
        }
    }
    
    public static void ToConsole(PropertyInfo[] array)
    {
        foreach (PropertyInfo info in array)
        {
            Debug.Log(info.Name + " : " + info.GetValue(null));
        }
    }
}