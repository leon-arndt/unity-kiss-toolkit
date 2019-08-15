using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burdock : Ingredient {
    public float startHealth = 30f;
    public float health;
    public bool clinging = false;

    Transform p1Transform, p2Transform;

    Rigidbody rb;

    Vector3 previousWorldPosition;

    PlayerController host;

    // Use this for initialization
    void Start () {
        health = startHealth;

        p1Transform = GameController.instance.player1.transform;
        p2Transform = GameController.instance.player2.transform;

        previousWorldPosition = transform.position;

        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isPickedUp) return;

        if (sleepTime >= 0)
        {
            sleepTime -= Time.deltaTime;
            MoveAway();
        }

        if (!clinging && sleepTime < 0.1)
        {
            //find the closest player
            Transform closestPlayer = (Vector3.Distance(transform.position, p1Transform.position) < Vector3.Distance(transform.position, p2Transform.position)) ? p1Transform : p2Transform;

            Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);

            transform.LookAt(playerPositionIgnoreY);

            if (Vector3.Distance(transform.position, closestPlayer.position) < 5f)
            {
                transform.position += transform.forward * ingredientData.moveSpeed * Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        if (clinging)
        {
            float amountMoved;

            amountMoved = Vector3.Distance(previousWorldPosition, transform.position);

            health -= amountMoved;

            previousWorldPosition = transform.position;
        }

        if (health <= 0)
        {
            Uncling();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //only check for victims if not picked up
        if (!isPickedUp)
        {
            //check if the collider is a player
            if (other.GetComponent<PlayerController>() != null)
            {
                //parent to the player
                transform.SetParent(other.transform);


                //injure player
                host = other.GetComponent<PlayerController>();
                host.slowedEffect = true;

                clinging = true;

                rb.isKinematic = true;
            }
        }
    }

    private void Uncling()
    {
        sleepTime = 2f;
        health = startHealth;

        clinging = false;
        transform.SetParent(null);

        rb.isKinematic = false;

        if (host != null)
        {
            host.slowedEffect = false;
            host = null;
        }
    }

    private void MoveAway()
    {
        //find the closest player and move away
        Transform closestPlayer = (Vector3.Distance(transform.position, p1Transform.position) < Vector3.Distance(transform.position, p2Transform.position)) ? p1Transform : p2Transform;

        Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);

        transform.LookAt(playerPositionIgnoreY);
        transform.Rotate(new Vector3(0, 180, 0)); //turn around

        if (Vector3.Distance(transform.position, closestPlayer.position) < 2f)
        {
            transform.position += transform.forward * ingredientData.moveSpeed * Time.deltaTime;
        }
    }

    public override void Pickup(Transform newParent)
    {
        if (clinging)
        {
            Uncling();
        }
        base.Pickup(newParent);
    }
}
