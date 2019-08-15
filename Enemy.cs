using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float aggroDistance = 10f;
    public float moveSpeed = 5f;

    public Transform player1Transform, player2Transform;

    enum AIState { idle, chasing}
    AIState aiState;
    
    // Use this for initialization
    void Start () {
        aiState = AIState.idle;
        //Transform player1Transform = GameController.Instance.player1.transform;
        //Transform player2Transform = GameController.Instance.player2.transform;
    }

    // Update is called once per frame
    void Update () {

        //find the closest player
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;


        if (Vector3.Distance(transform.position, closestPlayer.position) < 5f)
        {
            aiState = AIState.chasing;
        }
        else
        if (Vector3.Distance(transform.position, closestPlayer.position) > 10f) //lose the player
        {
            aiState = AIState.idle;
        }

        //idle animation
        if (aiState == AIState.idle)
        {
            transform.Rotate((Mathf.Sin(Time.time) - 0.5f) * Vector3.up, Space.World);
        }



        //chase the player
        if (aiState == AIState.chasing)
        {
            transform.LookAt(closestPlayer);
            if (Vector3.Distance(transform.position, closestPlayer.position) > 1f)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            
        }

    }
}
