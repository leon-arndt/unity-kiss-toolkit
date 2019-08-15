using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script was kindly provided by Marian Brinkmann.
/// It was altered by Leon Arndt to work for Tea onn Rails.
/// </summary>
public class GrayscaleEffect : MonoBehaviour
{
    public AnimationCurve m_EffectOnValue = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,1f));
    private const string SHADER_NAME = "Hidden/GreyscaleShader";
    private Material m_Material;
    public static GrayscaleEffect Instance;
        
    void Awake()
    {
        m_Material = new Material(Shader.Find(SHADER_NAME));

        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //if (AbilityWheelController.MAIN != null)
        //{
        //    AbilityWheelController.MAIN.OnWheelActivation += OnWheelActivity;
        //}
    }

    private void OnDisable()
    {
        //if (AbilityWheelController.MAIN != null)
        //{
        //    AbilityWheelController.MAIN.OnWheelActivation -= OnWheelActivity;
        //}
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, m_Material);
    }

    void SetGrayscaleIntensity(float value)
    {
        m_Material.SetFloat("_GreyscaleAmout", m_EffectOnValue.Evaluate(value));
    }

    public void FadeToAndFromGray(bool toGray)
    {
        if (toGray)
        {
            Debug.Log("fading to gray");
            StartCoroutine(FadeToGray());
        }
        else
        {
            StartCoroutine(FadeFromGray());
        }
    }


    IEnumerator FadeToGray()
    {
        for (float f = 0f; f <= 1; f += 0.1f)
        {
            SetGrayscaleIntensity(f);
            yield return null;
        }
    }

    IEnumerator FadeFromGray()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            SetGrayscaleIntensity(f);
            yield return null;
        }
    }
}