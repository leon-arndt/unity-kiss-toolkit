using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainStation : MonoBehaviour {
    //setting up singleton
    public static TrainStation instance = null;

    public static bool shouldPlayCustomerScene = false;
    public PlayerController player1, player2;

    //public Transform player1Reaction, player2Reaction;

    public Vector3 player1Start, player2Start;

    public GameObject customerHouse;

    private Camera cutsceneCamera;

    private float standardFogDensity;
    private Color standardFogColor;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Train Level Loaded");

        //Give the players their hats
        HatManager.OutfitBothplayers();
    }

    //move to start to work with WWise upgrade execution order
    private void Start()
    {
        if (GameManager.instance.shouldPlayCustomerReactionInHub)
        {
            AudioManager.instance.StopAll();
            PlayCustomerMusic();

            player1Start = player1.transform.position;
            player2Start = player2.transform.position;

            standardFogColor = RenderSettings.fogColor;
            standardFogDensity = RenderSettings.fogDensity;

            StationUI.Instance.UpdateStationName();
            ShowCustomerReaction();

            //reset players when the dialogue ends
            EventManager.OnDialogueEnd += EndCustomerReaction;
        }
        else
        {
            PlayNormalMusic();
        }
    }

    public void ShowCustomerReaction()
    {
        TrainStationData destination = GameManager.instance.activeLevel.destinationStation;

        //lock player movement
        player1.enabled = false;
        player2.enabled = false;


        //build the customer house
        Vector3 housePosition = destination.housePosition;
        float customerHouseYRotation = destination.houseYRotation;

        customerHouse = Instantiate(destination.customerHouse, 
housePosition, Quaternion.Euler(0, customerHouseYRotation, 0));

        //update player transforms
        Transform player1Target = customerHouse.transform.GetChild(1).transform;
        Transform player2Target = customerHouse.transform.GetChild(2).transform;

        player1.transform.position = player1Target.position;
        player2.transform.position = player2Target.position;

        player1.transform.rotation = player1Target.rotation;
        player2.transform.rotation = player2Target.rotation;

        //destroy the targets
        player1Target.gameObject.SetActive(false);
        player2Target.gameObject.SetActive(false);

        //Render Settings
        RenderSettings.fogColor = destination.fogColor;
        RenderSettings.fogDensity = destination.fogDensity;


        //activate the cutscene camera
        if (customerHouse.transform.GetChild(0).GetComponent<Camera>() != null)
        {
            cutsceneCamera = customerHouse.transform.GetChild(0).GetComponent<Camera>();
            cutsceneCamera.enabled = true;
        }
        else
        {
            Debug.Log("cutscene camera could not be found");
        }


        StationUI.Instance.SetCinematicBarVisibility(true);

        Invoke("ShowDialogue", 2f);
    }

    public void ShowDialogue()
    {
        DialogueManager.Instance.LoadDialogueScene(GameManager.instance.activeLevel.customerReaction);
        DialogueManager.Instance.EnterDialogue();
    }

    public void EndCustomerReaction()
    {
        player1.transform.position = player1Start;
        player1.transform.rotation = Quaternion.Euler(0, 180, 0);
        player2.transform.position = player2Start;
        player2.transform.rotation = Quaternion.Euler(0, 180, 0);

        player1.enabled = true;
        player2.enabled = true;

        //fog
        RenderSettings.fogColor = standardFogColor;
        RenderSettings.fogDensity = standardFogDensity;

        //return to the old camera
        cutsceneCamera.enabled = false;

        StationUI.Instance.SetResultsScreenVisibility(true);
        StationUI.Instance.UpdateResultsScreen();
        StationUI.Instance.SetCinematicBarVisibility(false);

        //stop listening to the dialogue
        EventManager.OnDialogueEnd -= EndCustomerReaction;

        Destroy(customerHouse);

        //return to normal station music
        AudioManager.instance.StopAll();
        PlayNormalMusic();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.instance.StopAll();
        SaveManager.SaveGlobal();
        GameManager.LoadMainMenuScene();
    }

    public void Unpause()
    {
        GrayscaleEffect.Instance.FadeToAndFromGray(Time.timeScale == 1);
        //show menu
        if (StationUI.Instance != null)
        {
            StationUI.Instance.uiSettings.SetPauseScreenVisibility(Time.timeScale == 1);
        }

        Time.timeScale = 1 - Time.timeScale;
    }

    private void PlayNormalMusic()
    {
        //audio
        AudioManager.instance.PlayEvent("Play_station_music");
    }

    private void PlayCustomerMusic()
    {
        TrainStationData destination = GameManager.instance.activeLevel.destinationStation;

        string songName =destination.customerSongName;
        AudioManager.instance.PlayEvent(songName);

        //also play the customer atmo
        string atmoName = destination.weatherSoundName;
        if (atmoName != null)
        {
            AudioManager.instance.PlayEvent(atmoName);
        }
    }
}
