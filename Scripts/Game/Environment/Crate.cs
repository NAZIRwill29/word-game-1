using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    private float lastImmuneCrate;
    public int rewardExp;

    private void Start()
    {
        fighterAudio = GetComponent<AudioSource>();
    }
    protected override void Death()
    {
        fighterAudio.PlayOneShot(effectSound[0], 0.3f);
        //death anim + destroy
        StartCoroutine(DeathAnimDestroy());
        //check if have dropitem -> drop item after death
        if (dropItem)
            dropItem.SpawnItem(transform);
        if (rewardExp != 0)
            GameManager.instance.GrantXp(rewardExp);
    }
    protected override IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(0);
        effectAnim[0].SetTrigger("Explode");
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmuneCrate > immuneTime)
        {
            lastImmuneCrate = Time.time;
            //make sound effect when collide
            fighterAudio.PlayOneShot(effectSound[1], 0.3f);
        }
        base.ReceiveDamage(dmg);

    }
}
