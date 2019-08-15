using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains the UI fields which are adressed by the UIController through this class
/// </summary>
public class PlantCard : MonoBehaviour {
    [SerializeField]
    private Text plantName, plantTrait;

    [SerializeField]
    private Image plantImage;

    public void UpdatePlantCard(IngredientData ingredientData)
    {
        Translator translator = GameManager.instance.GetComponent<Translator>();
        plantName.text = ingredientData.plantName;
        plantTrait.text = translator.GetTranslation(ingredientData.trait);
        plantImage.sprite = UIController.CreateSpriteFromTexture(ingredientData.ingredientTexture);
    }
}
