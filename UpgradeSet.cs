using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Used for the station shop
/// This component is stored in the scene and stores available upgrades in a separate scriptable object
/// This script is used by other components to retrieve the available upgrades from the shop
[CreateAssetMenu(fileName = "New UpgradeSet", menuName = "UpgradeSet", order = 3)]
public class UpgradeSet : ScriptableObject
{
    public string setName;
    public Upgrade[] availableUpgrades;
}
