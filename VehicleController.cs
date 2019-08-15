using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {
    //setting up singleton
    public static VehicleController Instance = null;

    static float vehicleStartSpeed = 12f; // m per second
    public const float DANGER_THRESHOLD = 60f;
    public float vehicleSpeed = 0;
    public float startDistanceFromGoal; //in meters
    public float distanceFromGoal; 
    public float temperature;
    public float damage = 0f;
    public float totalDamageSustained = 0f;
    public bool moving = false;
    public bool movingOffscreen;
    public float damageScaling;
    private float damageScalingPreSettings;

    public GameObject geometryGroup;

    private float startTime;
    private float maxTemperature = 100f;
    private float minTemperature = 20f;
    private float timeBetweenWarningSounds = 1f;
    private float timeSinceLastWarningSound = 0f;

    //Singleton
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

    // Use this for initialization
    void Start () {
        distanceFromGoal = startDistanceFromGoal;
        damageScaling = GameManager.instance.activeLevel.vehicleDamageMultiplier;
        damageScalingPreSettings = damageScaling;
        UpdateDamageScaling();
    }
	
	// Update is called once per frame
	void Update () {
        if (movingOffscreen)
        {
            float amount = 0.1f;
            geometryGroup.transform.Translate(amount * Vector3.right);
            GameController.instance.player1.transform.Translate(amount * Vector3.left);
            GameController.instance.player2.transform.Translate(amount * Vector3.left);
            GameController.instance.player1.GetComponent<Rigidbody>().AddForce(100f * Vector3.right);
            GameController.instance.player2.GetComponent<Rigidbody>().AddForce(100f * Vector3.right);
        }

        if (moving)
        {
            //speed up at the beginning of the game
            if (Time.time < startTime + 7)
            {
                vehicleSpeed = Mathf.Lerp(vehicleSpeed, vehicleStartSpeed, 0.001f * (Time.time - startTime));
            }

            //move towards the goal
            if (distanceFromGoal > 0.1)
            {
                distanceFromGoal -= Time.deltaTime * vehicleSpeed;
            }

            //game over if the train takes too much damage
            if (damage >= 100)
            {
                GameController.instance.GameOver();
                damageScaling = 0f;
            }
            else
            {
                //warning audio
                if (damage > DANGER_THRESHOLD)
                {
                    timeSinceLastWarningSound += Time.deltaTime;
                    
                    if (timeSinceLastWarningSound > timeBetweenWarningSounds)
                    {
                        AudioManager.instance.PlayEvent("Play_ui_train_danger");
                        timeSinceLastWarningSound = 0f;
                    }
                }
            }

            //cool down the temp
            if (temperature > 20) {
                temperature -= 0.4f * Time.deltaTime;
            }
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hazard>() != null)
        {
            Debug.Log("Started Camera shake");
            CameraShake.shakeDuration = 0.1f;
        }
    }

    public void StartMoving()
    {
        moving = true;
        startTime = Time.time;
    }

    public void SlowDown()
    {
        vehicleSpeed *= 0.8f;
    }

    public void SpeedUp()
    {
        vehicleSpeed += 0.20f * vehicleSpeed + Random.Range(0, 2);
    }

    public void IncreaseTemperature(float f)
    {
        temperature = Mathf.Min(maxTemperature, temperature + f);
    }

    public void DecreaseTemperature(float f)
    {
        temperature = Mathf.Max(minTemperature, temperature - f);
    }

    public void StopCompletly()
    {
        vehicleSpeed = 0;
    }

    public void ReturnToNormalSpeed()
    {
        vehicleSpeed = vehicleStartSpeed;
    }

    public float GetSpeed()
    {
        return vehicleSpeed;
    }

    public float GetRatioCurrentSpeedToNormal()
    {
        return vehicleSpeed / vehicleStartSpeed;
    }

    public float GetTemperatureRatio()
    {
        return temperature / maxTemperature;
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public float GetDistanceFromGoal()
    {
        return distanceFromGoal;
    }

    public float GetDistanceProgressRatio()
    {
        return (startDistanceFromGoal - distanceFromGoal) / startDistanceFromGoal;
    }

    public void AddDamage(float f)
    {
        float damageToAdd = f * damageScaling;

        damage += damageToAdd;
        totalDamageSustained += damageToAdd;
    }

    public void RemoveDamage(float f)
    {
        //remove damage but never remove too much
        damage = Mathf.Max(0, damage - damageScaling * f);
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetDamageRatio()
    {
        return damage / 100f;
    }

    public void StartMovingOffscreen()
    {
        GetComponent<Collider>().enabled = false;
        movingOffscreen = true;
    }

    public void UpdateDamageScaling(float f)
    {
        damageScaling = damageScalingPreSettings * f;
    }

    public void UpdateDamageScaling()
    {
        float factor = 0.5f + 0.5f * SettingsManager.Instance.DifficultyFactor;
        damageScaling = damageScalingPreSettings * factor;
    }

}
