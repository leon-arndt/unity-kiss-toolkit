using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour {
    [SerializeField]
    private ParticleSystem psSmoke;

    [SerializeField]
    private Animator animator;

    private bool doorOpen;
    private Transform player1Transform, player2Transform;

    private void Start()
    {
        player1Transform = GameController.instance.player1.transform;
        player2Transform = GameController.instance.player2.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Coal>() != null)
        {
            AddCoal(other.gameObject);
        }
    }

    private void Update()
    {
        float distanceToClosestPlayer;

        //find the closest player
        Transform closestPlayer = (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position)) ? player1Transform : player2Transform;

        //open the door if the player is close
        distanceToClosestPlayer = Vector3.Distance(transform.position, closestPlayer.position);
        if (distanceToClosestPlayer < 3f && doorOpen == false)
        {
            animator.SetBool("DoorOpen", true);
            doorOpen = true;
        }
        else if (distanceToClosestPlayer > 3f && doorOpen == true)
        {
            animator.SetBool("DoorOpen", false);
            doorOpen = false;
        }
    }

    private void AddCoal(GameObject coal)
    {
        EventManager.ThrowAddCoalEvent();

        VehicleController.Instance.SpeedUp();
        VehicleController.Instance.IncreaseTemperature(20f + Random.Range(0, 5));

        //if the player was carrying the object
        if (coal.transform.parent != null)
        {
            if (coal.transform.parent.GetComponent<PlayerController>() != null)
            {

                //determine PlayerController
                PlayerController player = coal.transform.parent.GetComponent<PlayerController>();

                //Player drops object automatically
                player.DropObject();
            }
        }

        //audio
        AudioManager.instance.PlayEvent("Play_add_coal_furnace");

        //vfx
        psSmoke.Clear();
        psSmoke.Play();

        PlayerController.RemoveFromBothInteractableLists(coal);

        Destroy(coal);
        Debug.Log("Furnaced received coal");
    }
}
