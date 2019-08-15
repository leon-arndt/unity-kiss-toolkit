using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationBall : PickUp {
    PlayerController host;
    
    // Stun plants by throwing coal at them
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < 1f) return;

        if (other.GetComponent<PlayerController>() != null)
        {
            if (other.GetComponent<PlayerController>() != host)
            {
                other.GetComponent<PlayerController>().AddDizzyTime(2);
                ParticleManager.Instance.CreateStunParticle(other.transform);
            }
        }
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        host = newParent.GetComponent<PlayerController>();
    }

    public override void Throw()
    {
        base.Throw();
        host = null;
    }

    public override void PutDown()
    {
        base.PutDown();
        host = null;
    }
}
