using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class used to find the desired PlayerController in any scene.
/// </summary>
public class Discovery : MonoBehaviour {
    public static PlayerController GetPlayer(int playerID)
    {
        if (GameController.instance != null)
        {
            return (playerID == 0) ? GameController.instance.player1 : GameController.instance.player2;
        }
        else
        {
            return (playerID == 0) ? TrainStation.instance.player1 : TrainStation.instance.player2;
        }
    }
}
