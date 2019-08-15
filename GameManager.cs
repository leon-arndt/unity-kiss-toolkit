using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Multi-scene game manager. More abstract than the game controller. Carries only data, no logic.
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager instance = null;  //Static instance of GameManager which allows it to be accessed by any other script.

    public bool inGame;
    public bool shouldPlayCustomerReactionInHub;

    //The level to load
    public GameLevel activeLevel;

    //save data
    public GlobalSaveData globalSaveData;

    //persistent data
    public PersistentData persistentData;

    public enum Language
    {
        English = 0,
        German = 1
    }

    public Language language;

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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
#if UNITY_EDITOR
        //save
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveManager.SaveGlobal();
        }
        
        //open
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.LoadGlobal();
        }
#endif
    }

    public static void RestartGameScene()
    {
        AudioManager.instance.StopAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadGameScene()
    {
        SceneManager.LoadScene(2);
    }

    public static void LoadHubScene()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadCreditsScene()
    {
        GameManager.instance.shouldPlayCustomerReactionInHub = false;
        SceneManager.LoadScene(3);
    }

    public static void QuitGame()
    {
        Debug.Log("Attempted to quit the game");
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
