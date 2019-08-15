using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler// required interface when using the OnPointerEnter method.
{
    private Button button;

    [SerializeField]
    public Upgrade upgrade;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.image.sprite = UIController.CreateSpriteFromTexture(upgrade.upgradeTexture);
        button.onClick.AddListener(TaskOnClick);
        UpdateVisualsIfPurchased();
    }

    //Called when the player clicks or selects the button
    void BuyUpgrade()
    {
        Debug.Log("Attempting to buy upgrade");
        if (GameManager.instance.globalSaveData.money > upgrade.goldCost)
        {
            StationShop.Instance.BuyUpgrade(upgrade);
            UpdateVisualsIfPurchased();
        }
    }


    private void TaskOnClick()
    {
        BuyUpgrade();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
        {
            TaskOnClick();
        }
    }

    //visually indicate whether the item has already been purchased
    private void UpdateVisualsIfPurchased()
    {
        if (StationShop.AlreadyPurchased(upgrade))
        {
            button.image.color = Color.white;
        }
        else
        {
            button.image.color = Color.gray;
        }
    }

    private void UpdateUI()
    {
        UIStationShop.Instance.UpdateUpgradeInfo(upgrade);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("An upgrade has been selected");
        UpdateUI();
    }
}