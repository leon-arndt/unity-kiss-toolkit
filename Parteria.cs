using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Both parts of Parteria use the same class. This is similar to how the PlayerController is used.
/// To keep functionality intact there is a isParent field to differentiate them from each other.
/// Unique is that there is also a combiend model with its own animator which needs to be implemented.
/// </summary>
public class Parteria : Ingredient {
    Rigidbody rb;
    public bool connected = false;
    public bool isParent = false;

    [SerializeField]
    Animator animator, combinedAnimator;

    private PlayerController carryingPlayer;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        if (isParent)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //movement
        if (!isPickedUp && !connected)
        {

            var hitInFront = Physics.Raycast(transform.position, transform.TransformDirection(-transform.forward), 2f);

            if (!hitInFront)
            {
                //move forward if free in front
                if (rb.velocity.magnitude < 5)
                {
                    rb.AddForce(50f * transform.forward);
                }

                //turn right if parent
                if (isParent)
                {
                    transform.Rotate(Vector3.up);
                }
                else
                {
                    //else turn left
                    transform.Rotate(Vector3.down);
                }
            }
            else
            {
                //turn right
                transform.Rotate(Vector3.up);
            }
        }

        //movement animation
        animator.SetFloat("movement", rb.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Parteria>() != null)
        {
            if (!connected)
            {
                //connect when both are picked up
                if (isPickedUp && other.GetComponent<Parteria>().isPickedUp)
                {
                    ConnectPieces(other);
                }
            }
        }
    }

    private void ConnectPieces(Collider other)
    {
        Debug.Log("Connect Parterias");

        //parent other to self if designated parent
        if (isParent)
        {
            //is a parent, exchange for parteria combined model
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            animator = combinedAnimator;

            //turn off the rigidbodies
            rb.isKinematic = true;
            other.GetComponent<Rigidbody>().isKinematic = true;

            //connect both at once so this method is not called twice
            connected = true;
            other.GetComponent<Parteria>().connected = true;

            AudioManager.instance.PlayEvent("Play_plant_connect");

            //child section

            //make sure the holding player drops the held child parteria
            other.GetComponent<Parteria>().carryingPlayer.DropObject();
            
            //destroy the child
            Destroy(other.gameObject);
        }
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        ChangePickedUpBool(true, connected);

        carryingPlayer = newParent.GetComponent<PlayerController>();
    }

    public override void PutDown()
    {
        base.PutDown();
        ChangePickedUpBool(false, connected);

        carryingPlayer = null;
    }

    public override void Throw()
    {
        base.Throw();
        ChangePickedUpBool(false, connected);

        carryingPlayer = null;
    }

    private void ChangePickedUpBool(bool pickedUpState, bool combined)
    {
        if (combined)
        {
            combinedAnimator.SetBool("pickedUp", pickedUpState);
        }
        else
        {
            animator.SetBool("pickedUp", pickedUpState);
        }
    }
}
