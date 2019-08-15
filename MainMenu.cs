using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A Main Menu Controller of sorts.
/// Employs the GameManager Scene functions.
/// Those can't be directly used for button events because of https://answers.unity.com/questions/798406/unity-46-button-onclicks-functions-not-showing.html
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject loadingScreen;

    [SerializeField]
    Text loadingProgressText;

    [SerializeField]
    Slider loadingSlider;

    [SerializeField]
    GameLevel tutorial;

    private void Start()
    {
        //audio: play the main theme
        AudioManager.instance.PlayEvent("Play_music_main_menu");
    }
    public void StartGame()
    {
        SaveManager.LoadGlobal();
        loadingScreen.SetActive(true);
        AudioManager.instance.StopAll();
        GameManager.instance.activeLevel = tutorial;
        StartCoroutine(LoadScene(1));
    }

    public void LoadTutorial()
    {
        SaveManager.LoadGlobal();
        loadingScreen.SetActive(true);
        AudioManager.instance.StopAll();
        StartCoroutine(LoadScene(2));
    }

    public void QuitGame()
    {
        GameManager.QuitGame();
    }

    public void GoToCredits()
    {
        GameManager.LoadCreditsScene();
    }

    IEnumerator LoadScene(int sceneID)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneID);

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
}