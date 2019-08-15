using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Used for the interaction gameObject inside the scene. Work together with the UIStationShop.
/// </summary>
public class StationShop : Interactable {
    public static StationShop Instance;

    public UpgradeSet currentUpgradeSet;
    public List<Upgrade> availableUpgrades;
    public List<Upgrade> purchasedUpgrades;
    public List<SceneUpgrade> upgradePool;
    
    // Use this for initialization
	void Start () {
        Instance = this;
        //Load save data?
        InitSceneData();
        LoadPurchasedUpgradeFromSaveData();
        EnableAllPurchasedSceneObjects();
	}

    //Slow method which autopopulates the available upgrades and upgrade pool list from Scene Data, solves the script execution order problem
    void InitSceneData()
    {
        //upgrade pool
        upgradePool.Clear();
        //availble Upgrades
        availableUpgrades.Clear();

        List<SceneUpgrade> foundSceneUpgrades = FindObjectsOfType<SceneUpgrade>().ToList();
        foreach (var item in foundSceneUpgrades)
        {
            upgradePool.Add(item);
            availableUpgrades.Add(item.upgrade);
        }
    }
    void LoadPurchasedUpgradeFromSaveData()
    {
        foreach (var item in availableUpgrades)
        {
            if (AlreadyPurchased(item))
            {
                Debug.Log(item.upgradeName + "Was previously purchased in SaveData");
                //add match to purchased upgrades
                purchasedUpgrades.Add(item);
            }
        }
    }

    //Go through the upgrade pool and compare against the purchased items to enable desired game objects
    public void EnableAllPurchasedSceneObjects()
    {
        Debug.Log("Attempting to enable all purchased scene objects");

        //disable all gameobjects in the entire upgrade pool
        foreach (var poolItem in upgradePool)
        {
            poolItem.gameObject.SetActive(false);
        }
        
        //create a purchased items names list
        List<string> purchasedItemsNames = new List<string>();

        //add all purchased items
        foreach (var purchasedItem in purchasedUpgrades)
        {
            purchasedItemsNames.Add(purchasedItem.upgradeName);
        }

        //check the entire upgrade pool and activate scene upgrades
        foreach (var poolItem in upgradePool)
        {
            if (purchasedItemsNames.Contains(poolItem.gameObject.name))
            {
                //if an item in the upgrade pool (its name) is also in the purchased upgrades then the upgrade pool gameobject should be enabled
                poolItem.gameObject.SetActive(true);
            }
        }
    }

    public override void Interact(Transform transform)
    {
        OpenShop();
    }

    private void OpenShop()
    {
        UIStationShop.Instance.OpenShop();
    }

    public void BuyUpgrade(Upgrade upgradeToBuy)
    {
        //TODO Callback if sucesfull to update the button visuals
        //Update the shop save data
        //Save through the savemanager
        if (GameManager.instance.globalSaveData.money > upgradeToBuy.goldCost)
        {
            //only purchase if not already owned (can only have one copy)
            if (!AlreadyPurchased(upgradeToBuy))
            {
                //subtract gold
                GameManager.instance.globalSaveData.money -= upgradeToBuy.goldCost;
                purchasedUpgrades.Add(upgradeToBuy);
                EnableAllPurchasedSceneObjects(); //could be optimized, O speed
                SaveUpgradeData(upgradeToBuy);

                //UI for gold and button changes callback
                UIStationShop.Instance.currentGoldTextUpdater.UpdateTextByEnum();
            }
        }
        else
        {
            Debug.Log("Not enough money to buy" + upgradeToBuy.name);
        }
    }

    //Check if upgrade has already been bought
    public static bool AlreadyPurchased(Upgrade upgradeToSave)
    {
        GlobalSaveData globalSaveData = GameManager.instance.globalSaveData;
        //look for a matching data piece in the level list
        ShopSaveData data = globalSaveData.shopSaveData.Where(obj => obj.dataName == upgradeToSave.upgradeName).SingleOrDefault();
        if (data != null)
        {
            //update global level save data again
            return true;
        }
        else
            return false;
    }

    //Save the new data, based on the level save data approach
    public void SaveUpgradeData(Upgrade upgradeToSave)
    {
        Debug.Log("Attempting to save shop data");
        GlobalSaveData globalSaveData = GameManager.instance.globalSaveData;

        //look for a matching data piece in the level list
        ShopSaveData data = globalSaveData.shopSaveData.Where(obj => obj.dataName == upgradeToSave.upgradeName).SingleOrDefault();

        if (data != null)
        {
            //update global level save data again
            data.unlocked = true;
        }
        else
        {
            //create a new one using default constructor
            ShopSaveData newData = new ShopSaveData();
            newData.dataName = upgradeToSave.upgradeName;
            newData.unlocked = true;
            globalSaveData.shopSaveData.Add(newData);
        }

        SaveManager.SaveGlobal();
    }
}
