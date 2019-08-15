using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scipt is used for the pulldown lever which operates the pot.
/// It works together with the pot scripts.
/// </summary>
public class PulldownLever : Interactable
{
    bool isPulledDown = false;
    float pullSpeed = 4f;
    Vector3 startPosition, potStartPosition, gearStartRotation;
    Transform pullingPlayer;

    [SerializeField]
    Transform leverTransform, leverOnTransform, potTransform, leverLightTurner;

    [SerializeField]
    private Light signalLight;

    [SerializeField]
    private Color enabledColor, disabledColor;

    [SerializeField]
    private Collider potLidCollider;

    [SerializeField]
    private Animator animator;

    private float leverLightTurnSpeed = 6f;
    private float potMoveDistance = 1f;

    private void Start()
    {
        startPosition = leverTransform.localPosition;
        potStartPosition = potTransform.localPosition;
    }

    public override void Interact(Transform playerTransform)
    {
        if (!Pot.Instance.isLocked)
        {
            isPulledDown = true;
            pullingPlayer = playerTransform;

            animator.SetBool("Pressed", true);

            //audio
            AudioManager.instance.PlayEvent("Play_pot_open");
        }
    }

    private void Update()
    {
        if (isPulledDown)
        {
            PullDown();

            //check if the player released the lever
            Rewired.Player player;
            int playerID = pullingPlayer.GetComponent<PlayerController>().playerID;
            player = Rewired.ReInput.players.GetPlayer(playerID);

            if (player.GetButtonUp("Action"))
            {
                isPulledDown = false;
                potLidCollider.isTrigger = false;

                animator.SetBool("Pressed", false);
                
                //audio
                AudioManager.instance.PlayEvent("Play_pot_close");
            }
        }
        else
        {
            Pot.Instance.isOpen = false;
            Pot.Instance.animator.SetBool("isOpen", false);
            LetGo();

            animator.SetBool("Pressed", false);

            //turn the lever light while the pot is locked
            if (Pot.Instance.isLocked)
            {
                leverLightTurner.Rotate(leverLightTurnSpeed * Vector3.up * Time.timeScale);
            }
        }
    }

    private void PullDown()
    {
        Vector3 leverDestination = startPosition - 0.6f * Vector3.up;
        leverTransform.localPosition = Vector3.Lerp(leverTransform.localPosition, leverDestination, pullSpeed * Time.deltaTime);
        potTransform.localPosition = Vector3.Lerp(potTransform.localPosition, potStartPosition + potMoveDistance * Vector3.up, pullSpeed * Time.deltaTime);

        if (Vector3.Distance(potTransform.localPosition, potStartPosition + potMoveDistance * Vector3.up) < 1)
        {
            Pot.Instance.isOpen = true;
            Pot.Instance.animator.SetBool("isOpen", true);
            potLidCollider.isTrigger = true;
        }
    }


    private void LetGo()
    {
        leverTransform.localPosition = Vector3.Lerp(leverTransform.localPosition, startPosition, pullSpeed * Time.deltaTime);
        potTransform.localPosition = Vector3.Lerp(potTransform.localPosition, potStartPosition, pullSpeed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == pullingPlayer)
        {
            isPulledDown = false;
        }
    }

    public void SetPullDownState(bool set)
    {
        isPulledDown = set;
        if (!set)
        {
            AudioManager.instance.PlayEvent("Play_pot_close");
        }
    }

    public void MakePotLidColliderNonTrigger()
    {
        potLidCollider.isTrigger = false;
    }

    public void UpdateSignalLightColor(bool green)
    {
        if (green)
        {
            signalLight.color = enabledColor;
        }
        else
        {
            signalLight.color = disabledColor;
        }
    }
}
