using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable object which dispenses coal when used.
/// </summary>
public class CoalDispenser : Interactable {
    [SerializeField]
    Transform coalSpawn;

    [SerializeField]
    GameObject coalPrefab;

    [SerializeField]
    float spawnForwardThrust, spawnForwardVariance, spawnRotationVariance;

    private Transform surpriseParentTransform;

    private void Start()
    {
        surpriseParentTransform = SurpriseManager.Instance.GetSurpriseParentTransform();
    }

    public override void Interact(Transform transform)
    {
        Dispense();
    }

    //Spawn coal
    void Dispense () {
        var newCoal = Instantiate(
                coalPrefab,
                coalSpawn.position,
                Quaternion.identity, surpriseParentTransform);

        // Add velocity to the coal
        newCoal.GetComponent<Rigidbody>().AddForce((spawnForwardThrust + Random.Range(-spawnForwardVariance, spawnForwardVariance)) * coalSpawn.transform.forward);
        newCoal.GetComponent<Rigidbody>().AddForce(0, 0, Random.Range(-spawnRotationVariance, spawnRotationVariance));
    }
}
