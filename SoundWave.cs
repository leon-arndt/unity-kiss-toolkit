using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for the hazard created by the Horn Flower attack.
/// </summary>
public class SoundWave : MonoBehaviour {
    private float lifeTime = 3f;
    private float growSpeed = 1f;
    private float moveSpeed = 0.15f;
    private float startXScale;
    private float startTime;
    private float pushForce = 1000f;

    [SerializeField]
    Transform[] waveTransforms;

    [SerializeField]
    Collider trigger;

    [SerializeField]
    Collider[] colliders;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        startTime = Time.time;
        startXScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(moveSpeed * Vector3.forward);

        //grow slowly
        transform.localScale = new Vector3(startXScale + growSpeed * (Time.time - startTime), 1, 1);

        UpdateWaves();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger.enabled)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                StartCoroutine(PushAwayOverTime(other.GetComponent<Rigidbody>()));
                other.GetComponent<PlayerController>().AddDizzyTime(2f);
                Destroy(gameObject, 0.5f);
            }
        }
    }


    private void PushAway(Rigidbody other)
    {
        other.AddForce(transform.forward * pushForce);
        trigger.enabled = false;
        Debug.Log("PUSH!!");

        foreach (var item in colliders)
        {
            item.enabled = false;
        }
    }

    IEnumerator PushAwayOverTime(Rigidbody other)
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            other.AddForce(transform.forward * pushForce);
            yield return null;
        }
    }

    void UpdateWaves()
    {
        for (int i = 0; i < waveTransforms.Length; i++)
        {
            float offset = i;
            float size = Mathf.Sin(10f * Time.time + i);
            waveTransforms[i].localScale = new Vector3(size, 1, 0.25f);
        }
    }
}
