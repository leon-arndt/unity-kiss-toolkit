using UnityEngine;
using System.Collections;

//Script written by ftvs on GitHub and altered by Leon Arndt to add decaying rotation over time

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    private Quaternion camStartRotation;

    // How long the object should shake for.
    public static float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public const float standardShakeAmount = 0.7f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public static CameraShake Instance;

    Vector3 originalPos;

    void Awake()
    {
        //disable if camera shake is turned off under settings
        if (SettingsManager.Instance.ScreenShake == false)
        {
            enabled = false;
        }



        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        camStartRotation = camTransform.rotation;

        //Singleton
        if (Instance == null)
        Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            //the amount in angles to randomly rotate the camera, gets closer to 0 as shakeDuration decreases
            float rotateAmount = Mathf.Min(0.5f, 0.2f * shakeDuration);
            Vector3 shakeVector = new Vector3(Random.Range(-rotateAmount, rotateAmount), Random.Range(-rotateAmount, rotateAmount), Random.Range(-rotateAmount, rotateAmount));
            camTransform.rotation = Quaternion.Euler(camStartRotation.eulerAngles + shakeVector);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
            camTransform.rotation = camStartRotation;
        }
    }


    //Pair of methods for delayed shake
    public IEnumerator DelayShake(float amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        shakeDuration = amount;
        yield return null;
    }

    public void ShakeWithDelay(float amount, float delay)
    {
        StartCoroutine(DelayShake(amount, delay));
    }
}