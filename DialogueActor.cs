using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DialogueActors are character game objects in the world which display animations and particles.
/// This script works in conjunction with the dialogue manager and dialogue segments.
/// The particles are created from prefabs which are stored in the dialogue segments.
/// Animators must have a reference (state) in their ttree with the desired animation.
/// </summary>
public class DialogueActor : MonoBehaviour {
    public DialogueSpeaker dialogueSpeaker;
    private Animator animator;

	// Use this for initialization
	void Start () {
        gameObject.name = dialogueSpeaker.speakerName;
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            animator = GetComponentInChildren<Animator>();
        }
	}
	
    public void Act(DialogueSegment dialogueSegment)
    {
        //particles
        if (dialogueSegment.particles != null)
            Instantiate(dialogueSegment.particles);

        //animations
        if (dialogueSegment.animation != null)
            animator.Play(dialogueSegment.animation.name);
    }
}
