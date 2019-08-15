using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This simple component is used to trigger the mellow goodbye music.
/// </summary>
public class AudioTrigger : MonoBehaviour {
    [SerializeField]
    string audioToTrigger;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                AudioManager.instance.StopAll();
                AudioManager.instance.PlayEvent(audioToTrigger);
            }

            triggered = true;
        }
    }
}
