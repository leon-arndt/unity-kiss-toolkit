using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData : SaveData
{
    public int teacupCount;
    public int money;

    //used for saving level data
    public float timeLeftOnLevel;
    public float damageSustainedOnLevel;

    public List<LevelSaveData> levelSaveData;
    public LevelSaveData currentLevelSaveData;

    //general data
    public List<string> encounteredIngredients;
    public Statistics stats;
    public QuestData questData;
    public List<ShopSaveData> shopSaveData;
}
