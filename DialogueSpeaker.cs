using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object which contains data for a speaker during dialogue.
/// </summary>
[CreateAssetMenu(fileName = "New DialogueSpeaker", menuName = "DialogueSpeaker", order = 3)]
public class DialogueSpeaker : ScriptableObject
{
    public string speakerName;
    public string voiceEvent;
    public Sprite speakerSprite;
}

