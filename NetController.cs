using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the controller for the Slotmachine.
/// The slotmachine was originally referred to as net in design documents.
/// </summary>
public class NetController : Interactable {
    //setting up singleton
    public static NetController Instance = null;

    [SerializeField]
    GameObject[] slotMachinePrefabs;

    [SerializeField]
    GameObject surpriseParent, coalPrefab;

    [SerializeField]
    Transform[] netSpawns;

    [SerializeField]
    Transform[] displayTransforms;

    [SerializeField]
    private ParticleSystem sparksPS, sparksIntensePS, sparksLoopPS;

    [SerializeField]
    private float spawnForwardThrust, spawnForwardVariance;

    [SerializeField]
    private Animator animator;



    private int stateIndex = 0;
    private bool isShaking = false;
    private int ingredientsAlreadyCreated = 0;
    private bool shouldTurnDisplay = false;
    private int ingredientsToCreateAtOnce = 3;
    private const float displayTurnSpeed = 10f;

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

    private void Start()
    {
        slotMachinePrefabs = GameManager.instance.activeLevel.slotMachinePrefabs;

        //only create one ingredient at once in the tutorial
        if (GameManager.instance.activeLevel.showTutorial)
        {
            ingredientsToCreateAtOnce = 1;
        }

        //vfx particle systems
        var emission = sparksPS.emission;
        emission.enabled = false;
        var emission2 = sparksIntensePS.emission;
        emission2.enabled = false;
        var emissionLoop = sparksLoopPS.emission;
        emissionLoop.enabled = false;
    }

    private void Update()
    {
        if (shouldTurnDisplay)
        {
            foreach (var item in displayTransforms)
            {
                item.Rotate(displayTurnSpeed * Vector3.right);
            }
        }

        if (stateIndex > 0)
        {
            //find the closest player
            Transform closestPlayer = (Vector3.Distance(transform.position, GameController.instance.player1.transform.position) < Vector3.Distance(transform.position, GameController.instance.player2.transform.position)) ? GameController.instance.player1.transform : GameController.instance.player2.transform;

            //reset the slot machine when both players walk away
            if (Vector3.Distance(transform.position, closestPlayer.position) > 3)
            {
                stateIndex = 0;
                var emissionLoop = sparksLoopPS.emission;
                emissionLoop.enabled = false;
            }
        }

    }

    public override void Interact(Transform playerTransform)
    {
        //test these two bools first to prevent spamming
        if (!shouldTurnDisplay && !isShaking)
        {
            stateIndex = 0;
            var emissionLoop = sparksLoopPS.emission;
            emissionLoop.enabled = true;

            shouldTurnDisplay = true;

            animator.SetTrigger("interact");
            Debug.Log(animator.name);
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0));

            Invoke("Activate", 1);
        }
    }

    void Activate()
    {
        EventManager.ThrowSlotMachineEvent();
        AudioManager.instance.PlayEvent("Play_slot_machine_turn_on");

        var emissionLoop = sparksLoopPS.emission;
        emissionLoop.enabled = false;

        if (!isShaking)
        {
            StartCoroutine(Shake());
            StartCoroutine(CreateNetLootItemsSequenced());
        }
    }

    //Spawn ingredients
    public void CreateNetLoot(int spawnIndex)
    {
        shouldTurnDisplay = false;

        GameObject lootToSpawn = slotMachinePrefabs[ingredientsAlreadyCreated % slotMachinePrefabs.Length];

        var loot = Instantiate(
               lootToSpawn,
               netSpawns[spawnIndex].transform.position,
               Quaternion.identity, surpriseParent.transform);

        // Add velocity to the ingredient
        loot.GetComponent<Rigidbody>().AddForce((spawnForwardThrust + Random.Range(-spawnForwardVariance, spawnForwardVariance)) * netSpawns[spawnIndex].transform.forward);
        // + 400f * Vector3.up + Random.Range(-100, 100) * netSpawns[spawnIndex].transform.right

        ingredientsAlreadyCreated++;

        //show plant tutorial for new plants
        if (lootToSpawn.GetComponent<Ingredient>() != null)
        {
            if (UIPlantTutorial.ShouldShowPlantCard(lootToSpawn.GetComponent<Ingredient>().ingredientData))
            {
                UIPlantTutorial.Instance.ShowPlantTutorial(lootToSpawn.GetComponent<Ingredient>().ingredientData);
            }
        }
    }

    public void SpawnCoalNext()
    {
        slotMachinePrefabs[ingredientsAlreadyCreated] = coalPrefab;
    }

    IEnumerator Shake()
    {
        isShaking = true;

        for (float f = 1f; f >= -1.1; f -= 0.1f)
        {
            //transform.Translate(0.1f * Vector3.up * f);
            //outputBox.transform.Translate(-0.1f * outputBox.transform.forward * f);
            //topPipe.transform.Translate(0.05f * outputBox.transform.right * f);
            yield return null;
        }

        isShaking = false;
    }


    IEnumerator DelayInteract(Transform callingTransform, float delay)
    {
        yield return new WaitForSeconds(delay);
        Interact(callingTransform);
        yield return null;
    }
    IEnumerator CreateNetLootItemsSequenced()
    {
        for (int i = 0; i < ingredientsToCreateAtOnce; i++)
        {
            CreateNetLoot(i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void InteractWithDelay(Transform callingTransform, float delay)
    {
        StartCoroutine(DelayInteract(callingTransform, delay));
    }
}
