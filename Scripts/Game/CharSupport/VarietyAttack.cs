using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarietyAttack : Shooting
{
    private Animator anim;
    [Tooltip("shooting / swing / moveAttack / teleport")]
    public string[] listAttackName;
    [Tooltip("duration of attack")]
    public float[] durationAttack;
    //init for every in cooldownTrigger
    public float cooldownSwing, cooldownTeleport;
    private string attackName;
    private int num;
    public GameObject enemy;
    public bool isTeleportOnce, isTeleportRandom, isTeleportWithSwing, isTeleportChase;
    [Tooltip("for teleport position")]
    [SerializeField]
    private float teleportMinX, teleportMaxX, teleportMinY, teleportMaxY;
    private float lastTeleport, lastMove, xSpeedOri, ySpeedOri;

    void Start()
    {
        //store original speed
        if (bossScript)
        {
            xSpeedOri = bossScript.xSpeed;
            ySpeedOri = bossScript.ySpeed;
        }
        else if (enemyScript)
        {
            xSpeedOri = enemyScript.xSpeed;
            ySpeedOri = enemyScript.ySpeed;
        }
        anim = GetComponent<Animator>();
        StartCoroutine(ChangeAttack());
    }

    //TODO - atatck move and teleport
    protected override void FixedUpdate()
    {
        //set paused game  or set stun
        if (CheckStunAndGamePause())
            return;
        //start attack
        DecideAttack(attackName);
    }

    //change attack
    private IEnumerator ChangeAttack()
    {
        //revert back to original speed 
        ChangeChaseSpeed(xSpeedOri, ySpeedOri);
        attackName = listAttackName[num];
        cooldownTrigger = durationAttack[num];
        num++;
        if (num >= listAttackName.Length)
            num = 0;
        yield return new WaitForSeconds(cooldownTrigger);
        StartCoroutine(ChangeAttack());
    }

    //decide attack to be used based on list
    private void DecideAttack(string attackName)
    {
        switch (attackName)
        {
            case "shooting":
                AttackTypeShooting();
                TriggerChase(false);
                break;
            case "swing":
                AttackTypeSwing();
                TriggerChase(true);
                break;
            case "moveAttack":
                AttackTypeMove();
                break;
            case "teleport":
                AttackTypeTeleport();
                break;
            default:
                break;
        }
    }

    //swing weapon attack type
    private void AttackTypeSwing()
    {
        if (Time.time - lastAttack > cooldownSwing)
        {
            lastAttack = Time.time;
            anim.SetTrigger("Swing");
            PlaySoundEffect(4);
        }
    }

    //move attack type
    private void AttackTypeMove()
    {
        AttackTypeSwing();
        if (Time.time - lastMove > cooldownTrigger)
        {
            lastMove = Time.time;
            // anim.SetTrigger("Move");
            ChangeChaseSpeed(3, 3);
            Debug.Log("moveAttack");
            PlaySoundEffect(5);
        }
    }

    //teleport attack type
    private void AttackTypeTeleport()
    {
        //revert back to original speed 
        // ChangeChaseSpeed(xSpeedOri, ySpeedOri);
        if (isTeleportRandom)
        {
            if (isTeleportOnce)
                TeleportRandom(teleportMinX, teleportMaxX, teleportMinY, teleportMaxY, cooldownTrigger);
            else
                TeleportRandom(teleportMinX, teleportMaxX, teleportMinY, teleportMaxY, cooldownTeleport);
        }
        else
        {
            if (isTeleportOnce)
                TeleportToPlayer(cooldownTrigger);
            else
                TeleportToPlayer(cooldownTeleport);
        }
    }

    //teleport to player 
    private void TeleportToPlayer(float cooldown)
    {
        DecideTypeAttackTeleport();
        if (Time.time - lastTeleport > cooldown)
        {
            lastTeleport = Time.time;
            Teleport(GameManager.instance.player.transform.position);
        }
    }


    //random teleport
    private void TeleportRandom(float minX, float maxX, float minY, float maxY, float cooldown)
    {
        DecideTypeAttackTeleport();
        if (Time.time - lastTeleport > cooldown)
        {
            lastTeleport = Time.time;
            float posX = Random.Range(minX, maxY);
            float posY = Random.Range(minY, maxY);

            Teleport(new Vector3(posX, posY, 1));
        }
    }

    //teleport attack type
    private void Teleport(Vector3 position)
    {
        enemy.transform.position = position;
        PlaySoundEffect(7);
    }
    //decide type attack in teleport
    private void DecideTypeAttackTeleport()
    {
        if (isTeleportWithSwing)
        {
            if (isTeleportChase)
            {
                AttackTypeSwing();
                TriggerChase(true);
            }
            else
                TriggerChase(false);
        }
        else
            AttackTypeShooting();
    }
}
