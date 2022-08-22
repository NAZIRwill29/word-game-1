using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //isBossSpecialWord - for boss only damaged with word only
    [SerializeField]
    private bool isEndStage, isBossSpecialWord;
    public InGame inGameScript;
    [SerializeField]
    private TextAsset wordList;
    private List<string> words;
    // private bool isCanMove = true;
    private void Awake()
    {
        if (isBossSpecialWord)
        {
            //convert word in .txt to string word
            words = new List<string>(wordList.text.Split(new char[]{
            ',', ' ', '\n', '\r'},
                System.StringSplitOptions.RemoveEmptyEntries
            ));
        }

    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //make only take damage when word is exist in it library
    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isBossSpecialWord)
        {
            base.ReceiveDamage(dmg);
        }
        else
        {
            //check if word exist then can take damage - only for boss word
            if (!CheckWordExist(charObjScript.word))
                return;
            if (Time.time - lastImmuneMover > immuneTime)
            {
                lastImmuneMover = Time.time;
                //make sound effect when collide
                fighterAudio.PlayOneShot(effectSound[1], 1.0f);
                //Debug.Log("play sound");

                hitpoint -= dmg.damageAmount;
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 15, Color.red, transform.position, Vector3.zero, 0.5f);

                //check to make death
                if (hitpoint <= 0)
                {
                    hitpoint = 0;
                    Death();
                }

            }
        }
    }

    //check if word exist
    private bool CheckWordExist(string word)
    {
        return words.Contains(word);
    }

    protected override void Death()
    {
        base.Death();
        if (isEndStage)
            inGameScript.PassStagePassed();

    }
}
