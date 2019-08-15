using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

/// <summary>
/// This component handles the audio for all buttons in the game.
/// </summary>
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler// required interface when using the OnPointerEnter method.
{
    private Button button;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    //Select Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayEvent("Play_ui_click");
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.instance.PlayEvent("Play_ui_click"); ;
    }

    //Click Methods
    public void OnPointerClick(PointerEventData eventData)
    {
        TaskOnClick();
    }

    private void TaskOnClick()
    {
        AudioManager.instance.PlayEvent("Play_ui_select_rew");
    }
}
