using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(BoxCollider))]
/// <summary>
/// This script is used for the seats with which the player can interact.
/// Works together with the PlayerController and Rewired.
/// </summary>
public class Seat : Interactable {
    PlayerController occupyingPlayer = null;
    Player playerInput = null;

    [SerializeField]
    Transform seatDepartTransform;

    bool seatFull;

	// Update is called once per frame
	void Update () {
        if (seatFull)
        {
            Vector3 desiredPosition = Vector3.Lerp(occupyingPlayer.transform.position, transform.position, 3f * Time.deltaTime);
            occupyingPlayer.transform.position = desiredPosition;

            //check whether to unseat player (when they start moving)))
            float moveHorizontal = playerInput.GetAxis("Move Horizontal");
            float moveVertical = playerInput.GetAxis("Move Vertical");
            if (Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f) 
            {
                UnsitPlayer(occupyingPlayer.transform);
            }
        }		
	}

    public override void Interact(Transform callingTransform)
    {
        if (!seatFull)
        {
            SeatPlayer(callingTransform);
        }
        else
        {
            UnsitPlayer(occupyingPlayer.transform);
        }
    }

    private void SeatPlayer(Transform playerTransform)
    {
        seatFull = true;
        
        //callingTransform.position = transform.position;
        occupyingPlayer = playerTransform.GetComponent<PlayerController>();
        occupyingPlayer.turnSpeed = 0f;

        //update rewired player
        playerInput = ReInput.players.GetPlayer(occupyingPlayer.playerID);

        occupyingPlayer.animator.SetBool("seated", true);
        playerTransform.GetComponent<CapsuleCollider>().enabled = false;
        playerTransform.GetComponent<Rigidbody>().isKinematic = true;

        //player can no longer be picked up
        occupyingPlayer.GetComponent<PlayerPickUp>().interactable = false;
    }

    private void UnsitPlayer(Transform playerTransform)
    {
        StartCoroutine(UnsitPlayerCoroutine(playerTransform));
    }

    IEnumerator UnsitPlayerCoroutine(Transform playerTransform)
    {
        occupyingPlayer.animator.SetBool("seated", false);
        seatFull = false;

        for (float f = 0f; f <= 1; f += 0.1f)
        {
            Vector3 desiredPosition = Vector3.Lerp(playerTransform.transform.position, seatDepartTransform.position, 2f * Time.deltaTime);
            playerTransform.position = desiredPosition;
            yield return null;
        }

        playerTransform.GetComponent<CapsuleCollider>().enabled = true;
        playerTransform.GetComponent<Rigidbody>().isKinematic = false;
        occupyingPlayer.turnSpeed = PlayerController.STANDARD_TURN_SPEED;

        occupyingPlayer = null;
        playerInput = null;
    }
}
