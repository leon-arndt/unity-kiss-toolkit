using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for the UI aspect of the shop. Is controlled by the StationShop interactable.
/// </summary>
public class UIStationShop : MonoBehaviour {
    [SerializeField]
    GameObject shopScreen, upgradeGroup, upgradeTemplate;

    [SerializeField]
    Text upgradeNameText, upgradeDescriptionText, upgradeCostText;

    [SerializeField]
    Image upgradeCloseupImage;

    [SerializeField]
    Upgrade defaultUpgrade;

    public UITextUpdater currentGoldTextUpdater;

    public static UIStationShop Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Init()
    {
        //CreateShopUpgrades();
        UpdateUpgradeInfo(defaultUpgrade);
    }
    
    //Called by Station Shop
    public void OpenShop()
    {
        Init();
        shopScreen.SetActive(true);
    }

    public void CloseShop()
    {
        shopScreen.SetActive(false);
    }


    public void CreateShopUpgrades()
    {
        for (int i = 0; i < StationShop.Instance.availableUpgrades.Count; i++)
        {
            var shopUpgrade = Instantiate(upgradeTemplate, upgradeGroup.transform);
            shopUpgrade.GetComponent<UpgradeButton>().upgrade = StationShop.Instance.availableUpgrades[i];
        }
    }

    public void UpdateUpgradeInfo(Upgrade upgrade)
    {
        upgradeNameText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.upgradeDescription;
        upgradeCostText.text = upgrade.goldCost.ToString() + " tea coins";
        upgradeCloseupImage.sprite = UIController.CreateSpriteFromTexture(upgrade.upgradeTexture);
    }
}
