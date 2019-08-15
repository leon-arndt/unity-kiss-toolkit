using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : PickUp {

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
    }

    public override void PutDown()
    {
        base.PutDown();
    }

    public override void Interact(Transform newParent)
    {
        //base.Interact();
        Pickup(newParent);
    }

    public override void Throw()
    {
        base.Throw();
        GetComponent<BoxCollider>().enabled = true;
    }

    // Stun plants by throwing coal at them
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < 1f) return;

        if (other.GetComponent<Ingredient>() != null)
        {
            other.GetComponent<Ingredient>().AddSleepTime(3);
            ParticleManager.Instance.CreateStunParticle(other.transform);
        }
    }
}
