using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBeam : MonoBehaviour {
    bool isLowering = false;
    float desiredSize = 8f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 10f);
        Invoke("StartLowering", 1);
	}
	
	// Update is called once per frame
	void Update () {
        if (isLowering)
        {
            Vector3 currentScale = transform.localScale;
            float size = currentScale.x - 0.5f * Time.deltaTime;
            transform.localScale = new Vector3(size, size, currentScale.z);

            if (currentScale.x < 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }

    void StartLowering()
    {
        isLowering = true;
    }

    //Make players dizzyand break equipment
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().AddBurnTime(2f);
            Debug.Log("Player hit by fireBeam");
        }

        //not detected properly --> refactor
        if (other.GetComponent<Breakable>() != null)
        {
            other.GetComponent<Breakable>().Break();
            Debug.Log("Equipment broken by fireBeam");
        }
    }
}
