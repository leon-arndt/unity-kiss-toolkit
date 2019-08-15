using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interact with the train to open the level select
/// </summary>
public class LevelSelect : Interactable {
    public override void Interact(Transform transform)
    {
        StationUI.Instance.SetLevelSelectScreenVisibility(true);
    }
}
