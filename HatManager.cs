using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This controls the hats the players wear
/// </summary>
public class HatManager : MonoBehaviour {
    public static PlayerController playerControllerToWorkWith;

    public static void UpdateCurrentHat(Hat newHat)
    {
        Destroy(playerControllerToWorkWith.hat);
        var hatToCreate = Instantiate(
        newHat.hatObject,
        playerControllerToWorkWith.transform.position + newHat.positionOffset,
        playerControllerToWorkWith.transform.rotation,
        playerControllerToWorkWith.transform); //parent

        hatToCreate.transform.SetParent(playerControllerToWorkWith.hatParentTransform);
        hatToCreate.transform.localScale = newHat.scaleFactor * hatToCreate.transform.localScale;
        playerControllerToWorkWith.hat = hatToCreate;
    }

    public static void UpdateGlobalDataWithNewHat(Hat newHat)
    {
        if (playerControllerToWorkWith.playerID == 0)
        {
            GameManager.instance.persistentData.p1CurrentHat = newHat;
        }
        else
        {
            GameManager.instance.persistentData.p2CurrentHat = newHat;
        }
    }

    public static void OutfitBothplayers()
    {
        playerControllerToWorkWith = Discovery.GetPlayer(0);
        if (GameManager.instance.persistentData.p1CurrentHat != null)
        UpdateCurrentHat(GameManager.instance.persistentData.p1CurrentHat);

        playerControllerToWorkWith = Discovery.GetPlayer(1);
        if (GameManager.instance.persistentData.p2CurrentHat != null)
            UpdateCurrentHat(GameManager.instance.persistentData.p2CurrentHat);
    }
}
