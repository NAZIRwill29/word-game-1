using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //Damage
    public int damage = 1;
    public float pushForce = 3;
    [Tooltip("thunder / ice / fire / wind")]
    public string specialPower;
    protected override void OnCollide(Collider2D coll)
    {
        //set paused game
        if (GameManager.instance.isPaused)
            return;
        //check if collide with player
        if (coll.tag == "Fighter" && coll.name == "Player")
        {
            //Debug.Log("damage");
            //Create new dmg obj b4 send to player
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };
            //send message to coll to make call ReceiveDamage function
            coll.SendMessage("ReceiveDamage", dmg);
            //send special damage 
            if (specialPower != "")
                SendSpecialDamage(coll);
        }
    }
    //send special damage to player
    private void SendSpecialDamage(Collider2D coll)
    {
        //send damage value to player
        coll.GetComponent<Player>().ChangeEnemyDamage(damage);
        coll.SendMessage("ReceiveSpecialPowerDamage", specialPower);
    }
}
