using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Custom Input Manager class. Written by Leon Arndt.
/// It is employed by the PlayerController class for actions.
/// Since both keyboard and controller is supported by the game we use arrays to handle the input.
/// Movement is handled by the Unity Input Manager.
/// </summary>
public class CustomInputManager : MonoBehaviour {
    public static bool GetKeyPressed(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            return true;
        }

        return false;
    }

    //dealing with an array
    public static bool GetKeyPressed(KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
                return true;
        }

        return false;
    }

    public static bool GetKeyReleased(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            return true;
        }

        return false;
    }

    //dealing with an array
    public static bool GetKeyReleased(KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyUp(key))
                return true;
        }

        return false;
    }

    public static bool GetButton(string buttonName)
    {
        if (Input.GetButton(buttonName))
        {
            return true;
        }

        return false;
    }
}
