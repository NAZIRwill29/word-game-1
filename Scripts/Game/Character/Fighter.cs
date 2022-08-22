using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //public field
    public int hitpoint = 20;
    public int maxHitpoint = 20;
    public float pushRecoverySpeed = 0.2f;
    //Immunity
    [SerializeField]
    protected float immuneTime = 1.0f;
    protected float lastImmune;
    //push
    protected Vector3 pushDirection;
    //Explode - Hit - Dead
    public Animator[] effectAnim;
    protected AudioSource fighterAudio;
    ////       0         1       2      3     4                5            6        7             8            9                   10          11
    //player = Explode - Hit - Dead - heal - shoot        - reload      - failed - word success - level up - reject char in word - hit weapon - hit blocking
    //enemy = Explode - Hit - Dead - magic - swing weapon - move attack - shoot - teleport
    [Tooltip("see in vscode")]
    public AudioClip[] effectSound;
    public DropItem dropItem;

    //All fighter can receive Damage / die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            if (effectAnim[1])
                effectAnim[1].SetTrigger("Hit");
            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 15, Color.red, transform.position, Vector3.zero, 0.5f);

            //check to make death
            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    //death
    protected virtual void Death()
    {
        fighterAudio.PlayOneShot(effectSound[2], 1.0f);
        //check if have dropitem -> drop item after death
        if (dropItem)
            dropItem.SpawnItem(transform);
    }

    //coroutine dead anim + destroy
    protected IEnumerator DeathAnimDestroy()
    {
        yield return StartCoroutine(DeathAnim());
        yield return StartCoroutine(DestroyEnemy());
    }
    protected virtual IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(0);
        if (effectAnim[2])
            effectAnim[2].SetTrigger("Dead");
    }
    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
