using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

/// <summary>
/// Level Button classes used for the Level Selection Screen in the train station. Jan Gihr helped to co-programm this :)
/// </summary>
public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler// required interface when using the OnPointerEnter method.
{
    private Button button;

    public GameLevel gameLevel;

    [SerializeField]
    Animation animator;

    [SerializeField]
    AnimationClip highlightClip, unhighlightClip;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("The cursor entered the selectable UI element.");
        StationUI.Instance.UpdateLevelInfo(gameLevel);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("The cursor clicked the selectable UI element.");
        TaskOnClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " was selected");
        StationUI.Instance.UpdateLevelInfo(gameLevel);
        animator.Play(highlightClip.name);
    }

    public void OnDeselect(BaseEventData data)
    {
        animator.Play(unhighlightClip.name);
    }

    private void TaskOnClick()
    {
        //Debug.Log("button clicked the selectable UI element.");
        GameManager.instance.activeLevel = gameLevel;
        StationUI.Instance.ButtonStartGame();
    }
}