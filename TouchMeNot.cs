using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMeNot : Ingredient
{
    Transform p1Transform, p2Transform;
    Rigidbody rb;

    float timeBetweenShivers = 5f;
    int turnDirection = 1;
    float timeUntilShiver;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        timeUntilShiver = timeBetweenShivers;
        p1Transform = GameController.instance.player1.transform;
        p2Transform = GameController.instance.player2.transform;
    }

    // Update is called once per frame
    void Update()
    {
        sleepTime -= Time.deltaTime;

        if (!isPickedUp && sleepTime < 0)
        {
            //Shiver once in a while
            if (timeUntilShiver > 0f)
            {
                timeUntilShiver -= Time.deltaTime;
                if (timeUntilShiver < 0.1f)
                {
                    Shiver();
                }
            }

            //find the vector to the closest player
            Transform closestPlayer = (Vector3.Distance(transform.position, p1Transform.position) < Vector3.Distance(transform.position, p2Transform.position)) ? p1Transform : p2Transform;

            Vector3 desiredDirection = transform.position - closestPlayer.position;

            //see if it is okay to move there
            if (!Physics.Raycast(transform.position, transform.TransformDirection(desiredDirection), 3f) && desiredDirection.magnitude < 5)
            {
                rb.AddForce(20f * desiredDirection.normalized);
                transform.LookAt(transform.position + new Vector3(desiredDirection.x, 0f, desiredDirection.z));
            }
            else
            {
                //if not turn in the turn direction
                transform.Rotate(turnDirection * 2f * Vector3.up, Space.World);
                rb.AddForce(20f * transform.forward);
            }
        }
    }

    private void Shiver()
    {
        timeUntilShiver = timeBetweenShivers;
        sleepTime = 1f;
    }

    private void FlipDirection()
    {
        turnDirection *= -1;
    }
}