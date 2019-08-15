using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactor script for the encyclopedia / glossary lady
/// </summary>
public class SmartTurtle : Interactable {

    public override void Interact(Transform transform)
    {
        StationUI.Instance.SetEncyclopediaScreenVisibility(true);
    }
}
