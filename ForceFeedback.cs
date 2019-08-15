using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

/// <summary>
/// This script works together with Rewired to handle controller vibration
/// </summary>
public class ForceFeedback : MonoBehaviour {
    public static void Vibrate(Player player)
    {
        // Set vibration in all Joysticks assigned to the Player
        int motorIndex = 0; // the first motor
        float motorLevel = 1.0f; // full motor speed
        float duration = 1.0f; // vibration duration

        player.SetVibration(motorIndex, motorLevel, duration);
    }
}
