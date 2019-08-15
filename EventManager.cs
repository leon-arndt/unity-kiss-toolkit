using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple messaging system. Objects can start and stop listening for actions.
/// Used for the DialogueManager to tell the VehicleController to start driving.
/// Also used for interactions during the tutorial.
/// </summary>
public class EventManager : MonoBehaviour {
    public delegate void DialogueEndAction();
    public static event DialogueEndAction OnDialogueEnd;

    public delegate void SlotMachineUseAction();
    public static event SlotMachineUseAction OnSlotMachineUse;

    public delegate void AddIngredientAction();
    public static event AddIngredientAction OnAddIngredient;

    public delegate void AddCoalAction();
    public static event AddCoalAction OnAddCoal;

    public delegate void FinishedCookingAction();
    public static event FinishedCookingAction OnFinishedCooking;

    public delegate void PipeFixedAction();
    public static event PipeFixedAction OnPipeFixed;


    //Clear Events
    public static void StopListeningForTutorialEvents()
    {
        EventManager.OnSlotMachineUse = null;
        EventManager.OnAddIngredient = null;
        EventManager.OnFinishedCooking = null;
        EventManager.OnAddCoal = null;
        EventManager.OnPipeFixed = null;
    }

    public static void ClearSlotMachineEvent()
    {
        EventManager.OnSlotMachineUse = null;
    }

    public static void ClearAddIngredientEvent()
    {
        EventManager.OnAddIngredient = null;
    }

    public static void ClearAddCoalEvent()
    {
        EventManager.OnAddCoal = null;
    }

    public static void ClearFinishedCookingEvent()
    {
        EventManager.OnFinishedCooking = null;
    }

    public static void ClearDialogueEvent()
    {
        EventManager.OnDialogueEnd = null;
    }


    // Throw events
    public static void ThrowDialogueEvent()
    {
        if (EventManager.OnDialogueEnd != null)
            EventManager.OnDialogueEnd();
    }

    public static void ThrowSlotMachineEvent()
    {
        if (EventManager.OnSlotMachineUse != null)
            EventManager.OnSlotMachineUse();
    }

    public static void ThrowAddIngredientEvent()
    {
        if (EventManager.OnAddIngredient != null)
            EventManager.OnAddIngredient();
    }

    public static void ThrowAddCoalEvent()
    {
        if (EventManager.OnAddCoal != null)
            EventManager.OnAddCoal();
    }

    public static void ThrowFinishedCookingEvent()
    {
        if (EventManager.OnFinishedCooking != null)
            EventManager.OnFinishedCooking();
    }

    public static void ThrowPipeFixedEvent()
    {
        if (EventManager.OnPipeFixed != null)
            EventManager.OnPipeFixed();
    }


}
