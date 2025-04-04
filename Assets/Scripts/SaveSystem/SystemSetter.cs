﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using SaveSystem;
using UnityEngine;

public class SystemSetter : MonoBehaviour
{
    public SaveType SaveType;
    private List<ISavable> _savables = new List<ISavable>();
    private void Awake()
    {
        if (SaveType == SaveType.Prefs)
        {
            SetSavablesUp(new PrefsSaveLoadSystem());
        }
        else if (SaveType == SaveType.Json)
        {
            SetSavablesUp(new JsonSaveLoadSystem());
        }
        else
        {
            Debug.LogError("SaveType hasn't assigned");
        }
    }

    public void AddSavable(ISavable savable)
    {
        _savables.Add(savable);
    }

    private void SetSavablesUp(ISaveLoadSystem saveLoadSystem)
    {
        foreach (ISavable savable in _savables)
        {
            savable.SaveState += saveLoadSystem.Save;
            savable.LoadState += saveLoadSystem.Load;
        }
    }
}