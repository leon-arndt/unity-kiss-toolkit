using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaNeckButton : Interactable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact(Transform transform)
    {
        if (transform.GetComponent<PlayerController>() != null)
        {
            if (!transform.GetComponent<PlayerController>().longNeck)
            {
                //extend the neck
                transform.GetComponent<PlayerController>().longNeck = true;
                transform.GetComponent<PlayerController>().neckRigTransform.Translate(-2, 0, 0, Space.Self);
            }
            else
            {
                //reset the player neck
                //transform.GetComponent<PlayerController>().longNeck = false;
                //transform.GetComponent<PlayerController>().neckRigTransform.Translate(2, 0, 0, Space.World);
            }
        }
    }
}
