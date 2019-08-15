using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The surpries manager handles the breaking of breakable objects such as the pipes and gears.
/// </summary>
public class SurpriseManager : MonoBehaviour {
    //setting up singleton
    private static SurpriseManager instance;

    public static SurpriseManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SurpriseManager");
                instance = go.AddComponent<SurpriseManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    GameObject surpriseParent, rockPrefab;

    [SerializeField]
    Pipe pipe, highPipe;

    [SerializeField]
    Gear gear;

    [SerializeField]
    GameObject[] netLootPrefabs;

    [SerializeField]
    Breakable[] breakables;

    private int supplyCrateCount;

    // Use this for initialization
	void Start () {
        instance = GetComponent<SurpriseManager>(); //required for singleton with instance in scene
    }
 

    public void BreakPipe()
    {
        pipe.Interact(this.transform);
    }

    public void BreakHighPipe()
    {
        highPipe.Interact(this.transform);
    }

    public void BreakGear()
    {
        gear.Interact(this.transform);
    }

    //break a random breakable
    public void BreakRandom()
    {
        breakables[Random.Range(0, breakables.Length)].Interact(this.transform);
    }

    public Transform GetSurpriseParentTransform()
    {
        return surpriseParent.transform;
    }
}
