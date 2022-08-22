using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - not working
public class BombBall : Bomb
{
    private Rigidbody2D ballRb;
    public bool isShoot;
    public Transform shooterTransform;
    [SerializeField]
    private int damage = 1;

    protected override void Start()
    {
        base.Start();
        ballRb = GetComponent<Rigidbody2D>();
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (isHasExplode)
            return;
        if (coll.name == "Player" || coll.name == "CharObj")
        {
            BombExplode();
            AfterCollide(coll, damage);
        }
        else
        {
            BombExplode();
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
        anim.SetTrigger("Idle");
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
        isShoot = false;
        //stop force
        ballRb.velocity = Vector2.zero;
        //make to original posistion
        gameObject.transform.position = shooterTransform.position;
        anim.SetTrigger("Idle");
    }
    //set isShoot
    public void SetIsShoot(bool shoot)
    {
        isShoot = shoot;
    }
}
