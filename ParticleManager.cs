using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This particle manager is used by the players as a way to handle particle creation.
/// </summary>
public class ParticleManager : MonoBehaviour {
    //setting up singleton
    public static ParticleManager Instance = null;

    [SerializeField]
    private GameObject walkParticles, stunParticles, throwParticles;

    //Singleton
    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    public void CreateWalkParticles(Transform playerTransform)
    {
        GameObject particle = Instantiate(walkParticles, playerTransform.position + 0.5f * Vector3.down, Quaternion.identity);
        particle.transform.SetParent(transform);
    }

    public void CreateStunParticle(Transform objectTransform)
    {
        GameObject particle = Instantiate(stunParticles, objectTransform.position + 0.5f * Vector3.up, Quaternion.identity);
        particle.transform.SetParent(objectTransform);
    }

    public void CreateThrowParticles(Transform objectTransform)
    {
        GameObject particle = Instantiate(throwParticles, objectTransform.position, Quaternion.identity);
        particle.transform.SetParent(objectTransform);
    }
}
