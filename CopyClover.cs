using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyClover : Ingredient {
    PlayerController playerController1, playerController2;
    Transform player1Transform, player2Transform;

    // Use this for initialization
    void Start () {
        player1Transform = GameController.instance.player1.transform;
        player2Transform = GameController.instance.player2.transform;

        playerController1 = GameController.instance.player1;
        playerController2 = GameController.instance.player2;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isPickedUp)
        {
            //find the closest player
            Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;

            Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);

            transform.LookAt(playerPositionIgnoreY);

            if (Vector3.Distance(transform.position, closestPlayer.position) < 10f)
            {
                Vector3 playerMovement = player1Transform.GetComponent<Rigidbody>().velocity + player2Transform.GetComponent<Rigidbody>().velocity;
                //GetComponent<Rigidbody>().AddForce(100f * playerMovement);
                transform.position += -1.5f * playerMovement * ingredientData.moveSpeed * Time.deltaTime;
            }
        }
    }
}
