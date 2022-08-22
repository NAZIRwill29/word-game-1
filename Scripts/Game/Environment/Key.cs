using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    private GameObject player;
    //GoldKey - SilverKey
    [Tooltip("GoldKey / SilverKey")]
    public string keyName;
    public Animator keyAnim;
    private bool isPicked;
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
    }
    //collect
    protected override void OnCollect()
    {
        //prevent from picked many time and prevent pick more key
        if (isPicked || player.GetComponent<Player>().isHasKey)
            return;
        base.OnCollect();
        //turn on ke in player
        player.GetComponent<Player>().GetKey(keyName);
        //play sound when collide
        collidableAudio.PlayOneShot(triggerSound, 1.0f);
        //turn off
        keyAnim.SetTrigger("Pick");
        isPicked = true;
    }
    public void DropKey()
    {
        keyAnim.SetTrigger("Drop");
        isPicked = false;
    }
}
