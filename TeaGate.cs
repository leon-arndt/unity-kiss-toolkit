using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to handle both tea gates.
/// The tea gates are mysterious objects in the train station which open up once the player has connected enough teacups.
/// </summary>
public class TeaGate : Interactable {
    [SerializeField]
    DialogueScene dialogueScene;

    [SerializeField]
    GameObject[] gateCups;

    [SerializeField]
    GameObject[] doors;

    [SerializeField]
    Collider doorCollider;

    [SerializeField]
    public uint teacupRequirement;

    [SerializeField]
    TeaGate previousTeaGate;

    private void Start()
    {
        int teacupsEarned = GameManager.instance.globalSaveData.teacupCount;
        //ignore the once needed to reach the last tea gate if there was one
        uint ignoreTeacups = 0;
        if (previousTeaGate != null)
        {
            ignoreTeacups = previousTeaGate.teacupRequirement;
        }

        for (int i = 0; i < gateCups.Length; i++)
        {
            if (teacupsEarned - ignoreTeacups > i)
            {
                gateCups[i].SetActive(true);
            }
            else
            {
                gateCups[i].SetActive(false);
            }
        }

        //open the doors once the player has earned enough teacups
        if (teacupsEarned - ignoreTeacups > teacupRequirement)
        {
            foreach (var door in doors)
            {
                door.SetActive(false);
                doorCollider.enabled = false;
            }
        }
    }
    public override void Interact(Transform transform)
    {
        if (!DialogueManager.Instance.inDialogue)
        {
            DialogueManager.Instance.LoadDialogueScene(dialogueScene);
            DialogueManager.Instance.EnterDialogue();
        }
    }
}
