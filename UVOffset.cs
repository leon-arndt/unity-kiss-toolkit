using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVOffset : MonoBehaviour {
    float scrollSpeed = 0.5f;

    void Update()
    {
        scrollSpeed = 0.5f * VehicleController.Instance.GetRatioCurrentSpeedToNormal();

        var offset = Time.time * scrollSpeed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset % 1, 0);
    }
}
