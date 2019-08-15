using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Used or saving and loading individual levels.
/// </summary>
public class SaveManager : MonoBehaviour {
    //global save and load
    public static void SaveGlobal()
    {
        BinaryDataSaver.SaveData(GameManager.instance.globalSaveData);
        Debug.Log("Save the game globally");
    }

    public static void LoadGlobal()
    {
        //only open if existing
        if ((GlobalSaveData)BinaryDataSaver.LoadData(GameManager.instance.globalSaveData) != null)
        {
            GameManager.instance.globalSaveData = (GlobalSaveData)BinaryDataSaver.LoadData(GameManager.instance.globalSaveData);
        }
    }
    
    
    //retrieve data by finding the matching element in the level save data list
    public static LevelSaveData GetLevelData(GameLevel level)
    {
        GlobalSaveData globalSaveData = GameManager.instance.globalSaveData;

        List<LevelSaveData> levelSaveDataList = new List<LevelSaveData>(globalSaveData.levelSaveData);
        LevelSaveData data = levelSaveDataList.Where(obj => obj.dataName == level.levelName).SingleOrDefault();
        return data;
    }

    //required for proper teacup progress to be saved
    public static void UpdateCurrentLevelDataWithPrevious()
    {
        LevelSaveData previousLevelData = SaveManager.GetLevelData(GameManager.instance.activeLevel);
        GameManager.instance.globalSaveData.currentLevelSaveData.teacupsEarnedOnLevel = previousLevelData.teacupsEarnedOnLevel;
        GameManager.instance.globalSaveData.currentLevelSaveData.earnedQuickTeacup = previousLevelData.earnedQuickTeacup;
        GameManager.instance.globalSaveData.currentLevelSaveData.earnedSafeTeacup = previousLevelData.earnedSafeTeacup;
    }

    public static void SaveLevelData(GameLevel level)
    {
        GlobalSaveData globalSaveData = GameManager.instance.globalSaveData;

        //look for a matching data piece in the level list
        LevelSaveData data = globalSaveData.levelSaveData.Where(obj => obj.dataName == level.levelName).SingleOrDefault();

        if (data != null)
        {
            //update global level save data again
            data.damageSustainedOnLevel = globalSaveData.damageSustainedOnLevel;
            data.earnedQuickTeacup = globalSaveData.currentLevelSaveData.earnedQuickTeacup;
            data.earnedSafeTeacup = globalSaveData.currentLevelSaveData.earnedSafeTeacup;
            data.timeLeftOnLevel = globalSaveData.timeLeftOnLevel;
        }
        else
        {
            //create a new one
            LevelSaveData newData = new LevelSaveData();
            newData.dataName = level.levelName;
            newData.damageSustainedOnLevel = globalSaveData.damageSustainedOnLevel;
            newData.teacupsEarnedOnLevel = globalSaveData.currentLevelSaveData.teacupsEarnedOnLevel;
            newData.earnedQuickTeacup = globalSaveData.currentLevelSaveData.earnedQuickTeacup;
            newData.earnedSafeTeacup = globalSaveData.currentLevelSaveData.earnedSafeTeacup;
            newData.timeLeftOnLevel = globalSaveData.timeLeftOnLevel;
            globalSaveData.levelSaveData.Add(newData);
        }
    }
}
