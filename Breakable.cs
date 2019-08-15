using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is an abstract class is used by Gear and Pipe who inherit from it
public abstract class Breakable : Interactable
{
    //tracks how much damage this breakable has caused to the vehicle since breaking
    public float damageCaused;

    //abstract methods which must be implemented
    public abstract void Break();
    public abstract void Fix();
}
