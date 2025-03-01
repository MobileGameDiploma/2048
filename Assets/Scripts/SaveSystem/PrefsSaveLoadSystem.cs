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
                
                if (type.ToString() == "int")
                {
                    PlayerPrefs.SetInt(currentName, (int)prop.GetValue(Object));
                }
                else if (type.ToString() == "float")
                {
                    PlayerPrefs.SetString(currentName, Convert.ToString(prop.GetValue(Object)));
                }
                else if (type.ToString() == "float")
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
                
                if (propType.ToString() == "int")
                {
                    prop.SetValue(PlayerPrefs.GetInt(currentName), 0);
                }
                else if (propType.ToString() == "float")
                {
                    prop.SetValue(PlayerPrefs.GetFloat(currentName), 0);
                }
                else if (propType.ToString() == "float")
                {
                    prop.SetValue(PlayerPrefs.GetString(currentName), 0);
                }
                else
                {
                    LoadObject(currentName, prop.GetValue(obj));
                }
            }
        }
    }
}