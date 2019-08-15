using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {
    public static WeatherManager instance = null;  //Static instance of GameManager which allows it to be accessed by any other script.

    //weather
    public enum Weather { Sunny, Rocky, Rainy, Icy, Heat };
    public Weather currentWeather;
    public Weather[] weatherPool;

    [SerializeField]
    GameObject iceGO, heatHazardGO, rockPrefab, surpriseParent;

    [SerializeField]
    WeatherData sunnyData, rockyData, rainyData, icyData, heatData;

    [SerializeField]
    Transform rockSpawn;

    [SerializeField]
    ParticleSystem rainPS, snowPS, heatPS, heatDangerPS;

    [SerializeField]
    Light sunLight, heatDangerLight;

    [SerializeField]
    UIWeatherDial weatherDial;

    [SerializeField]
    private Vector3[] possibleFirePositions;
    private Vector3 currentFirePosition;

    public float timeLeftForCurrentWeather;
    public int weatherPoolIndex = 0;

    private float standardSunIntensity;
    private float standardAmbientIntensity;
    private float standardFireDangerIntensity;

    private float sunIntensityTarget;
    private float ambientIntensityTarget;
    private float fireDangerIntensityTarget;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        //get the weather pool from the active level data
        weatherPool = GameManager.instance.activeLevel.weather;

        ClearWeather();
        currentWeather = Weather.Sunny;

        UIController.Instance.UpdateWeatherIcon(currentWeather);

        var emission = rainPS.emission;
        emission.enabled = false;

        standardSunIntensity = sunLight.intensity;
        standardFireDangerIntensity = 18.42f;
    }

    private void Update()
    {
        if (VehicleController.Instance.moving)
        {
            timeLeftForCurrentWeather -= Time.deltaTime;

            //update lighting
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, sunIntensityTarget, Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, ambientIntensityTarget, Time.deltaTime);
            heatDangerLight.intensity = Mathf.Lerp(heatDangerLight.intensity, fireDangerIntensityTarget, Time.deltaTime);

            //Change the weather when time runs out for current
            if (timeLeftForCurrentWeather < 0)
            {
                Weather newWeather = weatherPool[weatherPoolIndex % weatherPool.Length];
                ChangeWeather(newWeather);

                weatherPoolIndex++;
                timeLeftForCurrentWeather = 12f;
            }

            //affect pot temperature
            if (currentWeather == Weather.Icy || currentWeather == Weather.Rainy)
            {
                VehicleController.Instance.DecreaseTemperature(0.02f);
            }
            else
            if (currentWeather == Weather.Heat)
            {
                VehicleController.Instance.IncreaseTemperature(0.04f);
            }
        }
    }

    public void ChangeWeather(Weather newWeather)
    {
        switch (newWeather)
        {
            case Weather.Rocky:
                CreateRocks();
                break;
            case Weather.Heat:
                StartHeatwave();
                break;
            case Weather.Rainy:
                StartRaining();
                break;
            case Weather.Icy:
                MakeWeatherIcy();
                break;
            case Weather.Sunny:
                MakeWeatherSunny();
                break;
        }
    }

    public void ClearWeather()
    {
        var emission = rainPS.emission;
        emission.enabled = false;

        var snowEmission = snowPS.emission;
        snowEmission.enabled = false;

        var heatEmission = heatPS.emission;
        heatEmission.enabled = false;

        var dangerEmission = heatDangerPS.emission;
        dangerEmission.enabled = false;

        iceGO.SetActive(false);
        Ice.ResetAllPlayersDrag();

        heatHazardGO.SetActive(false);

        fireDangerIntensityTarget = 0;
        heatDangerLight.enabled = false;
    }

    public void CreateRocks()
    {
        GameController.instance.timeProtectedFromEvents = 5f;

        Debug.Log("Spawned rocks");

        var rocks = Instantiate(
                rockPrefab,
                rockSpawn.transform.position,
                rockSpawn.transform.rotation, surpriseParent.transform);

        ClearWeather();
        currentWeather = Weather.Rocky;
        UIController.Instance.UpdateWeatherIcon(currentWeather);
        weatherDial.SpinDialTo(rockyData.weatherDialRotation);
        UIController.Instance.ShowRockDangerPopup();
        CameraShake.Instance.ShakeWithDelay(0.6f, 2f);
        NetController.Instance.InteractWithDelay(transform, 2f);

        sunIntensityTarget = standardSunIntensity;
        ambientIntensityTarget = standardAmbientIntensity;
    }

    public void StartHeatwave()
    {
        ClearWeather();
        currentWeather = Weather.Heat;
        UIController.Instance.UpdateWeatherIcon(currentWeather);
        weatherDial.SpinDialTo(heatData.weatherDialRotation);

        sunIntensityTarget = 1.5f;
        ambientIntensityTarget = 1.5f;

        //turn on fire PS
        var heatEmission = heatPS.emission;
        heatEmission.enabled = true;

        var dangerEmission = heatDangerPS.emission;
        dangerEmission.enabled = true;

        //update fire PS position
        currentFirePosition = possibleFirePositions[Random.Range(0, possibleFirePositions.Length)];
        heatPS.transform.position = currentFirePosition;
        heatDangerPS.transform.position = currentFirePosition;
        heatHazardGO.transform.position = currentFirePosition;
        heatDangerLight.transform.position = currentFirePosition;

        Invoke("EnableFireHazard", 1f);
    }

    public void MakeWeatherSunny()
    {
        ClearWeather();
        currentWeather = Weather.Sunny;
        UIController.Instance.UpdateWeatherIcon(currentWeather);
        weatherDial.SpinDialTo(sunnyData.weatherDialRotation);

        sunIntensityTarget = standardSunIntensity;
        ambientIntensityTarget = standardAmbientIntensity;
    }

    public void MakeWeatherIcy()
    {
        ClearWeather();
        currentWeather = Weather.Icy;
        UIController.Instance.UpdateWeatherIcon(currentWeather);
        weatherDial.SpinDialTo(icyData.weatherDialRotation);

        iceGO.SetActive(true);

        var emission = snowPS.emission;
        emission.enabled = true;

        sunIntensityTarget = 0.8f;
        ambientIntensityTarget = 0.8f;
    }

    public void StartRaining()
    {
        ClearWeather();
        currentWeather = Weather.Rainy;
        weatherDial.SpinDialTo(rainyData.weatherDialRotation);
        UIController.Instance.UpdateWeatherIcon(currentWeather);

        var emission = rainPS.emission;
        emission.enabled = true;

        sunIntensityTarget = 0.1f;
        ambientIntensityTarget = 0.1f;
    }

    public void EnableFireHazard()
    {
        heatHazardGO.SetActive(true);
        fireDangerIntensityTarget = standardFireDangerIntensity;
        heatDangerLight.enabled = true;
    }
}
