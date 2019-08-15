using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StationUI : MonoBehaviour {
    //setting up singleton
    public static StationUI Instance = null;

    [SerializeField]
    private GameObject levelSelectScreen, resultsScreen, encyclopediaScreen, hatSelectScreen, upgradesScreen, statsScreen, loadingScreen, circleFade, cinematicBarsParent;

    [SerializeField]
    private Button firstLevel, firstPlant, firstUpgrade, firstHat, firstStatsButton, tierUpButton;

    [SerializeField]
    private Text levelTimeText, levelNameText, levelNumberText, levelMissionText, levelCustomerNameText, stationNameText, stationNameWorldText, resultsTimeText, resultsTeacupsText, resultsDamageText, encyclopediaIngredientNameText, encyclopediaIngredientDescText, encycCookText, encycTraitText, encycFlavorText, hatNameText, hatDescText, tierNameText, loadingProgressText, moneyEarnedText;

    [SerializeField]
    private Image[] ingredientImages, resultsTeacups, levelSelectTeacups;

    [SerializeField]
    private Image specialLevelImage, encyclopediaDetailImage, hatImage, levelCustomerImage, encyclopediaSelectHighlight, levelSelectHighlight, upgradeSelectHighlight, hatSelectHighlight;

    [SerializeField]
    private RawImage playerCamImage;

    [SerializeField]
    private RenderTexture player1RT, player2RT;

    public Color lockedColor;

    [SerializeField]
    private Slider loadingSlider;

    [SerializeField]
    private Animation cinematicTopBar, cinematicBottomBar, cinematicFade;

    [SerializeField]
    private AnimationClip cinematicIn, cinematicOut;

    public Sprite hatLock, teacup;

    public UISettings uiSettings;

    private bool loading;
    private Translator translator;

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

    private void Start()
    {
        circleFade.SetActive(true);
        SetAllWindowsInvisible();

        if (!GameManager.instance.shouldPlayCustomerReactionInHub)
        {
            cinematicBarsParent.SetActive(false);
        }

        translator = GameManager.instance.GetComponent<Translator>();
    }

    //Handle player input exclusive to this scene (for station UI objects)
    private void Update()
    {
        if (resultsScreen.activeSelf)
        {
            if (Input.anyKeyDown)
                SetResultsScreenVisibility(false);
        }

        if (Input.GetAxis("Cancel") > 0)
        {
            SetAllWindowsInvisible();
        }
    }

    #region visibilityFunctions
    public void SetAllWindowsInvisible()
    {
        SetLevelSelectScreenVisibility(false);
        SetResultsScreenVisibility(false);
        SetEncyclopediaScreenVisibility(false);
        SetHatSelectScreenVisibility(false);
        SetUpgradesScreenVisibility(false);
        SetStatsScreenVisibility(false);
    }

    public void SetLevelSelectScreenVisibility(bool visible)
    {
        levelSelectScreen.SetActive(visible);

        if (visible)
        {
            firstLevel.Select();
            UpdateLevelInfo(firstLevel.GetComponent<LevelButton>().gameLevel);
        }

        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }

    public void SetResultsScreenVisibility(bool visible)
    {
        resultsScreen.SetActive(visible);

        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }

    public void SetEncyclopediaScreenVisibility(bool visible)
    {
        encyclopediaScreen.SetActive(visible);

        if (visible)
        {
            firstPlant.Select();
            MoveEncycHighlight(firstPlant.transform.position);
        }

        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }

    public void SetHatSelectScreenVisibility(bool visible)
    {
        hatSelectScreen.SetActive(visible);

        if (visible)
        {
            firstHat.Select();
            MoveHatHighlight(firstHat.transform.position);
        }

        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }

    public void SetUpgradesScreenVisibility(bool visible)
    {
        upgradesScreen.SetActive(visible);

        if (visible)
        {
            firstUpgrade.Select();
        }

        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }

    public void SetStatsScreenVisibility(bool visible)
    {
        statsScreen.SetActive(visible);   
        
        if (visible)
        {
            GetComponent<UIStats>().UpdateStatsText();
            firstStatsButton.Select();
        }
        //disable the players when the menu is open
        TrainStation.instance.player1.enabled = !visible;
        TrainStation.instance.player2.enabled = !visible;
    }
    #endregion

    public void DetermineRenderTextureToShow(int playerID)
    {
        playerCamImage.texture = playerID == 0 ? player1RT : player2RT;
    }

    public void UpdateResultsScreen()
    {
        float timeLeft = GameManager.instance.globalSaveData.timeLeftOnLevel;
        float damageSustained = GameManager.instance.globalSaveData.damageSustainedOnLevel;
        int teacupsEarned = GameManager.instance.globalSaveData.currentLevelSaveData.teacupsEarnedOnLevel;
        resultsTimeText.text = "You had " + UIController.ConvertFloatToTime(timeLeft) + " left to spare";
        resultsDamageText.text = "The train sustained " + Mathf.Round(damageSustained).ToString() + " damage.";
        resultsTeacupsText.text = teacupsEarned.ToString() + " / 3 Teacups Earned";
        StartCoroutine(AnimateTeacupUnlock(resultsTeacups, teacupsEarned));

        int moneyEarned = 100 + (int)GameManager.instance.globalSaveData.timeLeftOnLevel - (int)GameManager.instance.globalSaveData.damageSustainedOnLevel;
        moneyEarnedText.text = moneyEarned.ToString() + "gold earned!";
    }

    //Hovering on a level button
    public void UpdateLevelInfo(GameLevel level)
    {
        //update the level screen
        levelNameText.text = translator.GetTranslation(level.levelName);
        levelNumberText.text = "Level " + level.levelNumber + ":";
        levelTimeText.text = UIController.ConvertFloatToTime(level.levelTime);
        levelMissionText.text = translator.GetTranslation(level.missionText);
        levelCustomerNameText.text = translator.GetTranslation(level.customer.speakerName);
        levelCustomerImage.sprite = level.customer.speakerSprite;
        
        //is this a level with a special rule
        if (level.specialRule != GameController.SpecialRule.None)
        {
            specialLevelImage.gameObject.SetActive(true);
        }
        else
        {
            specialLevelImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < ingredientImages.Length; i++)
        {
            Texture2D texture = level.desiredIngredients[i].ingredientTexture;

            ingredientImages[i].sprite = Sprite.Create(texture,
                                     new Rect(0, 0, texture.width, texture.height),
                                     new Vector2(0.5f, 0.5f));
        }

        UpdateLevelMenuTeacups(level);
    }

    public void UpdateEncyclopediaInfo(IngredientData ingredientData)
    {
        encyclopediaIngredientNameText.text = ingredientData.plantName;

        string translatedDescription = translator.GetTranslation(ingredientData.description);
        encyclopediaIngredientDescText.text = translatedDescription;

        string translatedFlavor = translator.GetTranslation(ingredientData.flavor);
        encycFlavorText.text = "Flavor: " + translatedFlavor;

        string translatedTrait = translator.GetTranslation(ingredientData.trait);
        encycTraitText.text = translatedTrait;

        encyclopediaDetailImage.sprite = UIController.CreateSpriteFromTexture(ingredientData.ingredientTexture);
        encycCookText.text = "Cook Time: " + ingredientData.cookTime.ToString() + " Seconds";
    }

    public void UpdateHatInfo(Hat hat)
    {
        hatNameText.text = translator.GetTranslation(hat.hatName);
        hatDescText.text = translator.GetTranslation(hat.hatDescription);
    }

    public void ButtonStartGame()
    {
        //stop train station music
        AudioManager.instance.StopEvent("Play_station_music", 0);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2);

        //When the load is still in progress, oupdate the text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            loadingProgressText.text = "Loading progress: " + Mathf.Round(asyncOperation.progress * 100) + "%";
            float sliderGoalValue = Mathf.Lerp(loadingSlider.value, asyncOperation.progress, 0.5f);

            loadingSlider.value = sliderGoalValue;

            yield return null;
        }
    }


    IEnumerator AnimateTeacupUnlock(Image[] images, int num)
    {
        //color all gray by default
        foreach (var item in images)
        {
            item.color = lockedColor;
        }

        for (int i = 0; i < images.Length; i++)
        {
            if (i < num)
            {
                images[i].color = Color.white;
                images[i].GetComponent<Animation>().Play();
            }
            else
            {
                images[i].color = lockedColor;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateStationName()
    {
        if (GameManager.instance.shouldPlayCustomerReactionInHub)
        {
            string stationName = GameManager.instance.activeLevel.destinationStation.stationName;
            stationNameText.text = stationName;
        }
    }

    void UpdateLevelMenuTeacups(GameLevel level)
    {
        int num;
        if (SaveManager.GetLevelData(level) == null)
        {
            num = 0;
        }
        else
        {
            num = SaveManager.GetLevelData(level).teacupsEarnedOnLevel;
        }

        ColorTeacups(levelSelectTeacups, num);
    }

    private void ColorTeacups(Image[] images, int num)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < num)
            {
                images[i].color = Color.white;
            }
            else
            {
                images[i].color = lockedColor;
            }
        }
    }


    public void DisplayNextTier()
    {
        LevelSelector.Instance.LoadNextTier();
        tierNameText.text = LevelSelector.Instance.GetTierName();
    }

    public void DisplayPreviousTier()
    {
        LevelSelector.Instance.LoadPreviousTier();
        tierNameText.text = LevelSelector.Instance.GetTierName();
    }

    public void MoveEncycHighlight(Vector3 newPosition)
    {
        encyclopediaSelectHighlight.rectTransform.position = newPosition;
    }

    public void MoveHatHighlight(Vector3 newPosition)
    {
        hatSelectHighlight.rectTransform.position = newPosition;
    }

    // Update the loading bar while the game is loading
    public void SetLoadingSliderProgress(float f)
    {
        loadingSlider.value = f;
    }

    public void SetCinematicBarVisibility(bool visible)
    {
        if (visible)
        {
            cinematicBarsParent.SetActive(true);
            cinematicTopBar.Play(cinematicIn.name);
            cinematicBottomBar.Play(cinematicIn.name);
        }
        else
        {
            cinematicTopBar.Play(cinematicOut.name);
            cinematicBottomBar.Play(cinematicOut.name);
        }
    }

    public void PlayCinematicFadeToWhite()
    {
        cinematicFade.Play();
        Invoke("GoToCredits", cinematicFade.clip.length);
    }

    public void GoToCredits()
    {
        GameManager.LoadCreditsScene();
    }

    public void UpdateTierUpButton(bool interactable, int teacupsRequired = 0)
    {
        tierUpButton.interactable = interactable;
        if (!interactable)
        {
            //create a lock
            CreateLock(tierUpButton.gameObject, teacupsRequired);
            Debug.Log("created lock");
        }
        else
        {
            //delete lock in child of tier up button transform
            if (tierUpButton.transform.Find("Lock").gameObject != null)
            {
                Destroy(tierUpButton.transform.Find("Lock").gameObject);
                Debug.Log("destroyed lock");
            }
        }
    }
    public void CreateLock(GameObject parent, int teacupRequirment)
    {
        GameObject lockObj = new GameObject("Lock");
        lockObj.transform.SetParent(parent.transform);

        Image lockImg = lockObj.AddComponent<Image>();
        lockImg.sprite = StationUI.Instance.hatLock;
        lockImg.color = StationUI.Instance.lockedColor;

        GameObject lockTextObj = new GameObject("LockText");
        lockTextObj.transform.SetParent(lockObj.transform);
        Text lockText = lockTextObj.AddComponent<Text>();
        lockText.text = teacupRequirment.ToString();
        lockText.resizeTextForBestFit = true;
        lockText.font = DialogueManager.Instance.comfortaa;
        lockText.alignment = TextAnchor.MiddleCenter;

        GameObject teacupObj = new GameObject("LockTeacup");
        teacupObj.transform.SetParent(lockObj.transform);
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
}
