using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This UI script handles the spinning of the weather dial
/// </summary>
public class UIWeatherDial : MonoBehaviour {
    [SerializeField]
    RectTransform weathDial;

    [SerializeField]
    ParticleSystem dialFlash;

    uint timeBetweenDialSounds = 2;

    public void SpinDialTo(float f)
    {
        dialFlash.Play();
        StartCoroutine(SpinToDegree(f));
    }

    IEnumerator SpinToDegree(float degreeGoal)
    {
        //round up the z rotation float
        float nextIdenticalZ = Mathf.Ceil((weathDial.rotation.z + 360) / 360f) * 360;
        int timeSinceLastDialSound = 0;

        for (float f = 0f; f <= 1; f += 0.01f)
        {
            float zRot = Mathf.SmoothStep(weathDial.rotation.z, nextIdenticalZ + degreeGoal, f);
            weathDial.rotation = Quaternion.Euler(0, 0, zRot);
            
            //audio
            timeSinceLastDialSound++;
            if (timeSinceLastDialSound > timeBetweenDialSounds)
            {
                AudioManager.instance.PlayEvent("Play_ui_weather_dial");
                timeSinceLastDialSound = 0;
            }



            yield return null;
        }
    }
}
