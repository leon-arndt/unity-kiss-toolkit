using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

/// <summary>
/// Level Button classes used for the Level Selection Screen in the train station. Jan Gihr helped to co-programm this :)
/// </summary>
public class EncyclopediaButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler// required interface when using the OnPointerEnter method.
{
    private Button button;

    [SerializeField]
    private IngredientData ingredientData;

    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        button.image.sprite = UIController.CreateSpriteFromTexture(ingredientData.ingredientTexture);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StationUI.Instance.UpdateEncyclopediaInfo(ingredientData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TaskOnClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        StationUI.Instance.UpdateEncyclopediaInfo(ingredientData);
        StationUI.Instance.MoveEncycHighlight(GetComponent<RectTransform>().position);
    }

    private void TaskOnClick()
    {
    }
}