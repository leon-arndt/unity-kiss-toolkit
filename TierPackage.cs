using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A TierPackage is a series of game levels. They could be thought of as all the levels in a Mario World.
/// Packaging them like this makes it easier for modders to get their hands dirty and add new levels.
/// </summary>
[CreateAssetMenu(fileName = "New Tier Package", menuName = "Tier", order = 3)]
public class TierPackage : ScriptableObject
{
    public string tierName;

    private const int SIZE = 6;
    public GameLevel[] gameLevels = new GameLevel[SIZE];

    //Make sure designers never add more than six levels to a tier
    void OnValidate()
    {
        if (gameLevels.Length != SIZE)
        {
            Debug.LogWarning("Don't change the tier array size!");
            Array.Resize(ref gameLevels, SIZE);
        }
    }
}
