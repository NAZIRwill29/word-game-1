using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    //for sprite img
    public Sprite emptyChest;
    public int pesosAmount = 5;
    protected override void OnCollide(Collider2D coll)
    {
        //call parent function
        //base.OnCollide(coll);
        // Debug.Log("Grant Penny");
        //only player
        if (coll.name != "Player")
            return;
        if (!collected)
        {
            collected = true;
            //change img to empty chest
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            //Debug.Log("Grant " + pesosAmount + " pesos");
            GameManager.instance.pesos += pesosAmount;
            GameManager.instance.ShowText("+" + pesosAmount + " pesos!", 25, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
            //play sound when collide
            collidableAudio.PlayOneShot(triggerSound, 1.0f);
        }
    }
}
