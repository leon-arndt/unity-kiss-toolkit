using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for the enemies in the game.
/// All ingredients are pickups. All plants derive from this class.
/// Each ingredient uses a scriptable object of type IngredientData to store its stats.
/// </summary>
public class Ingredient : PickUp
{
    public IngredientData ingredientData;
    public float sleepTime;

    // Use this for initialization
    void Start()
    {
        sleepTime = ingredientData.sleepTime;
    }

    public override void Pickup(Transform newParent)
    {
        isPickedUp = true;

        base.Pickup(newParent);
    }

    public override void PutDown()
    {
        isPickedUp = false;

        base.PutDown();
        GetComponent<BoxCollider>().enabled = true;
    }

    public override void Interact(Transform newParent)
    {
        Pickup(newParent);
    }

    public override void Throw()
    {
        isPickedUp = false;

        base.Throw();
        GetComponent<BoxCollider>().enabled = true;
    }

    public void AddSleepTime(float f)
    {
        if (sleepTime < 0.1f)
        {
            sleepTime = f;
        }
    }

    //used to make sure that the plants behavior is stopped when it is added to the pot
    public virtual void Pacify()
    {

    }

    void OnDestroy()
    {
        PlayerController.RemoveFromBothInteractableLists(gameObject);
        print("Ingredient was destroyed");
    }
}
