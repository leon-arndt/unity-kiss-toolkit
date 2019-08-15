using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object which contains an array of dialogue segements. Used by the DialogueManager to displayer dialogue.
/// </summary>
[CreateAssetMenu(fileName = "New DialogueScene", menuName = "DialogueScene", order = 3)]
public class DialogueScene : ScriptableObject {
    public DialogueSegment[] dialogueSegments;
}
