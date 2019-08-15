using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Force Player and crates to respawn
/// </summary>
[RequireComponent(typeof(Collider))]
public class ForceRespawn : MonoBehaviour {
    [SerializeField]
    Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        //is the other object a player?
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().transform.position = respawnPoint.position + Random.Range(-3, 3) * Vector3.right;
        }

        //is the other object a crate?
        if (other.GetComponent<PickUp>())
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<PickUp>().transform.position = respawnPoint.position + Random.Range(-3, 3) * Vector3.right;
        }
    }
}
