using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialAnimation : MonoBehaviour {
    [SerializeField]
    Material defaultFace, hitFace, blinkFace, body;

    [SerializeField]
    Renderer rend;

    public float revertTime = 0f;
    private const float timeBetweenBlinks = 3f;
    private const float blinkDuration = 0.3f;
    public float timeUntilNextBlink = timeBetweenBlinks;

    private void Start()
    {
        timeUntilNextBlink = Random.Range(0, 5);
    }

    private void Update()
    {
        if (revertTime > 0)
        {
            revertTime -= Time.deltaTime;
        }

        //calls once
        if (revertTime < 0.1f && revertTime > 0f)
        {
            rend.material = defaultFace;
            revertTime = 0f;
        }
        else
        {
            //revert time must be 0
            timeUntilNextBlink -= Time.deltaTime;

            if (timeUntilNextBlink < 0)
            {
                Blink();
            }
        }
    }

    public void ChangeFaceToHit()
    {
        rend.material = hitFace;
        revertTime = 2f;
    }

    //Flicker the material to indicate invincibility
    public void FlickerMaterial(float f)
    {
        float fresnelStrength = 0.5f * Mathf.Sin(35f * f) + 0.5f;
        rend.material.SetFloat("_FresnelStrength", fresnelStrength);
        body.SetFloat("_FresnelStrength", fresnelStrength);
    }

    private void Blink()
    {
        if (rend.material != hitFace)
        {
            rend.material = blinkFace;
            revertTime = blinkDuration;
            timeUntilNextBlink = timeBetweenBlinks + Random.Range(0, 5);
        }
    }
}
