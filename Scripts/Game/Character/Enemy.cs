using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //experience
    public int xpValue = 1;
    //logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    public Transform playerTransform;
    public Vector3 startingPosition;
    //hitbox
    // private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D filter;
    [Tooltip("healthBar")]
    public HealthEnemy healthEnemy;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        // hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        //change healthBar
        healthEnemy.OnHitpointChange();
    }

    protected virtual void FixedUpdate()
    {
        //is player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            //make chasing
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;
            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    //chase player
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                //reset to previous position
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            //reset to previous position
            UpdateMotor(startingPosition - transform.position);
        }

        // UpdateMotor(Vector3.zero);
        //check for overlaps
        //look for overlap collider
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            //continue code
            if (hits[i] == null)
                continue;
            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }
            //clean array
            hits[i] = null;
        }
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        //change healthBar
        healthEnemy.OnHitpointChange();
    }

    protected override void Death()
    {
        base.Death();
        //death anim + destroy
        StartCoroutine(DeathAnimDestroy());
        //hide healthBar
        healthEnemy.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        //give exp
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
    //play sound effect
    public void PlaySoundMagic()
    {
        fighterAudio.PlayOneShot(effectSound[3], 1.0f);
    }
    public void PlaySoundSwingWeapon()
    {
        fighterAudio.PlayOneShot(effectSound[4], 1.0f);
    }
    public void PlaySoundMoveAttack()
    {
        fighterAudio.PlayOneShot(effectSound[5], 1.0f);
    }
    public void PlaySoundShoot()
    {
        fighterAudio.PlayOneShot(effectSound[6], 1.0f);
    }
    public void PlaySoundTeleport()
    {
        fighterAudio.PlayOneShot(effectSound[7], 1.0f);
    }
    public void ChangeChaseAndtriggerLength(float chaseNum, float triggerNum)
    {
        chaseLength = chaseNum;
        triggerLength = triggerNum;
    }
}
