using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Used to create mechanic tutorial videos as scriptable objects in the project.
/// These files cointain a video and a description which explains how to play parts of the game.
/// </summary>
[CreateAssetMenu(fileName = "New Mechanic Video", menuName = "TutorialVideo", order = 3)]
public class MechanicVideoData : ScriptableObject
{
    public string titleId;
    public string descId;
    public VideoClip videoClip;
}
