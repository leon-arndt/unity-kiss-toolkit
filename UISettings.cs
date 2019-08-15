using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This script handles the UI objects which are part of the game settings.
/// This script is present on objects in every scene.
/// </summary>
public class UISettings : MonoBehaviour {
    [SerializeField]
    GameObject pauseScreen, settingsScreen;

    [SerializeField]
    Button firstPauseButton;

    public static UISettings Instance = null;

    [SerializeField]
    Toggle dyslexicToggle, screenShakeToggle;

    [SerializeField]
    Slider difficultySlider;

    //only called when enabled
    private void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
        }

        SetPauseScreenVisibility(false);

        //Add listener for when the state of the Toggles changes, to take action in SettingsManager
        dyslexicToggle.onValueChanged.AddListener(delegate {
            SettingsManager.Instance.DyslexicMode = dyslexicToggle.isOn;
        });

        screenShakeToggle.onValueChanged.AddListener(delegate {
            SettingsManager.Instance.ScreenShake = screenShakeToggle.isOn;
        });

        difficultySlider.onValueChanged.AddListener(delegate {
            SettingsManager.Instance.DifficultyFactor = difficultySlider.value;
        });
    }

    public void SetPauseScreenVisibility(bool visible)
    {
        pauseScreen.SetActive(visible);
        settingsScreen.SetActive(false);

        if (visible)
        {

            firstPauseButton.Select();
        }
        else
        {
            //button needs to be deselected if the menu is closed otherwise transitions will break
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    //Settings business
    public void EnterSettings()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        UpdateUIWithSettings();
    }

    public void LeaveSettings()
    {
        pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);

        SettingsManager.Instance.SaveSettings();
    }

    public void UpdateUIWithSettings()
    {
        SettingsManager settingsManager = SettingsManager.Instance;

        dyslexicToggle.isOn = settingsManager.DyslexicMode;
        screenShakeToggle.isOn = settingsManager.ScreenShake;
        difficultySlider.value = settingsManager.DifficultyFactor;
    }
}
