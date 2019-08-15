using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magmaryl : Ingredient {
    Vector3 targetPosition;

    [SerializeField]
    GameObject fireBeamPrefab;

    Transform player1Transform, player2Transform;

    [SerializeField]
    Animator animator;

    float timeUntilAttack;
    float startAtackDelay = 1;
    float timeBetweenAttacks = 8f;
    float shootTime;

    // Use this for initialization
    void Start () {
        player1Transform = GameController.instance.player1.transform;
        player2Transform = GameController.instance.player2.transform;

        timeUntilAttack = startAtackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (sleepTime > 0)
        {
            sleepTime -= Time.deltaTime;
        }

        if (!isPickedUp && sleepTime < 0.1)
        {
            //attacking
            if (timeUntilAttack > 0)
            {
                timeUntilAttack -= Time.deltaTime;
            }
            else
            {
                Attack();
            }


            //shooting animations
            if (shootTime > 0)
            {
                shootTime -= Time.deltaTime;
                animator.SetFloat("shootTime", shootTime);
            }
            

            //turn at look at the player
            if (shootTime < 0.1)
            {
                //find the closest player
                Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;

                Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);


                //transform.LookAt(playerPositionIgnoreY);
                Vector3 targetDir = playerPositionIgnoreY - transform.position;

                // The step size is equal to speed times frame time.
                float step = 0.8f * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                Debug.DrawRay(transform.position, newDir, Color.red);

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }

    void Attack()
    {
        timeUntilAttack = timeBetweenAttacks;
        animator.SetTrigger("startAttack");
        Invoke("CreateFireBeam", 1f);
    }


    void CreateFireBeam()
    {
        var fire = Instantiate(
        fireBeamPrefab,
        gameObject.transform.position,
        gameObject.transform.rotation, SurpriseManager.Instance.GetSurpriseParentTransform());

        shootTime = 2f;

        AudioManager.instance.PlayEvent("Play_plant_fire_attack");
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        animator.SetBool("pickedUp", true);
    }

    public override void PutDown()
    {
        base.PutDown();
        timeUntilAttack = startAtackDelay;

        animator.SetBool("pickedUp", false);
    }

    public override void Pacify()
    {
        
    }
}
