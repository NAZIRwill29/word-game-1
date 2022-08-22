using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Collidable
{
    public ChangeSprite lockCS;
    public Chest chest;
    public ExitStage exitStage;
    [Tooltip("GoldKey / SilverKey")]
    public string keyName;
    public bool isHasAnimation;
    private GameObject player;
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
    }
    protected override void OnCollide(Collider2D coll)
    {
        //check has key script and keyname
        if (coll.name == keyName)
        {
            if (isHasAnimation)
            {
                lockCS.SetAnimation("On", 1);
            }
            else
            {
                lockCS.ChangeImage(1);
            }
            //if have chest script
            if (chest)
                chest.enabled = true;
            //check if have exitstage
            if (exitStage)
                StartCoroutine(ExitStageEnable());
            //use key in player
            player.GetComponent<Player>().UseKey(keyName);
        }
    }

    //coroutine enable exitStage
    private IEnumerator ExitStageEnable()
    {
        yield return new WaitForSeconds(0.4f);
        exitStage.enabled = true;
        Debug.Log("exitstage");
    }
}
