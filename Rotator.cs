using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script simply does what is written on the box.
/// </summary>
public class Rotator : MonoBehaviour {
    [SerializeField]
    private Vector3 rotateVector;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Time.timeScale * rotateVector);
	}
}
