using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {
    [SerializeField]
    bool shouldDestroyAfterWhile;

    [SerializeField]
    float destroyAfter;

    float playerStartFriction;
    static float PlayerStartFriction;

    private void Start()
    {
        playerStartFriction = GameController.instance.player1.GetComponent<CapsuleCollider>().material.dynamicFriction;
        PlayerStartFriction = playerStartFriction;

        if (shouldDestroyAfterWhile)
        {
            Destroy(gameObject, destroyAfter);
        }
    }

    public static void ResetAllPlayersDrag()
    {
        GameController.instance.player1.slipperyEffect = false;
        GameController.instance.player2.slipperyEffect = false;
    }
}
