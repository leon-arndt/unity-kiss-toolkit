using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is the basis for the data of every game levels.
/// The game levels in Tea on Rails are not stored in separate scenes but in these data containers.
/// This is a safer method of handling content generation which is less prone to leaking data.
/// </summary>
[CreateAssetMenu(fileName = "New Hat", menuName = "Hat", order = 3)]
public class Hat : ScriptableObject {
    public string hatName;
    public string hatDescription;
    public Texture2D hatTexture;
    public GameObject hatObject;
    public Vector3 positionOffset;
    public float scaleFactor = 0.5f;
    public int teacupsNeeded;
}