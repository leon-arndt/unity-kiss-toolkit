using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modulates the light intensity of a light in the scene over time.
/// </summary>
public class LightModulation : MonoBehaviour {
    [SerializeField]
    float minIntensity, modulationAmount, modulationSpeed, timeOffset;

    new Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    //modulate the intensity
    private void Update()
    {
        light.intensity = minIntensity + Mathf.PingPong(modulationSpeed * Time.time + timeOffset, modulationAmount);
    }

}
