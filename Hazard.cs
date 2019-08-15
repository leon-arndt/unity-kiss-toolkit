using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0.66f * Vector3.left);
	}
}
