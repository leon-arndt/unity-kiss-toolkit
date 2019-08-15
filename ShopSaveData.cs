using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a very simple data object to save the purchased shop items using their dataName and unlocked bool
/// The component has no functionality beyond this
/// </summary>
[System.Serializable]
public class ShopSaveData : SaveData {
    public bool unlocked;

    //Constructors
    public ShopSaveData()
    {
    }

    public ShopSaveData(string newName, bool newUnlocked)
    {
        dataName = newName;
        unlocked = newUnlocked;
    }
}
