using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The horn flower is a coding hybrid of the Tesla and the Magmaryl ingredient
public class HornFlower : Ingredient {
    [SerializeField]
    GameObject soundWavePrefab;

    [SerializeField]
    Animator animator;

    Transform player1Transform, player2Transform;

    [SerializeField]
    private float timeUntilAttack;

    [SerializeField]
    private float anticipationDelay;

    private float startTimeUntilAttack;

    private void Start()
    {
        player1Transform = GameController.instance.player1.transform;
        player2Transform = GameController.instance.player2.transform;

        startTimeUntilAttack = timeUntilAttack;

        //compensate for the slow animation
        animator.speed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (sleepTime > 0)
        {
            sleepTime -= Time.deltaTime;
        }

        //on floor and ready to attack
        if (!isPickedUp && sleepTime < 0.1)
        {
            if (timeUntilAttack > 0)
            {
                timeUntilAttack -= Time.deltaTime;
            }

            //play the attack anticipation before the actual attack
            if (timeUntilAttack < anticipationDelay)
            {
                animator.SetTrigger("attack");
            }

            if (timeUntilAttack < 0.1f)
            {
                Attack();
            }

            TurnTowardsPlayer();
        }
    }

    void Attack()
    {
        timeUntilAttack = startTimeUntilAttack;

        CreateSoundWave();
        animator.ResetTrigger("attack");
    }


    void CreateSoundWave()
    {
        var fire = Instantiate(
        soundWavePrefab,
        gameObject.transform.position + transform.forward,
        gameObject.transform.rotation, SurpriseManager.Instance.GetSurpriseParentTransform());

        //audio
        AudioManager.instance.PlayEvent("Play_plant_horn_attack");
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        animator.SetBool("pickedUp", true);
    }

    public override void ReverToState()
    {
        base.ReverToState();
        animator.SetBool("pickedUp", false);
    }

    private void TurnTowardsPlayer() { 
        //find the closest player
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;

        Vector3 playerPositionIgnoreY = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);


        //transform.LookAt(playerPositionIgnoreY);
        Vector3 targetDir = playerPositionIgnoreY - transform.position;

        // The step size is equal to speed times frame time.
        float step = 1.6f * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our rotation a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
