using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : PickUp {
    Rigidbody rb;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float followThreshold;
    Transform player1Transform, player2Transform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player1Transform = TrainStation.instance.player1.transform;
        player2Transform = TrainStation.instance.player2.transform;
    }

    private void Update()
    {
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;
        if (Vector3.Distance(transform.position, closestPlayer.position) < followThreshold)
        {
            MoveTowards();
        }
    }

    void RotateTowardsPlayer()
    {
        //find the closest player
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;
        Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);
        Vector3 targetDir = playerPositionIgnoreY - transform.position;

        // The step size is equal to speed times frame time.
        float step = 3f * Time.deltaTime;

        // Move our position a step closer to the target.
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveTowards()
    {
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;
        Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);
        Vector3 moveVector = playerPositionIgnoreY - transform.position;

        rb.AddForce(moveSpeed * moveVector);
    }
}
