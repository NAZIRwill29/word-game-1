using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPattern : MonoBehaviour
{
    public Boss bossScript;
    public Enemy enemyScript;
    public AudioSource attackAudio;
    protected float lastAttack, lastTrigger, lastTime;
    [SerializeField]
    protected float cooldownAttack = 1, cooldownTrigger = 10, chaseLength, triggerLength;
    public bool isHasAttackPattern;
    protected bool isCanAttack;
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //set paused game  or set stun
        if (CheckStunAndGamePause())
            return;
        if (isHasAttackPattern)
        {
            //check cooldown -> will activate attack pattern
            //if more than 0 & less than cooldown -> attack
            if ((Time.time - lastTrigger > 0) && (Time.time - lastTrigger < cooldownTrigger))
            {
                isCanAttack = true;
            }
            else
            {
                //if more than cooldown -> stop attack
                // make unable to attack 
                isCanAttack = false;
                if (Time.time - lastTrigger > cooldownTrigger * 2)
                {
                    //if more than 2 x cooldown -> redo again -> attack
                    lastTrigger = Time.time;
                }
            }
            // Debug.Log("time = " + Time.time);
            // Debug.Log("lastTrigger = " + lastTrigger);
        }
        else
            isCanAttack = true;
    }

    protected virtual void TriggerChase(bool isOn)
    {
        //check on or off
        if (!isOn)
            ChangeChaseAndtriggerLength(0, 0);
        else
            ChangeChaseAndtriggerLength(chaseLength, triggerLength);
    }
    protected virtual void ChangeChaseAndtriggerLength(float chaseNum, float triggerNum)
    {
        if (bossScript)
            bossScript.ChangeChaseAndtriggerLength(chaseNum, triggerNum);
        else if (enemyScript)
            enemyScript.ChangeChaseAndtriggerLength(chaseNum, triggerNum);
    }
    //play sound
    protected virtual void PlaySoundEffect(int num)
    {
        if (bossScript)
            attackAudio.PlayOneShot(bossScript.effectSound[num]);
        else if (enemyScript)
            attackAudio.PlayOneShot(enemyScript.effectSound[num]);
    }
    //check stun and game pause
    protected virtual bool CheckStunAndGamePause()
    {
        if (GameManager.instance.isPaused)
            return true;
        if (bossScript)
        {
            if (bossScript.isStun)
                return true;
        }
        else if (enemyScript)
        {
            if (enemyScript.isStun)
                return true;
        }
        return false;
    }
    //change chase speed
    protected virtual void ChangeChaseSpeed(float xSpeed, float ySpeed)
    {
        if (bossScript)
            bossScript.ChangeChaseSpeed(xSpeed, ySpeed);
        else if (enemyScript)
            enemyScript.ChangeChaseSpeed(xSpeed, ySpeed);
    }
}
