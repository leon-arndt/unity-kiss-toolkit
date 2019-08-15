using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the binpipe which deletes ingredients and its revolving door.
/// </summary>
public class TrashCan : MonoBehaviour {
    [SerializeField]
    Animator animator;

    [SerializeField]
    ParticleSystem smokeEffectPS;

    bool doorOpen = false;

    private void Start()
    {
        //increase the speed of the animation controller
        //animator.speed = 4f;
    }

    private void Update()
    {
        float distanceToClosestPlayer;
        Transform player1Transform = GameController.instance.player1.transform;
        Transform player2Transform = GameController.instance.player2.transform;

        //find the closest player
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;


        //open the door if the player is close
        distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.position);
        if (distanceToClosestPlayer < 3f && doorOpen == false)
        {
            animator.SetBool("DoorOpen", true);
            doorOpen = true;
            AudioManager.instance.PlayEvent("Play_binpipe_open");
        }
        else if (distanceToClosestPlayer > 3f && doorOpen == true)
        {
            animator.SetBool("DoorOpen", false);
            doorOpen = false;
            AudioManager.instance.PlayEvent("Play_binpipe_close");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!doorOpen) return; //only go on if the hatch is opened

        if (other.GetComponent<Ingredient>() != null)
        {
            if (other.gameObject.transform.parent != null)
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerController>())
                {
                    //determine PlayerController
                    PlayerController player = other.gameObject.transform.parent.GetComponent<PlayerController>();

                    player.DropObject();
                }
            }

            //remove from both players interactable list
            PlayerController.RemoveFromBothInteractableLists(other.gameObject);

            //play vfx
            smokeEffectPS.Play();

            other.GetComponent<Ingredient>().PutDown();
            Destroy(other.gameObject);
            Debug.Log("Trash destroyed ingredient");
        }
    }
}
