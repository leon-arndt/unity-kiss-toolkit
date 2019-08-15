using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a very simple script used by the station statistics board in order to display them.
/// </summary>
public class StatBoard : Interactable {
    public override void Interact(Transform transform)
    {
        StationUI.Instance.SetStatsScreenVisibility(true);
    }
}
