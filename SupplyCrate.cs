using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCrate : PickUp {
    //public bool isPickedUp = false;
    public float moveSpeed = 3f;
    public float sleepTime = 1f;
    Transform player1Transform, player2Transform;

    

    // Use this for initialization
    void Start () {
        PlayerController[] allPlayers = FindObjectsOfType<PlayerController>();

        player1Transform = allPlayers[0].transform;
        player2Transform = allPlayers[1].transform;
    }
	
	// Update is called once per frame
	void Update () {
        sleepTime -= Time.deltaTime;

        if (!isPickedUp && sleepTime < 0)
        {
            //find the closest player
            Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;

            Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);

            transform.LookAt(playerPositionIgnoreY);
            transform.Rotate(new Vector3(0, 180, 0)); //turn around

            if (Vector3.Distance(transform.position, closestPlayer.position) < 5f)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

            //jump sometimes
            if (Random.Range(0, 1000) > 990)
            {
                Debug.Log("SupplyCrate jumped");
                //transform.position += 0.1f * Vector3.up;
                if (GetComponent<Rigidbody>().velocity.magnitude < 1f)
                {
                    GetComponent<Rigidbody>().AddForce(300f * Vector3.up);
                }
            }
        }
    }

    public override void Pickup(Transform newParent)
    {
        isPickedUp = true;

        base.Pickup(newParent);
    }

    public override void PutDown()
    {
        isPickedUp = false;

        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(null);
        GetComponent<BoxCollider>().enabled = true;
    }

    public override void Interact(Transform newParent)
    {
        //base.Interact();
        Pickup(newParent);
    }

    public override void Throw()
    {
        isPickedUp = false;

        base.Throw();
        GetComponent<BoxCollider>().enabled = true;
    }
}
