using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullhorn : Ingredient {
    Vector3 targetPosition;
    public bool currentlyCharging = false;
    public int missedCharges = 0;
    public float dizzyTime = 0;
    public float timeUntilNextCharge = 3f;
    public float rotateTime;

    private const float dizzyDuration = 5f;
    private const float rotationDuration = 1f;
    private float chargeSpeed = 300f;
    private int numOfChargesBeforeDizzy = 2;
    private float timeBetweenCharges = 3f;
    private float timeUntilForcedVulnerability = 5f;
    private float timeBetweenForcedVulnerability = 10f;

    Rigidbody rb;

    Transform player1Transform, player2Transform;

    [SerializeField]
    Animator animator;

    [SerializeField]
    ParticleSystem effectPS, dangerousPS;

    // Use this for initialization
    void Start () {
        player1Transform = GameController.instance.player1.transform;
        player2Transform = GameController.instance.player2.transform;

        rb = GetComponent<Rigidbody>();

        var dangerousLoop = dangerousPS.emission;
        dangerousLoop.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        //count down the dizzy time
        if (dizzyTime > 0)
        {
            dizzyTime -= Time.deltaTime;
            animator.SetFloat("dizzyTime", dizzyTime);
            return;
        }

        //only act actively while not picked up
        if (!isPickedUp)
        {
            //finding a target
            if (!currentlyCharging)
            {
                //count down time until next charge and rotate time
                if (timeUntilNextCharge > 0) timeUntilNextCharge -= Time.deltaTime;
                if (rotateTime > 0) rotateTime -= Time.deltaTime;

                if (rotateTime > 0.1f)
                {
                    RotateTowardsPlayer();
                }

                //start charging
                if (timeUntilNextCharge < 0.1)
                {
                    StartRotating();
                    effectPS.Play();
                }

                //wear down slowly
                if (timeUntilForcedVulnerability > 0.1f)
                {
                    timeUntilForcedVulnerability -= Time.deltaTime;

                    if (timeUntilForcedVulnerability < 0.1f)
                    {
                        timeUntilForcedVulnerability = timeBetweenForcedVulnerability;
                        MakeDizzy();
                    }
                }
            }
            else
            {
                //charge towards out target
                rb.AddForce(chargeSpeed * transform.forward);

                //check if it hit something
                var hitInFront = Physics.Raycast(transform.position, transform.forward, 2f);
                Debug.DrawRay(transform.position, transform.forward);

                if (hitInFront)
                {
                    currentlyCharging = false;
                }
            }
        }
    }

    private void StartRotating()
    {
        rotateTime = rotationDuration;
        Invoke("StartCharging", rotationDuration);
        animator.SetTrigger("attack");
    }        

    private void StartCharging()
    {
        currentlyCharging = true;
    }

    public override void Interact(Transform newParent)
    {
        if (dizzyTime > 0f)
        {
            Pickup(newParent);
        }
        else
        {
            animator.SetTrigger("defend");
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        if (currentlyCharging)
        {
            //add dizzy time if the other object is a player
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                animator.SetTrigger("hit");
                collision.gameObject.GetComponent<PlayerController>().AddDizzyTime(2f);
            }
            else
            {
                animator.SetTrigger("hitWall");
            }

            currentlyCharging = false;
            timeUntilNextCharge = timeBetweenCharges;
            rotateTime = timeUntilNextCharge;
            missedCharges++;

            //make dizzy after desired number of charges
            if (missedCharges >= numOfChargesBeforeDizzy)
            {
                MakeDizzy();
            }
        }
        else
        {
            if (dizzyTime < 0.1 && !isPickedUp)
            {
                //still hurt players while stationary
                if (collision.gameObject.GetComponent<PlayerController>() != null)
                {
                    collision.gameObject.GetComponent<PlayerController>().AddDizzyTime(1f);
                }
            }
        }
    }

    void MakeDizzy()
    {
        missedCharges = 0;
        ParticleManager.Instance.CreateStunParticle(transform);
        dizzyTime = dizzyDuration;

        var dangerousLoop = dangerousPS.emission;
        dangerousLoop.enabled = false;

        interactable = true;

        //audio
        AudioManager.instance.PlayEvent("Play_dizzy");

        Invoke("CalmNerves", dizzyDuration);
    }

    void CalmNerves()
    {
        interactable = false;
        uiHint.DestroySelf();
        PlayerController.RemoveFromBothInteractableLists(gameObject);
        var dangerousLoop = dangerousPS.emission;
        dangerousLoop.enabled = true;
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
}
