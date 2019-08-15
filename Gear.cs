using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Breakable
{
    [SerializeField]
    bool broken = false;

    [SerializeField]
    float onBreakDamage = 5f;

    [SerializeField]
    float damagePerSecond = 2f;

    [SerializeField]
    float rotationSpeed = 1f;
    float protectedTime = 0f;

    Vector3 startPosition;
    Quaternion startRotation;

    [SerializeField]
    ParticleSystem breakingPS, brokenPS;

    [SerializeField]
    QuickTimeEvent qte;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        protectedTime -= Time.deltaTime;

        if (!broken)
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.timeScale * VehicleController.Instance.GetRatioCurrentSpeedToNormal());
        }
        else
        {
            //damage the vehicle
            VehicleController.Instance.AddDamage(damagePerSecond * Time.deltaTime);
            damageCaused += damagePerSecond * Time.deltaTime;
        }
    }

    public override void Interact(Transform callingTransform)
    {
        if (!broken)
        {
            //ignore cases
            if (protectedTime > 0) return;
            if (callingTransform.tag == "Player1" || callingTransform.tag == "Player2") return;

            //the gear breaks
            Break();
        }
        else
        {
            //only players may fix the gear
            if (callingTransform.tag != "Player1" && callingTransform.tag != "Player2") return;
            if (!qte.eventActive)
            {
                StartQTE(callingTransform);
            }
        }
    }

    public override void Break()
    {
        if (!broken)
        {
            Debug.Log("Gear has broken down");
            broken = true;
            VehicleController.Instance.SlowDown();
            VehicleController.Instance.AddDamage(onBreakDamage);
            damageCaused = onBreakDamage;

            CameraShake.shakeDuration = 0.3f;


            //particle effect
            breakingPS.gameObject.SetActive(true);
            brokenPS.gameObject.SetActive(true);

            //audio
            AudioManager.instance.PlayEvent("Play_gear_break");

            //remind after 2 seconds to fix
            Invoke("RemindFix", 2f);
        }
    }

    public override void Fix()
    {
        //fix the gear
        broken = false;
        VehicleController.Instance.SpeedUp();

        //gear can no longer be picked up
        protectedTime = 10f;

        //reset the gear
        transform.position = startPosition;
        transform.rotation = startRotation;

        //unparent if it has a parent
        if (transform.parent != null)
        {
            transform.parent = null;
        }

        //repair vehicle damage
        VehicleController.Instance.RemoveDamage(damageCaused);
        damageCaused = 0;


        //stop particle effect
        breakingPS.gameObject.SetActive(false);
        brokenPS.gameObject.SetActive(false);

        qte.SetReminderVisibility(false);
    }

    private void RemindFix()
    {
        qte.SetReminderVisibility(true);
    }

    public bool IsBroken()
    {
        return broken;
    }

    private void StartQTE(Transform callingTransform)
    {
        System.Action actionWhenQTECompleted = Fix;
        qte.StartQTE(callingTransform, actionWhenQTECompleted);
    }
}