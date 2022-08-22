using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collidable
{
    public Animator anim;
    protected bool isHasExplode;

    protected override void OnCollide(Collider2D coll)
    {
        if (isHasExplode)
            return;
        if (coll.name == "Player" || coll.name == "CharObj")
        {
            BombExplode();
        }
    }

    protected virtual void BombExplode()
    {
        anim.SetTrigger("Explode");
        collidableAudio.PlayOneShot(triggerSound, 1);
        isHasExplode = true;
    }

    public void ChangeIsHasExplode(bool isTrue)
    {
        isHasExplode = isTrue;
    }
}
