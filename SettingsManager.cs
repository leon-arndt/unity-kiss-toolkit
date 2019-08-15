using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This manager handles various in-game settings.
/// It uses player prefs to save the settings.
/// </summary>
public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance;

    private const string DIFFICULTY_FACTOR = "DIFFICULTY_FACTOR";
    private const string DYSLEXIC_MODE = "DYSLEXIC_MODE";
    private const string SCREEN_SHAKE = "SCREEN_SHAKE";

    private bool dyslexicMode;
    public bool DyslexicMode
    {
        get { return dyslexicMode; }
        set
        {
            if (value == dyslexicMode)
                return;

            dyslexicMode = value;

            //only relevant in some scenes
            if ((DialogueManager.Instance != null))
            {
                if (dyslexicMode)
                {
                    DialogueManager.Instance.RespectDyslexia();
                }
                else
                {
                    DialogueManager.Instance.UseStandardFont();
                }
            }
        }
    }

    private bool screenShake;
    public bool ScreenShake
    {
        get { return screenShake; }
        set
        {
            if (value == screenShake)
                return;

            screenShake = value;
            if (CameraShake.Instance != null)
            {
                if (screenShake)
                {
                    CameraShake.Instance.enabled = true;
                }
                else
                {
                    CameraShake.Instance.enabled = false;
                }
            }
        }
    }

    private float difficultyFactor;
    public float DifficultyFactor
    {
        get { return difficultyFactor; }
        set
        {
            if (value == difficultyFactor)
                return;

            difficultyFactor = value;
            if (VehicleController.Instance != null)
            {
                VehicleController.Instance.UpdateDamageScaling(0.5f + 0.5f * value);
            }
        }
    }

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadSettings();
    }


    public void LoadSettings()
    {
        DyslexicMode = PlayerPrefs.GetInt(DYSLEXIC_MODE, 0) == 1;
        ScreenShake = PlayerPrefs.GetInt(SCREEN_SHAKE, 1) == 1;
        DifficultyFactor = PlayerPrefs.GetFloat(DIFFICULTY_FACTOR, 0f);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(DYSLEXIC_MODE, Convert.ToInt32(DyslexicMode));
        PlayerPrefs.SetInt(SCREEN_SHAKE, Convert.ToInt32(ScreenShake));
        PlayerPrefs.SetFloat(DIFFICULTY_FACTOR, DifficultyFactor);
    }
}
