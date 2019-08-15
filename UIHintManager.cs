using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UIHint Creation.
/// This script needs to be attached to the world canvas.
/// </summary>
public class UIHintManager : MonoBehaviour {
    //setting up singleton
    public static UIHintManager instance = null;

    Canvas canvas;

    [SerializeField]
    Sprite spriteToUse, keyboardSpriteToUse, playerPickupSprite;

    [SerializeField]
    Material materialToUse;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void CreateUIHint(Transform playerTransform, Transform interactableTransform)
    {
        GameObject uiObj = new GameObject();

        uiObj.AddComponent<Billboard>();
        Image uiImage = uiObj.AddComponent<Image>();
        uiImage.rectTransform.sizeDelta = new Vector2(0.6f, 0.6f);

        //if the interactable is a player
        if (interactableTransform.GetComponent<PlayerController>() != null)
        {
            uiImage.sprite = playerPickupSprite;
        }
        else
        if (playerTransform.GetComponent<PlayerController>().playerID < Input.GetJoystickNames().Length)
        {
            //figure out which sprite image to use
            uiImage.sprite = spriteToUse;
        }
        else
        {
            uiImage.sprite = keyboardSpriteToUse;
        }

        uiImage.material = materialToUse;

        UIHint uiHint = uiObj.AddComponent<UIHint>();
        uiHint.playerTransform = playerTransform;
        uiHint.transformToTrack = interactableTransform;

        //set UIhint as interactables uihint
        interactableTransform.GetComponent<Interactable>().uiHint = uiHint;

        uiObj.name = "UIHint";
        uiObj.transform.position = interactableTransform.position;
        uiObj.GetComponent<RectTransform>().SetParent(canvas.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        uiObj.SetActive(true); //Activate the GameObject
    }

    public bool ShouldCreateUIHint(Interactable interactable)
    {
        //check to see that there are no other uihints on this interactable component
        if (interactable.uiHint != null)
        {
            return false;
        }
        
        //ignore picked up players
        if (interactable is PlayerPickUp)
        {
            if (interactable.GetComponent<PlayerPickUp>().isPickedUp == true)
            {
                return false;
            }
        }

        //ignore unbroken pipes
        if (interactable is Pipe)
        {
            if (!interactable.GetComponent<Pipe>().IsBroken())
                return false;
        }

        //ignore unbroken gear
        if (interactable is Gear)
        {
            if (!interactable.GetComponent<Gear>().IsBroken())
                return false;
        }
        return true;
    }
}
