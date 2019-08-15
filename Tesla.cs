using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : Ingredient {
    public float timeUntilAttack;
    private float startTimeUntilAttack;
    public float attackRadius;
    public float anticipationDelay = 1f;
    private float startAnticipationDelay;
    private float maxLightRange = 3.14f;
    public float timeRate = 1;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private ParticleSystem explosionPS;

    [SerializeField]
    private Light effectLight;

    private void Start()
    {
        startTimeUntilAttack = timeUntilAttack;
        startAnticipationDelay = anticipationDelay;
        effectLight.range = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeUntilAttack > 0)
        {
            timeUntilAttack -= timeRate * Time.deltaTime;
        }

        if (timeUntilAttack < 0.1f)
        {
            anticipationDelay -= timeRate * Time.deltaTime;
        }

        if (anticipationDelay < 0.1f)
        {
            Attack();
        }

        animator.SetFloat("timeUntilAttack", timeUntilAttack);
        animator.SetFloat("anticipationDelay", anticipationDelay);
    }

    void Attack()
    {
        //audio
        AudioManager.instance.PlayEvent("Play_plant_tesla");

        CameraShake.shakeDuration = 0.5f;
        explosionPS.Play();

        BreakNearbyBreakables();
        HurtNearbyPlayer(GameController.instance.player1);
        HurtNearbyPlayer(GameController.instance.player2);

        timeUntilAttack = 3f;
        anticipationDelay = startAnticipationDelay;

        StartCoroutine(FadeLightOn());
    }

    void HurtNearbyPlayer(PlayerController playerController)
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) < attackRadius)
        {
            playerController.AddDizzyTime(2);
        }
    }

    void BreakNearbyBreakables()
    {
        //could be optimized, finding objects this way is slow
        Breakable[] breakableList = FindObjectsOfType<Breakable>();

        foreach (var breakable in breakableList)
        {
            if (Vector3.Distance(transform.position, breakable.transform.position) < attackRadius)
            {
                breakable.Break();
            }
        }
    }

    //pause timer when picked up
    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        timeRate = 0f;
        animator.SetBool("pickedUp", true);
    }


    public override void ReverToState()
    {
        base.ReverToState();
        animator.SetBool("pickedUp", false);
        timeRate = 1f;
    }

    IEnumerator FadeLightOn()
    {
        for (float f = 0f; f <= 1; f += 0.1f)
        {
            effectLight.range = f * maxLightRange;
            yield return null;
        }
        StartCoroutine(FadeLightOff());
    }

    IEnumerator FadeLightOff()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            effectLight.range = f * maxLightRange;
            yield return null;
        }
    }
}
