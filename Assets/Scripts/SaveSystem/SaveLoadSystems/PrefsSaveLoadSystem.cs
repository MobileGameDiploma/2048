using System;
using UnityEngine;
using Object = System.Object;

namespace SaveSystem
{
    public class PrefsSaveLoadSystem : ISaveLoadSystem
    {
        public void Save(object saveData, string name)
        {
            SaveObject(saveData, name);
        }
        
        public object Load(string name, Type type)
        {
            object result = Activator.CreateInstance(type);
            string NameToLoad = name + type;
            
            LoadObject(NameToLoad, result);
            
            return result;
        }
        
        
        private void SaveObject(object obj, string name)
        {
            string NameLayout = name + obj.GetType();
            
            foreach (var prop in obj.GetType().GetProperties())
            {
                Type type = prop.PropertyType;
                string currentName = NameLayout + prop.Name;

                switch (type.ToString())
                {
                    case "System.Int32":
                        PlayerPrefs.SetInt(currentName, (int)prop.GetValue(obj));
                        break;
                    case "System.String":
                        PlayerPrefs.SetString(currentName, Convert.ToString(Convert.ToString(prop.GetValue(obj))));
                        break;
                    case "System.Single":
                        PlayerPrefs.SetFloat(currentName, (float)prop.GetValue(obj));
                        break;
                    default:
                        SaveObject(prop.GetValue(obj), currentName);
                        break;
                }
                // if (type.ToString() == "System.Int32")
                // {
                //     PlayerPrefs.SetInt(currentName, (int)prop.GetValue(obj));
                // }
                // else if (type.ToString() == "System.String")
                // {
                //     PlayerPrefs.SetString(currentName, Convert.ToString(Convert.ToString(prop.GetValue(obj))));
                // }
                // else if (type.ToString() == "System.Single")
                // {
                //     PlayerPrefs.SetFloat(currentName, (float)prop.GetValue(obj));
                // }
                // else
                // {
                //     SaveObject(prop.GetValue(obj), currentName);
                // }
            }
        }

        private void LoadObject(string name, object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                Type propType = prop.PropertyType;
                string currentName = name + prop.Name;


                switch (propType.ToString())
                {
                    case "System.Int32":
                        prop.SetValue(obj, PlayerPrefs.GetInt(currentName));
                        break;
                    case "System.Single":
                        prop.SetValue(obj, PlayerPrefs.GetFloat(currentName));
                        break;
                    case "System.String":
                        prop.SetValue(null, PlayerPrefs.GetString(currentName));
                        break;
                    default:
                        LoadObject(currentName, prop.GetValue(obj));
                        break;
                }
                // if (propType.ToString() == "System.Int32")
                // {
                //     prop.SetValue(obj, PlayerPrefs.GetInt(currentName));
                // }
                // else if (propType.ToString() == "System.Single")
                // {
                //     prop.SetValue(obj, PlayerPrefs.GetFloat(currentName));
                // }
                // else if (propType.ToString() == "System.String")
                // {
                //     prop.SetValue(null, PlayerPrefs.GetString(currentName));
                // }
                // else
                // {
                //     LoadObject(currentName, prop.GetValue(obj));
                // }
            }
        }
    }
}