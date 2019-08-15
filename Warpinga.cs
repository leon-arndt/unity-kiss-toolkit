using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warpinga : Ingredient {
    public Vector3[] warpOptions;

    public float timeUntilNextWarp;
    public float timeBetweenWarps;
    public int warpCount;

    [SerializeField]
    Animator animator;


	// Use this for initialization
	void Start () {
        timeUntilNextWarp = timeBetweenWarps;
	}
	
	// Update is called once per frame
	void Update () {
        if (isPickedUp) return;

        if (timeUntilNextWarp > 0) timeUntilNextWarp -= Time.deltaTime;

        if (timeUntilNextWarp < 0.1f)
        {
            timeUntilNextWarp = timeBetweenWarps;
            Warp();
        }

        if (warpCount < 5)
        {
            //also warp away if the player is too close
            Transform closestPlayer = (Vector3.Distance(transform.position, GameController.instance.player1.transform.position) < Vector3.Distance(transform.position, GameController.instance.player2.transform.position)) ? GameController.instance.player1.transform : GameController.instance.player2.transform;

            if (Vector3.Distance(transform.position, closestPlayer.position) < 1)
            {
                Warp();
            }
        }

        //animations
        animator.SetFloat("timeUntilNextWarp", timeUntilNextWarp);
    }

    void Warp()
    {
        //animations
        animator.SetTrigger("teleport");

        //audio
        AudioManager.instance.PlayEvent("Play_plant_warp");

        //increase warp count
        warpCount++;

        //find other warpinga
        GameObject[] otherWarpingas;
        otherWarpingas = GameObject.FindGameObjectsWithTag("Warpinga");
        Transform closestWarpinga;

        if (otherWarpingas.Length > 1)
        {
            Transform[] otherTransforms = ConvertToTransformArray(otherWarpingas);
            closestWarpinga = GetClosestWarpinga(otherTransforms);
            Debug.Log(closestWarpinga.position);
        }
        else
        {
            GameObject emptyGO = new GameObject();
            emptyGO.transform.position = new Vector3(1000, 1000, 1000);
            closestWarpinga = emptyGO.transform;
            Debug.Log("no other warpingas found");
        }

        Vector3 potentialPosition = warpOptions[Random.Range(0, warpOptions.Length)];
        
        //find the closest player to potential position
        Transform closestPlayer = (Vector3.Distance(potentialPosition, GameController.instance.player1.transform.position) < Vector3.Distance(transform.position, GameController.instance.player2.transform.position)) ? GameController.instance.player1.transform : GameController.instance.player2.transform;

        while ((Vector3.Distance(potentialPosition, closestPlayer.position) < 2f) //closest player
             || Vector3.Distance(potentialPosition, transform.position) < 2f //not too close to self
             || Vector3.Distance(potentialPosition, closestWarpinga.position) < 2f) //other warpingas
        {
            //find a new potential position
            potentialPosition = warpOptions[Random.Range(0, warpOptions.Length)];
            closestPlayer = (Vector3.Distance(potentialPosition, GameController.instance.player1.transform.position) < Vector3.Distance(transform.position, GameController.instance.player2.transform.position)) ? GameController.instance.player1.transform : GameController.instance.player2.transform;
        }

        //potentialPositon must be suitable
        transform.position = potentialPosition;
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        animator.SetBool("pickedUp", true);
    }

    public override void PutDown()
    {
        base.PutDown();
        animator.SetBool("pickedUp", false);
    }

    public override void Throw()
    {
        base.Throw();
        animator.SetBool("pickedUp", false);
    }

    Transform GetClosestWarpinga(Transform[] warpingas)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in warpingas)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist && t.position != currentPos)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    Transform[] ConvertToTransformArray(GameObject[] gameObjects)
    {
        Transform[] transforms = new Transform[gameObjects.Length];

        for (int i = 0; i < gameObjects.Length; i++)
        {
            transforms[i] = gameObjects[i].transform;
        }

        return transforms;
    }
}
