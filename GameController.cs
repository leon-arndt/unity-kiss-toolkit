using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game controller for the Game Unity scene.
/// Used by other classes to get the PlayerControll of player1 and player2.
/// </summary>
public class GameController : MonoBehaviour {
    //setting up singleton
    public static GameController instance = null;

    public bool debugMode = false;

    //public bool inDialogue = false;

    private static float secondsLeft;

    public PlayerController player1, player2;
    public VehicleController vehicleController;

    [SerializeField]
    GameObject staticCam, followCam, coalMachine;

    public IngredientData[] desiredIngredients = new IngredientData[3];
    public bool[] ingredientFound = new bool[3];

    public bool foundAllDesiredIngredients = false;

    public float timeProtectedFromEvents = 8f;
    private float timeUntilForcedBreak = 8f;
    private const float timeBetweenForcedBreaks = 18f;
    private bool isGameOver;

    public enum SpecialRule { None, NoFurnace, FiveSlotItems, TimeMovesWhenYouMove };
    public SpecialRule specialRule;


    private void Awake()
    {
        //assert that train station music stops playing
        AudioManager.instance.StopAll();


        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start () {
        secondsLeft = GameManager.instance.activeLevel.levelTime;
        desiredIngredients = GameManager.instance.activeLevel.desiredIngredients;
        UIController.Instance.UpdateMissionWithDesiredIngredients();
        GameManager.instance.shouldPlayCustomerReactionInHub = false;

        HatManager.OutfitBothplayers();

        //reset level save data
        GameManager.instance.globalSaveData.currentLevelSaveData = new LevelSaveData();
        if (SaveManager.GetLevelData(GameManager.instance.activeLevel) != null) {
            SaveManager.UpdateCurrentLevelDataWithPrevious();
        }

        if (debugMode)
        {
            CutsceneManager.instance.ExitIntroCutscene();
            CutsceneManager.instance.HideLevelText();
        }

        if (GameManager.instance.activeLevel.coalMachineEnabled)
        {
            coalMachine.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
        if (CutsceneManager.instance.inCutscene) return; //don't do anything while in dialogue

        //debug win
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Q))
        {
            WinGame();
        }
#endif


        //debug protects from all events
        if (debugMode)
        {
            timeProtectedFromEvents = 10f;
            secondsLeft = 999f;
        }

        //time running out
        if (secondsLeft > 1)
        {
            secondsLeft -= Time.deltaTime;
        }
        else
        {
            GameOver();
        }
        #region _EVENTS
        timeProtectedFromEvents -= Time.deltaTime;
        timeUntilForcedBreak -= Time.deltaTime;

        //forced breaking
        if (GameManager.instance.activeLevel.forcedBreaking)
        {
            if (timeUntilForcedBreak + timeProtectedFromEvents < 0)
            {
                SurpriseManager.Instance.BreakRandom();
                timeProtectedFromEvents = 5f;
                timeUntilForcedBreak = timeBetweenForcedBreaks;
            }
        }





        //random breaking
        if (GameManager.instance.activeLevel.randomBreaking)
        {
            if (timeProtectedFromEvents < 0)
            {
                if (Random.Range(0, 1000) > 998.5)
                {
                    SurpriseManager.Instance.BreakHighPipe();
                    timeProtectedFromEvents = 5f;
                }
                else
                if (Random.Range(0, 1000) > 998)
                {
                    SurpriseManager.Instance.BreakPipe();
                    timeProtectedFromEvents = 2f;
                }
                else
                if (Random.Range(0, 1000) > 998)
                {
                    SurpriseManager.Instance.BreakGear();
                    timeProtectedFromEvents = 2f;
                }
            }
        }
        #endregion
    }

    public static float GetSecondsLeft()
    {
        return secondsLeft;
    }

    public float GetSecondsLeftRatio()
    {
        return 1 - (secondsLeft / GameManager.instance.activeLevel.levelTime);
    }

    public void WinGame()
    {
        //game can only be won if it hasn't already been lost
        if (!isGameOver)
        {
            CutsceneManager.instance.StartGameOutroCutscene();
            if (GameManager.instance.activeLevel.customerReaction != null)
            {
                GameManager.instance.shouldPlayCustomerReactionInHub = true;
            }

            EventManager.OnDialogueEnd -= WinGame;
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            UIController.Instance.ShowGameOverScreen();

            //audio
            AudioManager.instance.PlayEvent("Play_ui_jingle_lose");

            //stop the train
            VehicleController.Instance.StopCompletly();

            //stop the players
            player1.GetComponent<Rigidbody>().isKinematic = true;
            player2.GetComponent<Rigidbody>().isKinematic = true;
            player1.enabled = false;
            player2.enabled = false;

            isGameOver = true;
        }
    }

    public PlayerController GetPlayerByID(int playerID)
    {
        if (playerID == 0)
        {
            return player1;
        }
        return player2;
    }

    public void Unpause()
    {
        UISettings.Instance.SetPauseScreenVisibility(Time.timeScale == 1);
        GrayscaleEffect.Instance.FadeToAndFromGray(Time.timeScale == 1);
        Time.timeScale = 1f;
    }
}
