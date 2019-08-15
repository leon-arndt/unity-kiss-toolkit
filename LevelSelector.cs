using UnityEngine;

/// <summary>
/// Loads the proper set of levels for each tier.
/// </summary>
public class LevelSelector : MonoBehaviour {
    //singleton
    public static LevelSelector Instance = null;

    public LevelButton[] levelButtons;
    public TierPackage[] tierPackages;
    private int currentTier = 0;
    private int maxAvailableTier = 0;
    public const int TIER_TEACUP_REQUIREMENT = 12;

    //setting up singleton
    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            //Otherwise destroy this.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DetermineMaxAvailableTier();
        LoadTierLevels(0);
    }

    public void Select(GameLevel levelToLoad)
    {
        GameManager.instance.activeLevel = levelToLoad;
    }

    public void LoadTierLevels(int i)
    {
        LoadLevels(tierPackages[i].gameLevels);
    }

    private void LoadLevels(GameLevel[] levels)
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].gameLevel = levels[i];
        }
        HandleTierUpButton();
    }

    public void LoadNextTier()
    {
        //never go above maxAvailableTier
        currentTier = Mathf.Min(currentTier + 1, maxAvailableTier);
        LoadTierLevels(currentTier);
    }

    public void LoadPreviousTier()
    {
        //never go below 0
        int desiredTier = Mathf.Max(currentTier - 1, 0);
        if (currentTier != desiredTier)
        {
            currentTier = Mathf.Max(currentTier - 1, 0);
            LoadTierLevels(currentTier);
        }
    }

    public string GetTierName()
    {
        return tierPackages[currentTier].tierName;
    }

    public void DetermineMaxAvailableTier()
    {
        int teacups = GameManager.instance.globalSaveData.teacupCount;
        Debug.Log("maxAvailableTier: " + maxAvailableTier);
        maxAvailableTier = 2;
        //maxAvailableTier = (int) (teacups / TIER_TEACUP_REQUIREMENT);
    }

    private void HandleTierUpButton()
    {
        //show lock for the next tier
        if (currentTier == maxAvailableTier)
        {
            //StationUI.Instance.UpdateTierUpButton(false, (maxAvailableTier + 1) * TIER_TEACUP_REQUIREMENT); //TODO: FIX SCENE NULL EXCEPTION
        }
        else
        {
            //StationUI.Instance.UpdateTierUpButton(true); //TODO: FIX SCENE NULL EXCEPTION
        }
    }
}
