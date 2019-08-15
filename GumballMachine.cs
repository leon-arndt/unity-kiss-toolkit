using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives the player toys to play around with
/// Similar to the slot machine but located in the train station
/// </summary>
public class GumballMachine : Interactable {
    [SerializeField]
    GameObject[] toyPrefabs;

    [SerializeField]
    Transform spawnTransform;

    int toysAlreadyCreated = 0;

    public override void Interact(Transform playerTransform)
    {
        Activate();
    }

    void Activate()
    {
        CreateToy();
    }

    void CreateToy()
    {
        GameObject lootToSpawn = toyPrefabs[toysAlreadyCreated % toyPrefabs.Length];
        toysAlreadyCreated++;

        var loot = Instantiate(
               lootToSpawn,
               spawnTransform.position,
               Quaternion.identity, transform);

        // Add velocity to the ingredient
        loot.GetComponent<Rigidbody>().AddForce(300f * spawnTransform.forward + 2f * Vector3.up + Random.Range(-2, 2) * transform.right);
    }
}
