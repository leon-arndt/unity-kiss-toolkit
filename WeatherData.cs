using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is used for weather data.
/// Since this data is swapped it is not as suitable for scriptable objects and not yet implemented.
/// Will be implemented once all weather design is final.
/// </summary>
[CreateAssetMenu(fileName = "New Weather", menuName = "Weather", order = 3)]
public class WeatherData : ScriptableObject
{
    public string weatherName;
    public string weatherDescription;
    public Texture2D weatherTexture;
    public Texture2D weatherTemperatureTexture;
    public float weatherDialRotation;
    public GameObject particles;
    public float temperatureChangePerSecond;
    public float lightIntensityTarget;
}
