using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enormius : Ingredient {
    PlayerController carryingPlayer;

    Rigidbody rb;
    public Animator animator;

    [SerializeField]
    private bool isGrounded;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        sleepTime = 2f;
	}

    private void Update()
    {
        if (sleepTime > 0) sleepTime -= Time.deltaTime;

        if (!isPickedUp && isGrounded)
        {
            Quaternion desired = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 0.02f * Time.time);
            transform.rotation = desired;
        }
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        Debug.Log("Enormius Pickup()");
        carryingPlayer = newParent.GetComponent<PlayerController>();
        carryingPlayer.slowedEffect = true;

        animator.SetBool("pickedUp", true);
    }

    public override void PutDown()
    {
        base.PutDown();
        if (carryingPlayer != null)
        {
            carryingPlayer.slowedEffect = false;
        }

        animator.SetBool("pickedUp", false);
    }

    public override void Throw()
    {
        base.Throw();
        carryingPlayer.slowedEffect = false;

        animator.SetBool("pickedUp", false);
    }

    //turn upright when colliding
    private void OnCollisionEnter(Collision collision)
    {
        //ignore players
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            return;
        }

        //play audio
        AudioManager.instance.PlayEvent("Play_plant_fall_impact");


        //check if it is in the air and ground if not
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }
}
