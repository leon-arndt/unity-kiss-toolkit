using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public UIHint uiHint;

    //can the player currently interact with this object (true by default)
    public bool interactable = true;

    public virtual void Interact(Transform transform)
    {

    }

    public void DeleteUIHint()
    {
        if (uiHint != null)
        {
            uiHint.DestroySelf();
            uiHint = null;
        }
    }
}
