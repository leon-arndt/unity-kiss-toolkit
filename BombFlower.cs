using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlower : Ingredient {
    public float timeUntilExplosion;
    private float startTimeUntilExplosion;
    public float explosionRadius;
    public bool hasExploded = false;
    public float timeRate = 1;

    [SerializeField]
    ParticleSystem explosionPS;

    [SerializeField]
    Animator animator;

    private void Start()
    {
        startTimeUntilExplosion = timeUntilExplosion;

        //audio
        AudioManager.instance.PlayEvent("Play_bomb_ticking");
    }

    // Update is called once per frame
    void Update () {
        if (timeUntilExplosion > 0)
        {
            timeUntilExplosion -= timeRate * Time.deltaTime;
        }

        if (timeUntilExplosion < 0.1f && !hasExploded)
        {
            Explode();
        }

        UpdateTimeLeftRatio();
	}

    void Explode()
    {
        CameraShake.shakeDuration = 0.5f;
        explosionPS.Play();
        hasExploded = true;

        BreakNearbyBreakables();
        HurtNearbyPlayer(GameController.instance.player1);
        HurtNearbyPlayer(GameController.instance.player2);

        //audio
        AudioManager.instance.PlayEvent("Play_explosion");
        //stop ticking audio
        AudioManager.instance.StopEvent("Play_bomb_ticking", 0);

        Destroy(gameObject, 0.5f);
    }

    void HurtNearbyPlayer(PlayerController playerController)
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) < explosionRadius)
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
            if (Vector3.Distance(transform.position, breakable.transform.position) < explosionRadius)
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

        //stop ticking audio
        AudioManager.instance.StopEvent("Play_bomb_ticking", 0);
    }

    //explode twice as fast
    public override void PutDown()
    {
        base.PutDown();
        IncreaseDanger();
        animator.SetBool("pickedUp", false);

        //audio
        AudioManager.instance.PlayEvent("Play_bomb_ticking");
    }

    //explode twice as fast
    public override void Throw()
    {
        base.Throw();
        IncreaseDanger();
        animator.SetBool("pickedUp", false);

        //audio
        AudioManager.instance.PlayEvent("Play_bomb_ticking");
    }

    void IncreaseDanger()
    {
        timeRate = 2f;
        timeUntilExplosion = startTimeUntilExplosion / 2f;
    }

    private void UpdateTimeLeftRatio()
    {
        animator.SetFloat("timeLeftRatio", timeUntilExplosion / startTimeUntilExplosion);
    }

    private void OnDestroy()
    {
        //remove from interactable list
        PlayerController.RemoveFromBothInteractableLists(gameObject);
        
        //stop ticking audio
        AudioManager.instance.StopEvent("Play_bomb_ticking", 0);
    }
}
