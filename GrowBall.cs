using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fun little component which lets balls grow when they are thrown
/// </summary>
public class GrowBall : PickUp {

    public override void Pickup(Transform parent)
    {
        base.Pickup(parent);
        Grow();
    }

    public override void Throw()
    {
        base.Throw();
        Grow();
    }

    private void Grow()
    {
        if (transform.localScale.x < 5)
        {
            transform.localScale = transform.localScale * 1.25f;
        }
    }
}
