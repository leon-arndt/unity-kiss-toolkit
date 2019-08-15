using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The QTEManager provides a global sprite reference source for all QTE instances.
/// It is accessed through the singleton pattern (QTEManager.Instance)
/// </summary>
public class QTEManager : MonoBehaviour {
    //setting up singleton
    public static QTEManager Instance = null;

    //All the valid keyboard codes for keyboard and controller
    public List<KeyCode> validP1Keys, validP2Keys;
    public List<KeyCode> validJoy1Buttons;
    public List<KeyCode> validJoy2Buttons;

    public Sprite aButton, bButton, xButton, yButton, transparent;
    public Sprite[] redJoyconSprites, blueJoyconSprites;

    //Singleton
    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }
}
