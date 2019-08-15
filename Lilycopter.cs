using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lilycopter : Ingredient {
    [SerializeField]
    private Vector3[] goalPositions;
    private Vector3 currentGoal;
    private Vector3 previousGoal;

    public float delayUntilFloatUp;
    public float floatDelayAfterDrop;
    private float timeUntilFloatUp;
    private float timeUntilGoalChange;
    private float timeBeweenGoals = 3f;
    public float maxFloatHeight;
    public bool floatingUp;

    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    private float moveSpeedForce = 5f;
    private float maxSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeUntilFloatUp = delayUntilFloatUp;

        currentGoal = goalPositions[0];
        previousGoal = currentGoal;
    }

    // Update is called once per frame
    void Update () {
        //don't do anything once picked up
        if (isPickedUp) return;

        if (timeUntilFloatUp > 0)
        {
            timeUntilFloatUp -= Time.deltaTime;
        }
        else
        {
            //planar movement
            if (!isPickedUp)
            {
                // The step size is equal to speed times frame time.
                float step = moveSpeedForce * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, currentGoal, step);
            }
        }
        
        //float up
        if (timeUntilFloatUp < 0.1f && !floatingUp)
        {
            FloatUp();

            //audio
            AudioManager.instance.PlayEvent("Play_plant_helicopter");
        }

        if (floatingUp)
        {
            if (transform.position.y < maxFloatHeight)
            {
                rb.AddForce(Vector3.up);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            //change goals
            timeUntilGoalChange -= Time.deltaTime;
            if (timeUntilGoalChange < 0)
            {
                while (currentGoal == previousGoal)
                {
                    currentGoal = goalPositions[Random.Range(0, goalPositions.Length)];
                }
                previousGoal = currentGoal;
                timeUntilGoalChange = timeBeweenGoals;
            }
        }
    }

    private void FloatUp()
    {
        floatingUp = true;
        rb.useGravity = false;
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        floatingUp = false;
        animator.SetBool("pickedUp", true);

        //audio
        AudioManager.instance.StopEvent("Play_plant_helicopter", 0);
    }

    public override void PutDown()
    {
        base.PutDown();
        timeUntilFloatUp = floatDelayAfterDrop;
        animator.SetBool("pickedUp", false);
    }

    public override void Throw()
    {
        base.Throw();
        timeUntilFloatUp = floatDelayAfterDrop;
        animator.SetBool("pickedUp", false);
    }
}
