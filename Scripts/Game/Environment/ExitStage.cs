using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitStage : Collidable
{
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            //exit stage
            GameManager.instance.BackToMenu();
            GameManager.instance.PlaySoundExitStage();
        }
    }
}
