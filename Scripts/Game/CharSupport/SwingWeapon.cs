using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : EnemyAttackPattern
{
    private Animator weaponAnim;
    // Start is called before the first frame update
    void Start()
    {
        weaponAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isCanAttack)
        {
            AttackTypeSwing();
            TriggerChase(true);
        }
        else
        {
            TriggerChase(false);
        }
    }

    //method swing weapon
    private void AttackTypeSwing()
    {
        if (Time.time - lastAttack > cooldownAttack)
        {
            lastAttack = Time.time;
            weaponAnim.SetTrigger("Swing");
            PlaySoundEffect(4);
        }
    }
}
