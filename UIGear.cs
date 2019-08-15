using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is used to animate the gears based on the current damage level of the vehicle.
/// </summary>
public class UIGear : MonoBehaviour {
    [SerializeField]
    bool reverse;

    float rotationSpeed = 1f;
    RectTransform rt;
    Vector3 turnVector;

    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();

        if (reverse)
        {
            turnVector = Vector3.back;
        }
        else
        {
            turnVector = Vector3.forward;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float damageSpeedFactor = (1 - VehicleController.Instance.GetDamageRatio());
        rt.Rotate(turnVector * damageSpeedFactor);
	}
}
