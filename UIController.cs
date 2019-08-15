using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    //setting up singleton
    public static UIController Instance = null;

    [SerializeField]
    Text timeLeftText, vehicleSpeedText, temperatureText, distanceFromGoalText, levelNameText;

    [SerializeField]
    Canvas canvas;

    public
    Image temperatureImage, temperatureEffectImage, slotMachineHintImage, ovenHintImage, P1controlImage, P2controlImage, damageIndicatorImage, damageIndicatorBarImage, weatherImage, cookingBarImage, cookingBarBackground, temperatureDial, damageFill;

    [SerializeField]
    private Sprite sunnyWeather, rockyWeather, rainyWeather, icyWeather, heatWeather, temperatureDecrease, temperatureIncrease;

    [SerializeField]
    private Slider distanceSlider, damageSlider, temperatureSlider;

    [SerializeField]
    private Image[] desiredIngredientImages, missionCheckmarkImages;

    [SerializeField]
    GameObject hurryUpGameObject, fixTrainGameObject, levelCompletePopup, putIngredientsGameObject, destinationPopup, rockDangerPopup, gameOverScreen, successScreen, cookingScreen;

    [SerializeField]
    RectTransform damageBackgroundRT;

    [SerializeField]
    RawImage p1CameraImage, p2CameraImage;

    [SerializeField]
    RenderTexture p1RenderTexture, p2RenderTexture;

    [SerializeField]
    Animation weatherAnimation, p1PortraitAnimation, p2PortraitAnimation, circleFadeAnimation, cookingBarAnimation;

    [SerializeField]
    AnimationClip impactClip, shrinkClip, growClip, cookingBarDownClip, cookingBarUpClip;

    [SerializeField]
    PlantCard[] plantCards;

    [SerializeField]
    Button restartGameButton;

    Vector3 damageBackgroundStartPosition;

    [SerializeField]
    Gradient damageColorGradient;

    public UISettings uiSettings;
    public Translator translator; //reference can be used by other classes as well


    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        canvas.gameObject.SetActive(true);

        damageBackgroundStartPosition = damageBackgroundRT.localPosition;

        SetCookingScreenVisibility(false);
        cookingBarImage.type = Image.Type.Filled;
        cookingBarImage.fillMethod = Image.FillMethod.Horizontal;

        damageIndicatorBarImage.type = Image.Type.Filled;
        damageIndicatorBarImage.fillMethod = Image.FillMethod.Horizontal;

        UpdateLevelText();
        UpdateAllPlantCards();
        DisableAllCheckmarkImages();
        translator = GameManager.instance.GetComponent<Translator>();
    }
	
	// Update is called once per frame
	void Update () {
        float secondsLeft = GameController.GetSecondsLeft();
        timeLeftText.text = ConvertFloatToTime(secondsLeft);

        if (Mathf.Round(secondsLeft) == 240)
        {
            putIngredientsGameObject.SetActive(true);
        }



        //show a special popup if time is running out
        if (secondsLeft < 15 && !hurryUpGameObject.activeSelf)
        {
            hurryUpGameObject.SetActive(true);
        }

        //train fixing popup
        if (VehicleController.Instance.GetDamage() > 60f && !fixTrainGameObject.activeSelf)
        {
            fixTrainGameObject.SetActive(true);
        }

        vehicleSpeedText.text = ((int)3.6f * Mathf.Round(VehicleController.Instance.GetSpeed())).ToString() + " km/h";

        //temperature text and icon
        temperatureText.text = ((int)VehicleController.Instance.GetTemperature()).ToString() + " °C";
        temperatureImage.rectTransform.localScale = 0.5f * Vector3.one + 0.5f * VehicleController.Instance.GetTemperatureRatio() * Vector3.one;

        //update the temperature dial
        float desiredDialZRot = 100 - VehicleController.Instance.GetTemperature();
        float currentDialZRot = temperatureDial.rectTransform.localRotation.z;
        temperatureDial.rectTransform.localRotation = Quaternion.Euler(0, 0, desiredDialZRot); 

        string distanceFromGoalString = ConvertVehicleDistanceToKilometers(VehicleController.Instance.GetDistanceFromGoal()).ToString();

        //add missing punctuation to the string
        if (distanceFromGoalString.Length == 1)
        {
            distanceFromGoalString = distanceFromGoalString + ".";
        }

        //add missing zeroes after the float, could be problematic if using distances higher than ten
        while (distanceFromGoalString.Length < 4)
        {
            distanceFromGoalString = distanceFromGoalString + "0";
        }

        distanceFromGoalText.text = distanceFromGoalString + " km";


        //sliders
        temperatureSlider.value = VehicleController.Instance.GetTemperatureRatio();
        distanceSlider.value = GameController.instance.GetSecondsLeftRatio();


        //damage
        damageSlider.value = 1 - VehicleController.Instance.GetDamage() /100f;
        damageIndicatorBarImage.fillAmount = VehicleController.Instance.GetDamage() / 100f;
        damageIndicatorImage.color = new Color(damageIndicatorImage.color.r, damageIndicatorImage.color.g, damageIndicatorImage.color.b, 0.7f * VehicleController.Instance.GetDamage() / 100f);
        damageFill.color = damageColorGradient.Evaluate(1 - VehicleController.Instance.GetDamage() / 100f);

        //shake damage above 70 damage
        float shakeThreshold = VehicleController.DANGER_THRESHOLD;
        float shakeAmount = VehicleController.Instance.GetDamage() > shakeThreshold ? 0.1f * (VehicleController.Instance.GetDamage() - shakeThreshold) : 0;
        damageBackgroundRT.localPosition = damageBackgroundStartPosition + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));

        //update the player cameras
        UpdatePlayerCameras();
    }

    public void MatchImageColorToTemperature()
    {
        temperatureImage.color = new Color(4 * VehicleController.Instance.GetTemperature() / 255f, 0, 0);
    }


    public void MoveControlHintToPlayer(int playerID, Vector3 position)
    {
        if (playerID == 0)
        {
            P1controlImage.rectTransform.position = position;
        }
        else
        if (playerID == 1)
        {
            P2controlImage.rectTransform.position = position;
        }
    }

    public void UpdateMissionWithDesiredIngredients()
    {
        for(int i = 0; i < desiredIngredientImages.Length; i++)
        {

            Texture2D texture = GameController.instance.desiredIngredients[i].ingredientTexture;

            desiredIngredientImages[i].sprite = Sprite.Create(texture,
                                   new Rect(0, 0, texture.width, texture.height),
                                    new Vector2(0.5f, 0.5f));
        }
    }

    public void UpdateMissionIngredientFound(int i)
    {
        Color col = desiredIngredientImages[i].color;
        desiredIngredientImages[i].color = new Color(col.r, col.g, col.b, 0.2f);
        desiredIngredientImages[i].GetComponent<Animation>().Play();
        missionCheckmarkImages[i].gameObject.SetActive(true);
        missionCheckmarkImages[i].GetComponent<Animation>().Play();
    }

    private void DisableAllCheckmarkImages()
    {
        foreach (Image image in missionCheckmarkImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    //methods for the level data
    public void UpdateLevelText()
    {
        levelNameText.text = GameManager.instance.GetComponent<Translator>().GetTranslation(GameManager.instance.activeLevel.levelName);
    }

    //methods for the cooking bar
    public void UpdateCookingBar(float f)
    {
        cookingBarImage.fillAmount = f;
    }

    public void SetCookingScreenVisibility(bool visible)
    {
        if (visible)
        {
            cookingBarAnimation.Play(cookingBarDownClip.name);
        }
        else
        {
            cookingBarAnimation.Play(cookingBarUpClip.name);
        }
    }

    public void MoveCookingBarToIngredient(int i)
    {
        cookingBarBackground.rectTransform.position = desiredIngredientImages[i].rectTransform.position + 30 * Vector3.down;
    }

    public void PlayCircleFadeAnimation(bool reverse)
    {
        AnimationClip clipToPlay = reverse ? shrinkClip : growClip;
        circleFadeAnimation.Play(clipToPlay.name);
    }

    public float ConvertVehicleDistanceToKilometers(float distanceInMeters)
    {
        float distanceInKilometers;
        distanceInKilometers = distanceInMeters / 1000f;
        distanceInKilometers =  Mathf.Round(distanceInKilometers * 100f) / 100f;

        return distanceInKilometers;
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        StartCoroutine(SelectWithDelay(restartGameButton, 1f));
    }

    public void SetLevelCompleteVisibility(bool visible)
    {
        levelCompletePopup.SetActive(visible);
    }

    public void ShowDestinationPopup()
    {
        destinationPopup.SetActive(true);
    }

    public void ShowRockDangerPopup()
    {
        rockDangerPopup.SetActive(true);
    }

    //Button scripts
    public void ButtonTryAgain()
    {
        Time.timeScale = 1f;
        GameManager.RestartGameScene();
    }

    public void ButtonGoToHub()
    {
        Time.timeScale = 1f;
        AudioManager.instance.StopAll();
        GameManager.LoadHubScene();
    }

    public void UpdateWeatherIcon(WeatherManager.Weather newWeather)
    {
        switch(newWeather)
        {
            case WeatherManager.Weather.Sunny:
                weatherImage.sprite = sunnyWeather;
                temperatureEffectImage.sprite = null;
                break;
            case WeatherManager.Weather.Rainy:
                weatherImage.sprite = rainyWeather;
                temperatureEffectImage.sprite = temperatureDecrease;
                break;
            case WeatherManager.Weather.Rocky:
                weatherImage.sprite = rockyWeather;
                temperatureEffectImage.sprite = null;
                break;
            case WeatherManager.Weather.Icy:
                weatherImage.sprite = icyWeather;
                temperatureEffectImage.sprite = temperatureDecrease;
                break;
            case WeatherManager.Weather.Heat:
                weatherImage.sprite = heatWeather;
                temperatureEffectImage.sprite = temperatureIncrease;
                break;
            default:
                weatherImage.sprite = sunnyWeather;
                temperatureEffectImage.sprite = null;
                break;
        }

        weatherAnimation.Play(impactClip.name);
    }

    public void UpdatePlayerCameras()
    {
        p1CameraImage.texture = p1RenderTexture;
        p2CameraImage.texture = p2RenderTexture;
    }

    public void PlayPortraitAnimation(int playerID)
    {
        if (playerID == 0)
        {
            p1PortraitAnimation.Play();
        }
        else
        {
            p2PortraitAnimation.Play();
        }
    }

    public static string ConvertFloatToTime(float f)
    {
        string subTenAddendum = f % 60 < 10 ? "0" : "";
        return ((int)(f / 60f)).ToString() + ":" + subTenAddendum + ((int)(f % 60)).ToString();
    }

    public static Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        return Sprite.Create(texture,
                                     new Rect(0, 0, texture.width, texture.height),
                                     new Vector2(0.5f, 0.5f));
    }

    public void UpdateAllPlantCards()
    {
        for (int i = 0; i < GameController.instance.desiredIngredients.Length; i++)
        {
            plantCards[i].UpdatePlantCard(GameController.instance.desiredIngredients[i]);
        }
    }

    IEnumerator SelectWithDelay(Button button, float delay)
    {
        yield return new WaitForSeconds(delay);
        button.Select();
        yield return null;
    }
}
