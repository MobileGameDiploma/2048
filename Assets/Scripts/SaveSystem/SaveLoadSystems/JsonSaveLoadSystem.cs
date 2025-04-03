using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public class JsonSaveLoadSystem : ISaveLoadSystem
    {
        public void Save(object saveData, string name)
        {
            string NameLayout = name + saveData.GetType();
            string json = JsonUtility.ToJson(saveData);
            Debug.Log(json);
            File.WriteAllText(Application.persistentDataPath + "/" + NameLayout + ".json", json);
        }

        public object Load(string name, Type type)
        {
            object result = Activator.CreateInstance(type);
            string NameLayout = name + type;
            string json = File.ReadAllText(Application.persistentDataPath + "/" + NameLayout + ".json");
            result = JsonUtility.FromJson(json, type);
            return result;
        }
    }
}