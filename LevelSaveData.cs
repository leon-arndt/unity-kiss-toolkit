using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This serializable object is required beacause Game Levels are scriptable objects.
/// They need to be convered to save data before they can be safe.
/// </summary>
[System.Serializable]
public class LevelSaveData : SaveData
{
    public float timeLeftOnLevel;
    public float damageSustainedOnLevel;
    public int teacupsEarnedOnLevel;
    public bool earnedQuickTeacup, earnedSafeTeacup;

    //Constructors
    public LevelSaveData()
    {
    }

    public LevelSaveData(string newName, float newTimeLeft, float newDamage, int newTeacupsEarned)
    {
        dataName = newName;
        timeLeftOnLevel = newTimeLeft;
        damageSustainedOnLevel = newDamage;
        teacupsEarnedOnLevel = newTeacupsEarned;
    }
}
