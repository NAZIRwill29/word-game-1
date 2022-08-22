using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string message;
    private float cooldown = 3;
    private float lastShout;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
    }

    protected override void OnCollide(Collider2D coll)
    {
        //Debug.Log("collide");
        if (coll.name != "Player")
            return;
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 25, Color.white, transform.position + new Vector3(0, 0.36f, 0), Vector3.zero, 4.0f);
            // Debug.Log("talk");
            //make sound effect when attack
            collidableAudio.PlayOneShot(triggerSound, 1.0f);
        }
    }
}
