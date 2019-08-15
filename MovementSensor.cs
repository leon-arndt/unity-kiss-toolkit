using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by the throw particles to detect when the thrown pickup has stopped moving.
public class MovementSensor : MonoBehaviour {
    [SerializeField]
    ParticleSystem particleSystem;

    [SerializeField]
    bool destroyOnMovementStop;

    private Rigidbody rb;

    private float destroyDelay = 0.66f;
    private float lowerThreshold = 1.4f;

	// Use this for initialization
	void Start () {
        rb = transform.parent.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rb.velocity.magnitude < lowerThreshold)
        {
            StopAction();
        }
	}

    void StopAction()
    {
        if (destroyOnMovementStop)
        {
            //vfx particle systems
            var emission = particleSystem.emission;
            emission.enabled = false;
            Destroy(gameObject, destroyDelay);
        }
    }

}
