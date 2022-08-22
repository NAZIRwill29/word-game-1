using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D ballRb;
    public bool isShoot;
    public Transform shooterTransform;
    [SerializeField]
    private int damage = 1;
    [Tooltip("thunder / ice / fire / wind")]
    public string specialPower;
    private void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fighter")
        {
            if (other.name == "Player")
                AfterCollide(other, damage);
            else
                return;

        }
        else if (other.tag == "Destructable")
        {
            AfterCollide(other, damage);
        }
        else if (other.tag == "Weapon")
        {
            if (other.name == "CharObj")
                AfterCollide();
            else
                return;
        }
        else if (other.name == "Collision")
        {
            Debug.Log("Collision");
            AfterCollide();
        }
    }

    private void AfterCollide()
    {
        isShoot = false;
        //stop force
        ballRb.velocity = Vector2.zero;
        //make to original posistion
        gameObject.transform.position = shooterTransform.position;
    }
    private void AfterCollide(Collider2D other, int damage)
    {
        //Create new dmg obj b4 send to enemy
        Damage dmg = new Damage
        {
            damageAmount = damage,
            origin = transform.position,
            pushForce = 0
        };
        //send message to other to make call ReceiveDamage function
        other.SendMessage("ReceiveDamage", dmg);
        //send special damage 
        if (specialPower != "")
            SendSpecialDamage(other);
        isShoot = false;
        //stop force
        ballRb.velocity = Vector2.zero;
        //make to original posistion
        gameObject.transform.position = shooterTransform.position;
    }

    //send special damage to player
    private void SendSpecialDamage(Collider2D coll)
    {
        //send damage value to player
        coll.GetComponent<Player>().ChangeEnemyDamage(damage);
        coll.SendMessage("ReceiveSpecialPowerDamage", specialPower);
    }

    //set isShoot
    public void SetIsShoot(bool shoot)
    {
        isShoot = shoot;
    }

}
