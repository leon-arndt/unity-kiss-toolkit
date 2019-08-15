using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to follow the players in the hub.
/// Transform of the camera looks at the center position between the two players.
/// The camera zooms out when the players are far away from each other.
/// </summary>
public class FollowCam : MonoBehaviour {
    [SerializeField]
    Transform player1, player2;

    float followSpeed = 0.8f;
    float transitionTime = 0.15f;
    public float standardHeight = 12;
    public float height;
	
	// Update is called once per frame
	void Update () {
        //find the center between players, calculate the distance from each other and figure out how much to compensate
        Vector3 center = player1.transform.position + 0.5f * (player2.transform.position - player1.transform.position);
        float distanceBetweenPlayers = Vector3.Distance(player1.transform.position, player2.transform.position);

        if (DialogueManager.Instance.inDialogue)
        {
            float velocity = 0.0f;
            height = Mathf.SmoothDamp(height, standardHeight / 2f, ref velocity, transitionTime);
            //height = Mathf.Lerp(height, standardHeight / 2f, 1.6f * Time.deltaTime);
        }
        else
        {
            float velocity = 0.0f;
            height = Mathf.SmoothDamp(height, standardHeight, ref velocity, transitionTime);
            //height = Mathf.Lerp(height, standardHeight, 1.6f * Time.deltaTime);
        }

        float compensateDistance = height + 0.6f * distanceBetweenPlayers;

        //lerp the x and z components for the new camera position
        float newX = Mathf.Lerp(transform.position.x, center.x, followSpeed * Time.deltaTime);
        float newZ = Mathf.Lerp(transform.position.z, center.z - compensateDistance, followSpeed * Time.deltaTime);

        transform.position = new Vector3(newX, compensateDistance, newZ);

        transform.LookAt(center);
	}
}
