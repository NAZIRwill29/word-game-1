using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    public GameObject targetObj;
    private bool isAlive = true;
    public AudioClip healSound;
    public int manapoint = 20;
    public int maxManapoint = 20;
    public int damage = 1;
    public int levelPlayer = 1;
    private float lastIncreaseMana;
    private float cooldown = 5;
    public Joystick joystick;
    public Word wordScript;
    public Keyboard keyboardScript;
    public Char[] charSpecialChars;
    public GameObject specialChar;
    private int specialCharNo;
    private Image infoSpecialEffecttImage;
    private Sprite[] specialEffectSprites;
    public GameObject goldKey;
    public GameObject silverKey;
    public bool isHasKey;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        infoSpecialEffecttImage = GameManager.instance.infoSpecialEffecttImage;
        specialEffectSprites = GameManager.instance.specialEffectSprites;
    }
    void Update()
    {
        if (manapoint < maxManapoint)
        {
            //check cooldown
            if (Time.time - lastIncreaseMana > cooldown)
            {
                lastIncreaseMana = Time.time;
                //increase mana
                ChangeMana(1);
                GameManager.instance.OnManapointChange();
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //get input from user for move player
        // float x = Input.GetAxisRaw("Horizontal");
        // float y = Input.GetAxisRaw("Vertical");

        float x = joystick.Horizontal;
        float y = joystick.Vertical;

        if (isAlive)
            //call update motor in mover
            UpdateMotor(new Vector3(x, y, 0), targetObj);
    }
    //swap character
    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }
    //when lvl up
    public void OnLevelUp(bool isSound)
    {
        damage = levelPlayer;
        SetDamage(damage);
        //set special char
        SetSpecialChar();
        //increase hp mp
        int increasePoint = 20;
        for (int i = 0; i < levelPlayer; i++)
        {
            if (levelPlayer >= 5)
                increasePoint += i * 5;
            else if (levelPlayer >= 10)
                increasePoint += i * 9;
            else if (levelPlayer >= 15)
                increasePoint += i * 13;
            else //for lvl < 5
                increasePoint += i * 3;
            //increase hp
            maxHitpoint = increasePoint;
            //increase mp
            maxManapoint = increasePoint;
        }
        hitpoint = maxHitpoint;
        manapoint = maxManapoint;
        //play level up sound  in game play only
        if (isSound)
            fighterAudio.PlayOneShot(effectSound[8], 1.0f);
    }
    public void SetLevelPlayer(int level)
    {
        levelPlayer = level;
    }
    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp(false);
    }

    //set damage
    public void SetDamage(int dmg)
    {
        wordScript.SetDamage(dmg);
    }
    public void SetDamagePlayer(int dmg)
    {
        damage = dmg;
    }

    //set special char in specialChar container
    //level - 3 - thunder -> 7 - ice -> 10 - fire -> 15 - wind
    public void SetSpecialChar()
    {
        if (levelPlayer < 3)
        {
            //below level 3
            return;
        }
        else if (levelPlayer < 7)
        {
            //above lvl 3 & below lvl 7
            CharAddToBirthCharMult(1);
            Debug.Log("thunder");
        }
        else if (levelPlayer < 10)
        {
            //above lvl 7 & below lvl 10
            CharAddToBirthCharMult(2);
            Debug.Log("ice");
        }
        else if (levelPlayer < 15)
        {
            //above lvl 10 & below lvl 15
            CharAddToBirthCharMult(3);
            Debug.Log("fire");
        }
        else if (levelPlayer <= 20)
        {
            //above lvl 15 & below lvl 20
            CharAddToBirthCharMult(4);
            Debug.Log("wind");
        }

    }

    //add char to birthChar multiple time
    private void CharAddToBirthCharMult(int num)
    {
        for (int i = 0; i < num; i++)
        {
            charSpecialChars[i].AddToBirthchar(false);
        }
    }

    //reset special char
    public void ResetSpecialChar()
    {
        keyboardScript.ResetKeyboard();
        wordScript.ResetWord();
        foreach (var item in charSpecialChars)
        {
            //add to specialChar container and setSpecialChar default
            //item.SetSpecialChar("");
            item.AddToSpecialChar();
        }
        Debug.Log("reset special char");
    }

    //heal hp mp player
    public void Heal(int healingAmount)
    {
        //check if maxhealth
        if (hitpoint != maxHitpoint)
            hitpoint += healingAmount;
        if (manapoint != maxManapoint)
            manapoint += healingAmount;
        //prevent from more than maxhealth
        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;
        if (manapoint > maxManapoint)
            manapoint = maxManapoint;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        //change hp
        GameManager.instance.OnHitpointChange();
        GameManager.instance.OnManapointChange();
        //heal sound
        fighterAudio.PlayOneShot(effectSound[3], 4.0f);
        Debug.Log("heal");
    }

    //heal hp
    public void Heal(int healingAmount, bool isHp)
    {
        if (isHp)
        {
            //check if maxhealth
            if (hitpoint != maxHitpoint)
                hitpoint += healingAmount;
            //prevent from more than maxhealth
            if (hitpoint > maxHitpoint)
                hitpoint = maxHitpoint;
            //change hp
            GameManager.instance.OnHitpointChange();
        }
        else
        {
            if (manapoint != maxManapoint)
                manapoint += healingAmount;
            if (manapoint > maxManapoint)
                manapoint = maxManapoint;
            //change mp
            GameManager.instance.OnManapointChange();
        }
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        //heal sound
        fighterAudio.PlayOneShot(effectSound[3], 1.0f);
    }

    // when receive damage
    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
        //make sound effect when collide
        //playerAudio.PlayOneShot(triggerSound, 1.0f);
    }

    //make special effect damage
    protected override void ReceiveSpecialPowerDamage(string specialPower)
    {
        //check exist
        if (!gameObject)
            return;
        if (Time.time - lastSpecialImmuneMover > specialImmuneTime)
        {
            lastSpecialImmuneMover = Time.time;
            //do something
            //Debug.Log(specialPower);
            infoSpecialEffecttImage.gameObject.SetActive(true);
            switch (specialPower)
            {
                case "thunder":
                    //do special power effect
                    ThunderEffect();
                    StartCoroutine(ChangeInfoSpeicalEffectImage(5, 0));
                    break;
                case "ice":
                    //do special power effect
                    IceEffect();
                    StartCoroutine(ChangeInfoSpeicalEffectImage(10, 1));
                    break;
                case "fire":
                    //do special power effect
                    continuousDamage = hitDamageByOther / 5;
                    //make sure has damage
                    if (continuousDamage < 1)
                        continuousDamage = 1;
                    FireEffect();
                    StartCoroutine(ChangeInfoSpeicalEffectImage(10, 2));
                    break;
                case "wind":
                    //do special power effect
                    WindEffect();
                    StartCoroutine(ChangeInfoSpeicalEffectImage(10, 3));
                    break;
                default:
                    break;
            }
        }
    }
    //coroutine change info image
    private IEnumerator ChangeInfoSpeicalEffectImage(float time, int num)
    {
        infoSpecialEffecttImage.sprite = specialEffectSprites[num];
        yield return new WaitForSeconds(time);
        infoSpecialEffecttImage.gameObject.SetActive(false);
    }


    //death
    protected override void Death()
    {
        base.Death();
        effectAnim[2].SetTrigger("Dead");
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        GameManager.instance.inGameUI.GetComponent<CanvasGroup>().alpha = 0;
    }
    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
    //change mana
    public void ChangeMana(int point)
    {
        manapoint += point;
    }
    //play sound
    public void PlaySoundShoot()
    {
        fighterAudio.PlayOneShot(effectSound[4], 1.0f);
    }
    public void PlaySoundReload()
    {
        fighterAudio.PlayOneShot(effectSound[5], 1.0f);
    }
    public void PlaySoundFailed()
    {
        fighterAudio.PlayOneShot(effectSound[6], 1.0f);
    }
    public void PlaySoundWord()
    {
        fighterAudio.PlayOneShot(effectSound[7], 1.0f);
    }
    public void PlaySoundReject()
    {
        fighterAudio.PlayOneShot(effectSound[9], 1.0f);
    }
    public void PlaySoundHitWeapon()
    {
        fighterAudio.PlayOneShot(effectSound[10], 1.0f);
    }
    public void PlaySoundHitBlocking()
    {
        fighterAudio.PlayOneShot(effectSound[11], 1.0f);
    }

    public void GetKey(string keyName)
    {
        isHasKey = true;
        if (keyName == "GoldKey")
        {
            Debug.Log("goldKey");
            goldKey.SetActive(true);
        }
        else if (keyName == "SilverKey")
        {
            silverKey.SetActive(true);
        }
    }

    public void UseKey(string keyName)
    {
        if (keyName == "GoldKey")
        {
            goldKey.SetActive(false);
            DeleteKey();
        }
        else if (keyName == "SilverKey")
        {
            silverKey.SetActive(false);
            DeleteKey();
        }
    }

    private void DeleteKey()
    {
        isHasKey = false;
    }

    public void IncreaseDamagePotion(float time)
    {
        GameManager.instance.ShowText("2X Damage for " + time + "sec", 25, Color.red, transform.position, Vector3.up * 30, 1.0f);
        fighterAudio.PlayOneShot(effectSound[3], 1.0f);
        StartCoroutine(IncreaseDamage(time));
    }

    private IEnumerator IncreaseDamage(float time)
    {
        SetDamage(damage * 2);
        GameManager.instance.infoStatusImage.gameObject.SetActive(true);
        //change info status image
        ChangeInfoStatusImage(0);
        Debug.Log("status active");
        //wait for second
        yield return new WaitForSeconds(time);
        Debug.Log("status inactive");
        GameManager.instance.infoStatusImage.gameObject.SetActive(false);
        SetDamage(damage);
    }

    //coroutine change info status image
    private void ChangeInfoStatusImage(int num)
    {
        GameManager.instance.infoStatusImage.sprite = GameManager.instance.statusSprites[num];
    }


}
