using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        //if collide player
        if (coll.name == "Player")
            OnCollect();
    }

    //collect
    protected virtual void OnCollect()
    {
        collected = true;
    }
}
