using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : EnemyAttackPattern
{
    [Tooltip("for shooting")]
    [SerializeField]
    protected float shootingSpeed, minShootCooldown, maxShootCooldown;
    public GameObject[] ballShoots;
    protected Rigidbody2D ballShootsRb;
    protected Ball ballScript;
    [Tooltip("for magic shoot")]
    [SerializeField]
    protected bool isMagic;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isCanAttack)
            return;
        AttackTypeShooting();
    }

    protected virtual void AttackTypeShooting()
    {
        cooldownAttack = Random.Range(minShootCooldown, maxShootCooldown);
        //is player in range?
        if (Vector3.Distance(bossScript.playerTransform.position, bossScript.startingPosition) < chaseLength)
        {
            if (Time.time - lastAttack > cooldownAttack)
            {
                ShootingBall();
            }
        }
    }

    protected virtual void ShootingBall()
    {
        int index = Random.Range(0, ballShoots.Length);
        ballShootsRb = ballShoots[index].GetComponent<Rigidbody2D>();
        ballScript = ballShoots[index].GetComponent<Ball>();
        if (!ballScript.isShoot)
        {
            lastAttack = Time.time;
            Shoot(index);
        }
    }

    //shooting function
    protected virtual void Shoot(int index)
    {
        ballShootsRb.AddForce((bossScript.playerTransform.position - ballShoots[index].transform.position).normalized * shootingSpeed, ForceMode2D.Impulse);
        if (isMagic)
            PlaySoundEffect(3);
        else
            PlaySoundEffect(6);
        ballScript.SetIsShoot(true);
    }
}
