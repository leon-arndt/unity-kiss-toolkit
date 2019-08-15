using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHolder : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Gear>() != null)
        {
            //continue only if the gear is broken
            if (other.GetComponent<Gear>().IsBroken() == false) return;

            other.GetComponent<Gear>().Fix();

            //is being held by the player?
            if (other.gameObject.transform.parent != null)
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerController>() != null)
                {

                    //determine PlayerController
                    PlayerController player = other.gameObject.transform.parent.GetComponent<PlayerController>();

                    //Player drops object automatically
                    player.holdingSomething = false;

                    //Remove from player's interactable list
                    player.RemoveFromInteractableList(other.gameObject);
                }
            }

            Debug.Log("Gear holder received gear");
        }
    }
}
