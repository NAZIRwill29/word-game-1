using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTrigger : Collidable
{
    private bool isTrigger;
    // 0    1
    // off - on
    public Sprite[] propSprites;
    private SpriteRenderer propSR;
    public ChangeSprite[] affectedObjCS;
    public Chest chest;
    public bool isHasAnimation;
    private float lastTrigger;
    private float cooldown = 2;
    protected override void Start()
    {
        base.Start();
        propSR = GetComponent<SpriteRenderer>();
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player" || coll.name == "Special")
        {
            if (Time.time - lastTrigger > cooldown)
            {
                lastTrigger = Time.time;
                if (!isTrigger)
                {
                    //change propTrigger image
                    propSR.sprite = propSprites[1];
                    isTrigger = true;
                    if (affectedObjCS.Length > 0)
                    {
                        //if animation
                        if (isHasAnimation)
                        {
                            foreach (var item in affectedObjCS)
                            {
                                item.SetAnimation("On", 1);
                            }
                        }
                        else
                        {
                            //if have change sprite
                            foreach (var item in affectedObjCS)
                            {
                                item.ChangeImage(1);
                            }
                        }
                    }
                    //if have chest script
                    if (chest)
                        chest.enabled = true;
                    //play sound when collide
                    collidableAudio.PlayOneShot(triggerSound, 1.0f);
                }
                else
                {
                    //do something
                    propSR.sprite = propSprites[0];
                    isTrigger = false;
                    if (affectedObjCS.Length > 0)
                    {
                        //if animation
                        if (isHasAnimation)
                        {
                            foreach (var item in affectedObjCS)
                            {
                                item.SetAnimation("Off", 0);
                            }
                        }
                        else
                        {
                            //if have change sprite
                            foreach (var item in affectedObjCS)
                            {
                                item.ChangeImage(0);
                            }
                        }
                    }
                    if (chest)
                        chest.enabled = false;
                    //play sound when collide
                    collidableAudio.PlayOneShot(triggerSound, 1.0f);
                }
            }
        }
    }
}
