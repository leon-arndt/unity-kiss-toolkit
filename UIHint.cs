using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHint : MonoBehaviour {
    public Transform playerTransform;
    public Transform transformToTrack;

    private void Start()
    {
        //start moving when the dialogue ends
        //EventManager.InteractAction(interactable) += DestroySelf;
    }

    // Update is called once per frame
    void Update () {
        if (transformToTrack == null)
        {
            Destroy(gameObject);
            return;
        }

        //track transform
        transform.position = transformToTrack.position + Vector3.up;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
