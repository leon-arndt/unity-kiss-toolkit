using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for gears which are not meant to break
/// </summary>
public class VisualGear : MonoBehaviour {
    [SerializeField]
    float rotationSpeed = 1f;

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.right * rotationSpeed * Time.timeScale * VehicleController.Instance.GetRatioCurrentSpeedToNormal());
    }
}
