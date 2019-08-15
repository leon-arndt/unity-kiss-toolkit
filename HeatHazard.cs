using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is attached to game objects which appear during heatwaves.
/// This objects hurts the player when it touches them.
/// </summary>
public class HeatHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                other.GetComponent<PlayerController>().AddBurnTime(2f);
            }
        }
    }
}