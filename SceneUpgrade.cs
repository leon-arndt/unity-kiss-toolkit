using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is a scene component used by the upgrade system
/// All objects are disabled by default and then loaded by the stationshop
/// The objects add themselves to the station available upgrades (to prevent manual errors), not possible because of wrapped script execution order
/// This is a very flexible approach, the drawback is script execution order
/// </summary>
public class SceneUpgrade : MonoBehaviour {
    public Upgrade upgrade;

	// Use this for initialization
	void Start () {
        //StationShop.Instance.availableUpgrades.Add(this.upgrade);
        gameObject.name = upgrade.upgradeName;
	}
}
