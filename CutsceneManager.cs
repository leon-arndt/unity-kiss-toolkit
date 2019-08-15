using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple Cutscene Manager. It manages the start and end of levels.
/// Currently uses Invoke for timed methods. Would be cleaner to abstract to actions which would be easier to manage.
/// </summary>
public class CutsceneManager : MonoBehaviour {
    //setting up singleton
    public static CutsceneManager instance = null;
    public bool inCutscene;

    bool canContinue;

    [SerializeField]
    Camera gameCamera, outsideCamera;

    [SerializeField]
    GameObject trainOutside, trainInside, gameUI, dialogueUI, plantTips, anyKeyScreen, levelTextGO;

    [SerializeField]
    Animation levelNameAnimation;

    [SerializeField]
    Animator trainOutsideAnimator;

    [SerializeField]
    AnimationClip downClip, upClip, trainOutsideClip;

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

    // Use this for initialization
    void Start () {
        if (!GameController.instance.debugMode)
        {
            StartIntroCutscene();
        }
        else
        {
            levelTextGO.SetActive(false);
        }
    }

    private void Update()
    {
        if (canContinue)
        {
            if (Input.anyKeyDown)
            {
                ContinueToGame();
            }
        }
    }

    void StartIntroCutscene()
    {
        inCutscene = true;
        gameCamera.enabled = false;
        outsideCamera.enabled = true;
        trainOutside.SetActive(true);
        trainInside.SetActive(false);
        gameUI.SetActive(false);
        plantTips.SetActive(true);
        anyKeyScreen.SetActive(true);
        levelNameAnimation.Play(downClip.name);

        GameController.instance.player1.enabled = false;
        GameController.instance.player2.enabled = false;


        canContinue = true;

       // Invoke("StopTime", 1f);
    }

    public void ContinueToGame()
    {
        Time.timeScale = 1f;
        inCutscene = false;

        canContinue = false;
        HideView();
        Invoke("ShowView", 1);
        Invoke("ExitIntroCutscene", 1);

        levelNameAnimation.Play(upClip.name);
        trainOutsideAnimator.Play(trainOutsideClip.name);
    }

    public void ExitIntroCutscene()
    {
        gameCamera.enabled = true;
        outsideCamera.enabled = false;
        trainOutside.SetActive(false);
        trainInside.SetActive(true);
        gameUI.SetActive(true);
        plantTips.SetActive(false);
        anyKeyScreen.SetActive(false);
        VehicleController.Instance.StartMoving();

        GameController.instance.player1.enabled = true;
        GameController.instance.player2.enabled = true;

        if (GameManager.instance.activeLevel.showTutorial)
        {
            DialogueManager.Instance.LoadDialogueScene(TutorialController.instance.introTutorial);
            DialogueManager.Instance.EnterDialogue();
        }
    }

    public void HideLevelText()
    {
        levelNameAnimation.Play(upClip.name);
    }

    //called after a level is won
    public void StartGameOutroCutscene()
    {
        AudioManager.instance.PlayEvent("Play_ui_jingle_win");
        VehicleController.Instance.StartMovingOffscreen();
        UpdateSaveData();
        UIController.Instance.SetLevelCompleteVisibility(true);
        Invoke("HideView", 2f);
        Invoke("LoadHub", 3f);
    }

    void UpdateSaveData()
    {
        GameManager.instance.globalSaveData.timeLeftOnLevel = GameController.GetSecondsLeft();
        GameManager.instance.globalSaveData.damageSustainedOnLevel = VehicleController.Instance.totalDamageSustained;

        int numOfNewTeacups = TeacupManager.UpdateLevelSaveData(GameManager.instance.globalSaveData.currentLevelSaveData);
        GameManager.instance.globalSaveData.teacupCount += numOfNewTeacups;

        //earn money
        GameManager.instance.globalSaveData.money += 100 + (int)GameController.GetSecondsLeft() - (int)VehicleController.Instance.totalDamageSustained;

        //update total damage
        GameManager.instance.globalSaveData.stats.totalDamageTaken += VehicleController.Instance.totalDamageSustained;

        //save level then global
        SaveManager.SaveLevelData(GameManager.instance.activeLevel);
        SaveManager.SaveGlobal();
    }

   
    void HideView()
    {
        UIController.Instance.PlayCircleFadeAnimation(true);
    }

    void ShowView()
    {
        UIController.Instance.PlayCircleFadeAnimation(false);
    }

    void LoadHub()
    {
        GameManager.LoadHubScene();
    }
}

[System.Serializable]
public class CutsceneAction : System.Object
{
    public float time;
    public string methodName;
}