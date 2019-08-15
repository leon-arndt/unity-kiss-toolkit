using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the automatic upgrading within the train station as players earn more teacups.
/// It uses a combintation of in-scene references (game objects) as well as 
/// references to scriptable objects (upgrade data for name and teacup requirement).
/// </summary>
public class UpgradeManager : MonoBehaviour {
    [SerializeField]
    private StationUpgrade[] upgrades;

	// Use this for initialization
	void Start () {
        HandleUpgrades();
    }

    [System.Serializable]
    public class StationUpgrade : System.Object
    {
        public Upgrade upgradeData;
        public GameObject gameObjectToEnable;
        public GameObject[] disableGameObjects;
    }

    void HandleUpgrades()
    {
        //foreach (StationUpgrade upgrade in upgrades)
        //{
        //    //should the upgrade be unlocked
        //    if (GameManager.instance.globalSaveData.teacupCount >= upgrade.upgradeData.teacupsNeeded)
        //    {
        //        upgrade.gameObjectToEnable.SetActive(true);
        //
        //        //disable old iterations (disable bronze train when the gold one is unlocked)
        //        foreach (var goToDisable in upgrade.disableGameObjects)
        //        {
        //            goToDisable.SetActive(false);
        //        }
        //    }
        //}
    }
}
