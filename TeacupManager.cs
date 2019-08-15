using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Manager decides whether player should earn Teacups at the end of the level.
/// It uses a set of defined conditions for this such as the time remaining and how much damage the vehicle sustained.
/// Works together with the GameController and the GlobalSaveData Object.
/// </summary>
public class TeacupManager : MonoBehaviour {
    public static void ResetLevelTeacups()
    {
        GameManager.instance.globalSaveData.currentLevelSaveData.teacupsEarnedOnLevel = 0;
    }

    //Figure out how many teacups the player has earned this level
    public static int DetermineNumTeacupsEarned()
    {
        int teacupsEarned = 1;

        if (ShouldEarnQuickTeacup())
        {
            teacupsEarned++;
        }

        if (ShouldEarnCarefulTeacup())
        {
            teacupsEarned++;
        }

        return teacupsEarned;
    }

    //Figure out how many new cups were earned
    public static int UpdateLevelSaveData(LevelSaveData levelSaveData)
    {
        int teacupsAlreadyEarnedInLevel = levelSaveData.teacupsEarnedOnLevel;

        int newTeacupsEarned = 0;

        //the first one is for completion
        if (teacupsAlreadyEarnedInLevel == 0)
        {
            newTeacupsEarned++;
        }

        //only earn it if you don't already have it
        if (!levelSaveData.earnedQuickTeacup)
        {
            if (ShouldEarnQuickTeacup())
            {
                newTeacupsEarned++;
                levelSaveData.earnedQuickTeacup = true;
            }
        }

        if (!levelSaveData.earnedSafeTeacup)
        {
            if (ShouldEarnCarefulTeacup())
            {
                newTeacupsEarned++;
                levelSaveData.earnedSafeTeacup = true;
            }
        }

        levelSaveData.teacupsEarnedOnLevel = teacupsAlreadyEarnedInLevel + newTeacupsEarned;
        return newTeacupsEarned;
    }
    
    
    //condition for time teacup (more than half the level time left)
    public static bool ShouldEarnQuickTeacup()
    {
        float secondsLeft = GameController.GetSecondsLeft();
        return secondsLeft > GameManager.instance.activeLevel.levelTime / 2f;
    }

    //condition for damage teacup
    public static bool ShouldEarnCarefulTeacup()
    {
        float damageSustained = VehicleController.Instance.totalDamageSustained;
        return damageSustained < 100;
    }
}
