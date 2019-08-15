using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component stores data that is persistent on the GameManager but not saved.
/// This includes hats which are scriptable objects.
/// </summary>
[System.Serializable]
public class PersistentData : System.Object
{
    public Hat p1CurrentHat;
    public Hat p2CurrentHat;
}
