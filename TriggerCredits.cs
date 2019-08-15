using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple trigger which shows the credits when entered by the player.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerCredits : MonoBehaviour {
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                StationUI.Instance.PlayCinematicFadeToWhite();
            }
        }
    }
}
