using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The DialogueManager class.
/// Combines both Logic and UI. Uses data from the DialogueScene scriptable object.
/// Separated from the UIController and GameController so that it can be used in any context.
/// </summary>
public class DialogueManager : MonoBehaviour {
    //setting up singleton
    public static DialogueManager Instance = null;

    [SerializeField]
    private DialogueScene dialogueScene;

    private Translator translator;
    private DialogueSegment currentDialogueSegment;
    private string currentTranslatedDialogueText;

    [SerializeField]
    private Text dialogueText, speakerNameText;

    [SerializeField]
    private Image speakerImage;

    [SerializeField]
    private GameObject dialogueBox, gameplayUI;

    [SerializeField]
    public Font comfortaa, openDyslexic;

    public bool inDialogue;
    private bool listeningForSkips = false;
    int currentDialogueSegmentIndex = 0;
    float startTime;
    float lettersPerSecond = 32f;

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
        if (SettingsManager.Instance.DyslexicMode) {
            RespectDyslexia();
        } else {
            UseStandardFont();
        }

        translator = GameManager.instance.GetComponent<Translator>();
    }

    // Update is called once per frame
    void LateUpdate () {
        if (inDialogue) {
            int length = Mathf.Min(currentTranslatedDialogueText.Length, (int)(lettersPerSecond * (Time.time - startTime)));

            //display letter after letter
            if (dialogueText.text != currentTranslatedDialogueText)
            {
                dialogueText.text = currentTranslatedDialogueText.Substring(0, length);
                HandleVoice();
            }
            else
            {
                AudioManager.instance.StopEvent(currentDialogueSegment.dialogueSpeaker.voiceEvent, 0);
            }

            //Skip dialogue text
            if (GetValidSkip())
            {
                //show all text
                if (dialogueText.text != currentTranslatedDialogueText)
                {
                    dialogueText.text = currentTranslatedDialogueText;
                }
                else
                //if there are more dialogue segments to display, display a new one
                if (currentDialogueSegmentIndex < dialogueScene.dialogueSegments.Length - 1)
                {
                    currentDialogueSegmentIndex++;
                    currentDialogueSegment = dialogueScene.dialogueSegments[currentDialogueSegmentIndex];
                    currentTranslatedDialogueText = translator.GetTranslation(currentDialogueSegment.dialogueString);
                    ClearTextBox();
                    UpdateSpeakerInfo();
                }
                else
                {
                    HideTextBox();
                }
            }
        }
    }

    private bool GetValidSkip()
    {
        if (!listeningForSkips) return false;

        return Input.GetKeyDown(KeyCode.LeftControl) 
            || Input.GetKeyDown(KeyCode.Joystick2Button0) 
            || Input.GetKeyDown(KeyCode.RightControl) 
            || Input.GetKeyDown(KeyCode.Joystick1Button0);
    }

    private void ClearTextBox()
    {
        startTime = Time.time;
        dialogueText.text = "";
    }

    public void LoadDialogueScene(DialogueScene newDialogueScene)
    {
        dialogueScene = newDialogueScene;
    }

    public void EnterDialogue()
    {
        if (GameController.instance != null)
        {
            //disable the players
            GameController.instance.player1.enabled = false;
            GameController.instance.player2.enabled = false;
        }

        listeningForSkips = false;
        startTime = Time.time;
        ClearTextBox();
        currentDialogueSegmentIndex = 0;
        currentDialogueSegment = dialogueScene.dialogueSegments[0];
        currentTranslatedDialogueText = translator.GetTranslation(currentDialogueSegment.dialogueString);
        UpdateSpeakerInfo();
        gameplayUI.SetActive(false);
        dialogueBox.SetActive(true);
        inDialogue = true;

        //start listening after 0.1f seconds
        Invoke("StartListeningForSkips", 0.1f);
    }

    private void HideTextBox()
    {
        if (GameController.instance != null) {
            //enable the players
            GameController.instance.player1.enabled = true;
            GameController.instance.player2.enabled = true;
        }

        dialogueBox.SetActive(false);
        gameplayUI.SetActive(true);
        inDialogue = false;

        EventManager.ThrowDialogueEvent();
    }

    private void UpdateSpeakerInfo()
    {
        speakerImage.sprite = currentDialogueSegment.dialogueSpeaker.speakerSprite;
        speakerNameText.text = translator.GetTranslation(currentDialogueSegment.dialogueSpeaker.speakerName);

        //only emote if a dialogue actor can be found in the scene
        if (GameObject.Find(currentDialogueSegment.dialogueSpeaker.speakerName) != null)
        {
            GameObject actorGO = GameObject.Find(currentDialogueSegment.dialogueSpeaker.speakerName);
            DialogueActor actor = actorGO.GetComponent<DialogueActor>();
            actor.Act(currentDialogueSegment);
        }
    }

    private void StartListeningForSkips()
    {
        listeningForSkips = true;
    }

    private void HandleVoice()
    {
        if (Random.Range(0, 100) < 10)
        {
            string eventName = currentDialogueSegment.dialogueSpeaker.voiceEvent;
            AudioManager.instance.StopEvent(eventName, 0);
            AudioManager.instance.PlayEvent(eventName);
        }
    }

    public void RespectDyslexia()
    {
        dialogueText.font = openDyslexic;
        speakerNameText.font = openDyslexic;
    }

    public void UseStandardFont()
    {
        dialogueText.font = comfortaa;
        speakerNameText.font = comfortaa;
    }
}


[System.Serializable]
public class DialogueSegment : System.Object
{
    public DialogueSpeaker dialogueSpeaker;
    [TextArea(3, 10)]
    public string dialogueString;
    public GameObject particles;
    public AnimationClip animation;
}