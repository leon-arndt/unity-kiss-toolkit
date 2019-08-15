using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used for the customers at the end of the game to let the player talk to them.
/// Carries similarities to the StationGuide component.
/// </summary>
public class SimpleSpeaker : Interactable {
    [SerializeField]
    DialogueScene dialogue;

    public override void Interact(Transform transform)
    {
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        if (!DialogueManager.Instance.inDialogue)
        {
            DialogueManager.Instance.LoadDialogueScene(dialogue);
            DialogueManager.Instance.EnterDialogue();
        }
    }
}
