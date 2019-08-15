using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : PickUp {
    float carryDistance = 0.85f; //hardcoded

    public override void Pickup(Transform newParent)
    {
        transform.SetParent(newParent);
        Debug.Log("carryDistance is" + carryDistance);
        transform.position = newParent.position + carryDistance * newParent.forward + 0.5f * Vector3.up;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        GetComponent<Collider>().isTrigger = true;
        Invoke("MakeKinematic", 0.1f);
        gameObject.layer = 11; //prevent collisions with the player

        isPickedUp = true;

        //make parent uninteractable to prevent players from picking each other up
        SetInteractableStatus(newParent.GetComponent<PlayerPickUp>(), false);

        //move parents collider forward to compensate
        newParent.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0.5f);

        //player animation
        GetComponent<PlayerController>().UpdateBeingCarriedAnimation(true);
    }

    private void Update()
    {
        if (isPickedUp)
        {
            transform.position = transform.parent.position + carryDistance * transform.parent.forward + 0.5f * Vector3.up;
        }
    }

    public override void PutDown()
    {
        ReverToState();
    }

    public override void ReverToState()
    {
        isPickedUp = false;

        if (transform.parent != null)
        {
            //if parent is a player
            if (transform.parent.GetComponent<PlayerController>() != null)
            {
                transform.parent.GetComponent<PlayerController>().holdingSomething = false;

                //move parent collider backwards again
                //move parents collider forward to compensate
                transform.parent.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);

                //reset player animation
                //player animation
                GetComponent<PlayerController>().UpdateBeingCarriedAnimation(false);

                //make parent interactable again to prevent players from picking each other up
                SetInteractableStatus(transform.parent.GetComponent<PlayerPickUp>(), true);
                transform.SetParent(null);
            }
        }
        GetComponent<Rigidbody>().useGravity = true;

        //always a player make kinematic
        GetComponent<Collider>().isTrigger = false;
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.layer = 0; //prevent collisions with the player
    }

    private void MakeKinematic() {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SetInteractableStatus(Interactable interactableObject, bool state)
    {
        interactableObject.interactable = state;
    }
}