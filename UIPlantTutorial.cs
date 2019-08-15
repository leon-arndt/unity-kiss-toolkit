using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

/// <summary>
/// This component shows UI tutorials for new plants as they are created from the slot machine.
/// These are popups and present the information more clearly than the small thumbnails
/// </summary>
public class UIPlantTutorial : MonoBehaviour {
    public static UIPlantTutorial Instance;
    public bool tutorialVisible;

    [SerializeField]
    GameObject uiGroup;

    [SerializeField]
    Text plantNameText, plantDescriptionText, flavorText, cookTimeText, traitText;

    [SerializeField]
    Image plantImage;

    [SerializeField]
    AnimationClip showCardClip;

    Player player;

    private void Awake()
    {
        Instance = this;
        player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (tutorialVisible)
        {
            if (player.GetButtonDown("Action")) {
                HidePlantTutorial();
            }
        }
    }

    //Update the UI elements with data
    public void UpdatePlantTutorial(IngredientData ingredientData)
    {
        Translator translator;
        translator = UIController.Instance.translator;
        plantNameText.text = ingredientData.plantName;
        plantDescriptionText.text = translator.GetTranslation(ingredientData.description);
        flavorText.text = translator.GetTranslation(ingredientData.flavor);
        traitText.text = translator.GetTranslation(ingredientData.trait);
        cookTimeText.text = ingredientData.cookTime + " seconds";

        plantImage.sprite = UIController.CreateSpriteFromTexture(ingredientData.ingredientTexture);
    }

    public void ShowPlantTutorial(IngredientData ingredientData)
    {
        UpdatePlantTutorial(ingredientData);
        uiGroup.SetActive(true);
        tutorialVisible = true;

        //update global save data
        GlobalSaveData data = GameManager.instance.globalSaveData;
        if (!data.encounteredIngredients.Contains(ingredientData.plantName)) {
            data.encounteredIngredients.Add(ingredientData.plantName);
        }

        StartCoroutine(PauseTimeWithDelay(showCardClip.length));
        GrayscaleEffect.Instance.FadeToAndFromGray(true);
    }

    public void HidePlantTutorial()
    {
        uiGroup.SetActive(false);
        tutorialVisible = false;
        Time.timeScale = 1f;
        GrayscaleEffect.Instance.FadeToAndFromGray(false);
    }

    public static bool ShouldShowPlantCard(IngredientData ingredientData)
    {
       GlobalSaveData data = GameManager.instance.globalSaveData;
       if (data.encounteredIngredients.Contains(ingredientData.plantName))
       {
           return false;
       }
       else
           return true;
    }

    IEnumerator PauseTimeWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }
}
