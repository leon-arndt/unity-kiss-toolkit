using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is obsolete
/// </summary>
public class Lever : Interactable {
    [SerializeField]
    private bool position = false;

    public override void Interact(Transform playerTransform)
    {
        if (!position)
        {
            transform.Rotate(30f * Vector3.back);
            position = true;

            CameraShake.shakeDuration = 0.3f;
        }
        else
        {
            transform.Rotate(30f * Vector3.forward);
            position = false;

            CameraShake.shakeDuration = 0.3f;
        }
    }
}
