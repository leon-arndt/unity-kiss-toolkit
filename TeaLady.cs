using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used by an NPC in the hub.
/// The player can purchase upgrades with this NPC.
/// </summary>
public class TeaLady : Interactable {
    public override void Interact(Transform transform)
    {
        StationUI.Instance.SetUpgradesScreenVisibility(true);
    }
}
