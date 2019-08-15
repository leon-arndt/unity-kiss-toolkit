using System; //needed for actions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Breakable
{
    [SerializeField]
    bool broken = false;

    [SerializeField]
    float onBreakDamage = 10f;

    [SerializeField]
    float damagePerSecond = 1.5f;

    float protectedTime = 0f;

    [SerializeField]
    float breakRotationAmount = 30f;

    [SerializeField]
    Vector3 breakRotation;
    
    [SerializeField]
    ParticleSystem brakingPS, brokenPS;

    [SerializeField]
    QuickTimeEvent qte;

    private MeshFilter meshFilter;

    [SerializeField]
    Mesh normalMesh, brokenMesh;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update () {
        protectedTime -= Time.deltaTime;

        if (broken)
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

            Break();
        }
        else
        {
            //only players may fix the pipe
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
            //the pipe breaks
            //transform.Rotate(breakRotation);
            broken = true;
            GetComponent<Collider>().enabled = true;
            VehicleController.Instance.SlowDown();
            VehicleController.Instance.AddDamage(onBreakDamage);
            damageCaused = onBreakDamage;

            CameraShake.shakeDuration = 0.3f;

            //particle effect
            brakingPS.gameObject.SetActive(true);
            brokenPS.gameObject.SetActive(true);

            //replace mesh
            meshFilter.mesh = brokenMesh;

            //audio
            AudioManager.instance.PlayEvent("Play_pipe_break");
            AudioManager.instance.PlayEvent("Play_pipe_break_impact");

            //remind after 2 seconds to fix
            Invoke("RemindFix", 2f);
        }
    }

    private void StartQTE(Transform callingTransform)
    {
        Action actionWhenQTECompleted = Fix;
        qte.StartQTE(callingTransform, actionWhenQTECompleted);
    }


    public override void Fix()
    {
        EventManager.ThrowPipeFixedEvent();

        //fix the pipe
        //transform.Rotate(-breakRotation);
        broken = false;
        GetComponent<Collider>().enabled = true;
        VehicleController.Instance.SpeedUp();

        //repair vehicle damage
        VehicleController.Instance.RemoveDamage(damageCaused);
        damageCaused = 0;

        protectedTime = 10f;

        //replace mesh
        meshFilter.mesh = normalMesh;

        //particle effects
        brakingPS.gameObject.SetActive(false);
        brokenPS.gameObject.SetActive(false);

        qte.SetReminderVisibility(false);

        //audio
        AudioManager.instance.StopEvent("Play_pipe_break", 0);
    }

    private void RemindFix()
    {
        qte.SetReminderVisibility(true);
    }

    public bool IsBroken()
    {
        return broken;
    }
}
