using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides the player with hints at the train station.
/// </summary>
public class StationGuide : Interactable {
    [SerializeField]
    DialogueScene helloScene;

    [SerializeField]
    DialogueScene[] randomHints;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Animation uiAnimation;

    [SerializeField]
    AnimationClip shrinkDisappearClip;

    [SerializeField]
    GameObject talkingBubble;

    private int lastSceneIndex = 0;
    private bool talkedBefore;

    private void Start()
    {
        if (GameManager.instance.globalSaveData.teacupCount > 0)
        {
            talkingBubble.SetActive(false);
        }
        else
        {
            animator.SetBool("wave", true);
        }
    }

    public override void Interact(Transform transform)
    {
        animator.SetBool("wave", false);

        //hide the bubble
        if (talkingBubble != null)
        {
            uiAnimation.Play(shrinkDisappearClip.name);
            Destroy(talkingBubble, shrinkDisappearClip.length);
        }

        if (GameManager.instance.globalSaveData.teacupCount == 0 && !talkedBefore)
        {
            DisplayHello();
        }
        else
        {
            DisplayRandomHint();
        }
    }

    private int GenerateNewIndex()
    {
        int desiredIndex = Random.Range(0, randomHints.Length);

        while (desiredIndex == lastSceneIndex)
        {
            desiredIndex = Random.Range(0, randomHints.Length);
        }

        lastSceneIndex = desiredIndex;

        return desiredIndex;
    }


    private void DisplayHello()
    {
        talkedBefore = true;

        if (!DialogueManager.Instance.inDialogue)
        {
            DialogueManager.Instance.LoadDialogueScene(helloScene);
            DialogueManager.Instance.EnterDialogue();
        }
    }

    private void DisplayRandomHint()
    {
        DialogueScene sceneToLoad;
        int indexToLoad = GenerateNewIndex();
        sceneToLoad = randomHints[indexToLoad];

        if (!DialogueManager.Instance.inDialogue)
        {
            DialogueManager.Instance.LoadDialogueScene(sceneToLoad);
            DialogueManager.Instance.EnterDialogue();
        }
    }
}
