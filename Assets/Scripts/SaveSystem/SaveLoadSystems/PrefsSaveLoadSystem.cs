using System;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class PrefsSaveLoadSystem : ASaveLoadSystem
    {
        public override void Save(object saveData, string name)
        {
            SaveObject(saveData, name);
        }
        
        public override object Load(string name, Type type)
        {
            object result = Activator.CreateInstance(type);
            string NameToLoad = name + type;
            
            LoadObject(NameToLoad, result);
            
            return result;
        }
        
        
        private void SaveObject(object Object, string name)
        {
            string NameLayout = name + Object.GetType();
            
            foreach (var prop in Object.GetType().GetProperties())
            {
                Type type = prop.PropertyType;
                string currentName = NameLayout + prop.Name;
                
                if (type.ToString() == "System.Int32")
                {
                    PlayerPrefs.SetInt(currentName, (int)prop.GetValue(Object));
                }
                else if (type.ToString() == "System.String")
                {
                    PlayerPrefs.SetString(currentName, Convert.ToString(Convert.ToString(prop.GetValue(Object))));
                }
                else if (type.ToString() == "System.Single")
                {
                    PlayerPrefs.SetFloat(currentName, (float)prop.GetValue(Object));
                }
                else
                {
                    SaveObject(prop.GetValue(Object), currentName);
                }
            }
        }

        private void LoadObject(string name, object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                Type propType = prop.PropertyType;
                string currentName = name + prop.Name;
                
                if (propType.ToString() == "System.Int32")
                {
                    prop.SetValue(obj, PlayerPrefs.GetInt(currentName));
                }
                else if (propType.ToString() == "System.Single")
                {
                    prop.SetValue(obj, PlayerPrefs.GetFloat(currentName));
                }
                else if (propType.ToString() == "System.String")
                {
                    prop.SetValue(null, PlayerPrefs.GetString(currentName));
                }
                else
                {
                    LoadObject(currentName, prop.GetValue(obj));
                }
            }
        }
    }
}