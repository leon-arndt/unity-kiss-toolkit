using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable {
    public bool isPickedUp = false;
    public bool isBeingThrown = false; //is the currently being thrown and in air
    public bool dangerous = false; //should this pickup stun characters when thrown?

    private void FixedUpdate()
    {
        if (isPickedUp)
        {
            //turn upright if skewed and mirror the y rotation of the parent object
            float yRot;
            if (transform.parent != null)
            {
                yRot = transform.parent.eulerAngles.y;
            }
            else
            {
                yRot = transform.eulerAngles.y;
            }

            Quaternion desired = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, yRot, 0f), 0.01f * Time.time);
            transform.rotation = desired;

            //find the position to hold the object
            float carryDistance = CalculateCarryDistance();
            Vector3 desiredPos = transform.parent.position + carryDistance * transform.parent.forward + 0.1f * Vector3.up;

            //difference vector to get the desired velocity, disabled because velocity should not be added to kinematic objects
            Vector3 desiredVelocity = desiredPos - transform.position;
            Vector3 moveVector = desiredVelocity * 50f;
        }
    }

    public float CalculateCarryDistance()
    {
        float carryDistance = 0.5f + 0.5f * GetComponent<Collider>().bounds.size.x;
        return carryDistance;
    }

    public virtual void Pickup(Transform newParent)
    {
        transform.SetParent(newParent);
        float carryDistance = CalculateCarryDistance();
        Debug.Log("carryDistance is" + carryDistance);
        transform.position = newParent.position + carryDistance * newParent.forward + 0.1f * Vector3.up;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //if not a player
        if (GetComponent<PlayerController>() == null)
        {
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = 11; //prevent collisions with the player
        }

        isPickedUp = true;
    }

    public virtual void ReverToState()
    {
        isPickedUp = false;

        if (transform.parent != null)
        {
            //if a player with a parent
            if (transform.parent.GetComponent<PlayerController>() != null)
            {
                transform.parent.GetComponent<PlayerController>().holdingSomething = false;
                transform.SetParent(null);
            }   
        }
        GetComponent<Rigidbody>().useGravity = true;

        //if not a player
        if (GetComponent<PlayerController>() == null)
        {
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().isKinematic = false;
            gameObject.layer = 0; //prevent collisions with the player
        }
    }

    public virtual void PutDown()
    {
        ReverToState();
    }

    public virtual void Throw()
    {
        isBeingThrown = true;

        ParticleManager.Instance.CreateThrowParticles(transform);

        Transform tempParent = transform.parent;
        ReverToState();

        float speedToThrowWith = tempParent.GetComponent<Rigidbody>().velocity.magnitude + 12f;
        GetComponent<Rigidbody>().velocity = tempParent.forward * speedToThrowWith;

        float torqueAmount = 10000;
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-torqueAmount, torqueAmount), Random.Range(-torqueAmount, torqueAmount), Random.Range(-torqueAmount, torqueAmount)));
    }


    public override void Interact(Transform newParent)
    {
        Pickup(newParent);
    }

    //hurt others if dangerous
    private void OnCollisionEnter(Collision collision)
    {
        if (isBeingThrown)
        {
            isBeingThrown = false;

            if (!dangerous) return;

            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                collision.gameObject.GetComponent<PlayerController>().AddDizzyTime(2f);
            }
            return;
        }
    }
}