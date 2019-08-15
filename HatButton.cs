using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

/// <summary>
/// Level Button classes used for the Level Selection Screen in the train station. Jan Gihr helped to co-programm this :)
/// </summary>
public class HatButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler// required interface when using the OnPointerEnter method.
{
    private Button button;
    private Text teacupRequirement;

    [SerializeField]
    private Hat hat;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.image.sprite = UIController.CreateSpriteFromTexture(hat.hatTexture);

        if (GameManager.instance.globalSaveData.teacupCount < hat.teacupsNeeded)
        {
            button.interactable = false;
            button.onClick.AddListener(TaskOnClick);
            InitLockedUI();
        }
    }

    private void InitLockedUI()
    {
        GameObject lockObj = new GameObject("Lock");
        lockObj.transform.SetParent(gameObject.transform);

        Image lockImg = lockObj.AddComponent<Image>();
        lockImg.sprite = StationUI.Instance.hatLock;
        lockImg.color = StationUI.Instance.lockedColor;

        GameObject lockTextObj = new GameObject("LockText");
        lockTextObj.transform.SetParent(lockObj.transform);
        Text lockText = lockTextObj.AddComponent<Text>();
        lockText.text = hat.teacupsNeeded.ToString();
        lockText.resizeTextForBestFit = true;
        lockText.font = DialogueManager.Instance.comfortaa;
        lockText.alignment = TextAnchor.MiddleCenter;

        GameObject teacupObj = new GameObject("LockTeacup");
        teacupObj.transform.SetParent(gameObject.transform);
        Image teacupImg = teacupObj.AddComponent<Image>();
        teacupImg.sprite = StationUI.Instance.teacup;

        RectTransform lockTextRT = lockTextObj.GetComponent<RectTransform>();
        lockTextRT.localPosition = new Vector3(32, -80, 0);
        lockTextRT.sizeDelta = new Vector2(40, 40);

        RectTransform teacupRT = teacupObj.GetComponent<RectTransform>();
        teacupRT.localPosition = new Vector3(-32, -76, 0);
        teacupRT.sizeDelta = new Vector2(24, 24);

        RectTransform lockRT = lockObj.GetComponent<RectTransform>();
        lockRT.localPosition = Vector3.zero;
        lockRT.sizeDelta = new Vector2(96, 96);
        lockRT.localScale = Vector3.one;
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

    public void OnSelect(BaseEventData eventData)
    {
        UpdateUI();
        StationUI.Instance.MoveHatHighlight(GetComponent<RectTransform>().position);
    }

    private void TaskOnClick()
    {
        HatManager.UpdateCurrentHat(hat);
        HatManager.UpdateGlobalDataWithNewHat(hat);
    }

    private void UpdateUI()
    {
        StationUI.Instance.UpdateHatInfo(hat);
    }
}