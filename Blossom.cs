using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blossom : Ingredient {
    public Rigidbody rb;
    public float forwardForce, directionChangeAmount;

    private float repeatRate = 3f;
    private float firstJumpDelay = 0.5f;
    private float moveForwardTime = 0f;
    private float turnTime = 0f;

    [SerializeField]
    Animator animator;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("Jump", firstJumpDelay, repeatRate);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPickedUp)
        {
            if (moveForwardTime > 0f)
            {
                moveForwardTime -= Time.deltaTime;

                if (moveForwardTime > 0.1f)
                {
                    MoveForward();
                }
            }

            if (turnTime > 0.1f)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.5f))
                {
                    transform.Rotate(directionChangeAmount * 2f * Vector3.up, Space.World);
                    turnTime -= Time.deltaTime;
                }
            }
        }
	}

    void Jump()
    {
        animator.SetTrigger("jump");
        Invoke("SetForwardTime", 0.3f);
        Invoke("SetTurnTime", 1.2f);
    }

    void SetForwardTime()
    {
        moveForwardTime = 0.8f;

        //audio
        AudioManager.instance.PlayEvent("Play_spring_jump");
    }

    void SetTurnTime()
    {
        turnTime = Random.Range(1, 2.5f);
        int randomNotZero = Random.Range(0, 2) * 2 - 1;
        directionChangeAmount = randomNotZero + Random.Range(-0.5f, 0.5f);
    }

    void MoveForward()
    {
        rb.AddForce(forwardForce * transform.forward);
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        CancelInvoke("Jump");
        animator.SetBool("pickedUp", true);
    }

    public override void PutDown()
    {
        base.PutDown();
        InvokeRepeating("Jump", firstJumpDelay, repeatRate);
        animator.SetBool("pickedUp", false);
    }
}
