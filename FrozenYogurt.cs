using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Frozen Yogurt plant ingredient.
/// Uses a special bouncy material to imitate the ball from the game Pong.
/// </summary>
public class FrozenYogurt : Ingredient {
    [SerializeField]
    GameObject icePrefab;

    [SerializeField]
    Animator animator;

    [SerializeField]
    float iceCreateVerticalOffset;

    [SerializeField]
    Collider plantCollider;

    [SerializeField]
    PhysicMaterial defaultPhysicMaterial, bouncyPhysicMaterial;

    Rigidbody rb;
    private Vector3 lastIcePosition;

    private bool bouncy;
    private const float maxSpeed = 10f;
    private const float distanceBetweenIce = 1.5f;
    private const float increaseSpeedFactor = 1.01f;

    private void Start()
    {
        lastIcePosition = transform.position;
        rb = GetComponent<Rigidbody>();

        rb.AddForce(100f * transform.forward);
    }

    private void Update()
    {
        if (sleepTime > 0)
        {
            sleepTime -= Time.deltaTime;
        }

        if (!isPickedUp && bouncy)
        {
            if (Vector3.Distance(transform.position, lastIcePosition) > distanceBetweenIce)
            {
                CreateIce();
            }
        }

        //limit the velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        else
        {
            rb.velocity *= increaseSpeedFactor;
        }
    }

    private void CreateIce()
    {
        //Update the last Ice Position
        lastIcePosition = transform.position;

        //Create the ice
        var ice = Instantiate(
                   icePrefab,
                   transform.position + new Vector3(0, iceCreateVerticalOffset, 0),
                   Quaternion.identity, SurpriseManager.Instance.GetSurpriseParentTransform());
    }

    public override void Pickup(Transform newParent)
    {
        base.Pickup(newParent);
        animator.SetBool("pickedUp", true);
        plantCollider.material = defaultPhysicMaterial;
        bouncy = false;   
    }

    public override void ReverToState()
    {
        base.ReverToState();
        animator.SetBool("pickedUp", false);
        rb.constraints = RigidbodyConstraints.FreezeRotationX 
            | RigidbodyConstraints.FreezeRotationY 
            | RigidbodyConstraints.FreezeRotationZ;
    }

    //update the physic material and rigidbody constraints (pong ball behavior)
    private void MakeBouncy()
    {
        plantCollider.material = bouncyPhysicMaterial;
        rb.constraints = RigidbodyConstraints.FreezeRotationX 
            | RigidbodyConstraints.FreezeRotationY 
            | RigidbodyConstraints.FreezeRotationZ 
            | RigidbodyConstraints.FreezePositionY; //don't bounce up and down

        //add force
        float horizontalFactor = Random.Range(0, 2) * 2 - 1;
        rb.AddForce(300f * transform.forward + 200f * horizontalFactor * Vector3.right);

        bouncy = true;
    }

    //make bouncy when colliding with floor
    private void OnCollisionEnter(Collision collision)
    {
        if (!bouncy)
        {
            if (collision.gameObject.tag == "Floor")
            {
                //collided with floor
                MakeBouncy();
            }
        }
    }
}
