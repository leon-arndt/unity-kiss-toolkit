using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple component to make the gameObject look at the camera
public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + (transform.position - Camera.main.transform.position));
        }
	}
}
