using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple system to help the player learn how to play the game.
/// Works together with the DialogueManager and the Event Manager.
/// </summary>
public class TutorialController : MonoBehaviour {
    //setting up singleton
    public static TutorialController instance = null;

    private int tutorialStep;

    [SerializeField]
    public DialogueScene introTutorial, slotmachineTutorial, potTutorial, binTutorial, coalTutorial, repairingTutorial, finishedTutorial;

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
    private void Start()
    {
        //Listening for special interactions during the tutorial
        if (GameManager.instance.activeLevel.showTutorial)
        {
            //using the slot machine shows the pot tutorial
            EventManager.OnSlotMachineUse += LoadPotTutorial;
            EventManager.OnSlotMachineUse += DialogueManager.Instance.EnterDialogue;
            EventManager.OnSlotMachineUse += EventManager.ClearSlotMachineEvent;

            //adding ingredient to the pot shows the coal tutorial (also spawn coal next)
            EventManager.OnAddIngredient += LoadCoalTutorial;
            EventManager.OnAddIngredient += DialogueManager.Instance.EnterDialogue;
            EventManager.OnAddIngredient += NetController.Instance.SpawnCoalNext;
            EventManager.OnAddIngredient += EventManager.ClearAddIngredientEvent;

            //adding coal shows the repair tutorial (also break a pipe)
            EventManager.OnAddCoal += SurpriseManager.Instance.BreakPipe;
            EventManager.OnAddCoal += LoadRepairingTutorial;
            EventManager.OnAddCoal += DialogueManager.Instance.EnterDialogue;
            
            
            //this is also the end of the tutorial
            EventManager.OnPipeFixed += FinishTutorial;
        }
    }

    //these sadly remain here until I find a way to pass parameters to events :(
    private void LoadPotTutorial()
    {
        DialogueManager.Instance.LoadDialogueScene(potTutorial);
    }

    private void LoadBinTutorial()
    {
        DialogueManager.Instance.LoadDialogueScene(binTutorial);
    }

    private void LoadCoalTutorial()
    {
        DialogueManager.Instance.LoadDialogueScene(coalTutorial);
    }

    private void LoadRepairingTutorial()
    {
        DialogueManager.Instance.LoadDialogueScene(repairingTutorial);
    }

    private void FinishTutorial()
    {
        DialogueManager.Instance.LoadDialogueScene(finishedTutorial);
        DialogueManager.Instance.EnterDialogue();
        EventManager.StopListeningForTutorialEvents();

        //win the game after the tutorial is completed
        EventManager.ClearDialogueEvent();
        EventManager.OnDialogueEnd += GameController.instance.WinGame;
    }
}
