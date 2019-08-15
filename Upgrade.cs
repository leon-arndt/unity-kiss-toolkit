using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is used for station upgrades.
/// Upgrades include new trains, fountains, and station items such as the gumball machine and stats board
/// </summary>
[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade", order = 3)]
public class Upgrade : ScriptableObject {
    public string upgradeName;
    public string upgradeDescription;
    public Texture2D upgradeTexture;
    public int goldCost;
}