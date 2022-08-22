using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharObj : MonoBehaviour
{
    public float speed = 3.0f;
    private Rigidbody2D charRb;
    private SpriteRenderer charSR;
    [SerializeField]
    private SpriteRenderer charSpecialSR;
    public GameObject keyboard;
    public GameObject tempChar;
    public Player playerScript;
    public char letter;
    public int damage;
    private int damageCombine;
    [SerializeField]
    private float pushForce = 3;
    public Animator charAnim;
    private bool isWordCombine, isCharObjWordCombHit;
    public Sprite[] charSprites;
    private Vector3 originalPosition;
    public string word;
    public string specialPower;
    // Start is called before the first frame update
    void Start()
    {
        charRb = GetComponent<Rigidbody2D>();
        charSR = GetComponent<SpriteRenderer>();
        //tempChar = GameObject.Find("TempChar");
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //increase damage after level up
    public void SetDamage(int dmg)
    {
        damage = dmg;
        Debug.Log("damage = " + damage);
    }

    //shoot word
    public void ShootWord(GameObject targetChar, char letterClick, string specialText, int id)
    {
        //set to player position
        transform.position = GameManager.instance.player.gameObject.transform.position;
        // //set to target rotation
        // transform.rotation = Quaternion.Euler(0, 0, targetChar.transform.rotation.z);
        //get letter from char
        letter = letterClick;
        //refresh word - mkae it empty
        word = "";
        // Debug.Log("LetterCharObj = " + letter);
        //turn on SpriteRendere
        charSR.enabled = true;
        //specialText - make special power based on specialText
        SetSprecialPower(specialText);
        charRb.AddForce((targetChar.transform.position - transform.position).normalized * speed, ForceMode2D.Impulse);
        playerScript.PlaySoundShoot();
        StartCoroutine(StopCharObj(id));
    }

    //shoot combine word
    public void ShootWord(GameObject targetChar, int damageWord, string letterCombine)
    {
        transform.position = GameManager.instance.player.gameObject.transform.position;
        // transform.rotation = targetChar.transform.rotation;
        //turn on SpriteRendere
        charSR.enabled = true;
        isWordCombine = true;
        // Debug.Log(isWordCombine);
        damageCombine = damageWord;
        Debug.Log("damage = " + damageCombine);
        //do someting about word - for boss
        word = letterCombine;
        charRb.AddForce((targetChar.transform.position - transform.position).normalized * speed, ForceMode2D.Impulse);
        playerScript.PlaySoundShoot();
        StartCoroutine(StopCharObj());
    }

    //stop charObj movement on combine word
    private IEnumerator StopCharObj()
    {
        yield return new WaitForSeconds(1f);
        if (!isCharObjWordCombHit)
        {
            AfterCollide();
            tempChar.GetComponent<TempChar>().CharAddToBirth();
            Debug.Log("stop eror charObj");
        }
    }

    //stop charObj movement on single letter
    private IEnumerator StopCharObj(int id)
    {
        yield return new WaitForSeconds(1f);
        //check if char exist
        if (tempChar.GetComponentInChildren<Char>())
        {
            //check if char is same id
            if (tempChar.GetComponentInChildren<Char>().idChar == id)
            {
                AfterCollide();
                tempChar.GetComponent<TempChar>().CharAddToBirth();
                Debug.Log("stop eror charObj");
            }
        }
    }

    //method trigger when collide
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fighter")
        {
            if (other.name != "Player")
            {
                if (isWordCombine)
                {
                    //for stop charobj word combine
                    isCharObjWordCombHit = false;
                    // Debug.Log("wordcombine");
                    AfterCollide(other, damageCombine);
                }
                else
                {
                    AfterCollide(other, damage);
                    //move char to word
                    tempChar.GetComponent<TempChar>().CharAddToWord(letter);
                }
            }
        }
        else if (other.tag == "Destructable")
        {
            if (isWordCombine)
            {
                //for stop charobj word combine
                isCharObjWordCombHit = false;
                AfterCollide(other, damageCombine);
            }
            else
            {
                AfterCollide(other, damage);
                tempChar.GetComponent<TempChar>().CharAddToBirth();
            }

        }
        else if (other.tag == "Weapon")
        {
            //for stop charobj word combine
            isCharObjWordCombHit = false;
            Debug.Log("Weapon");
            GameManager.instance.player.PlaySoundHitWeapon();
            AfterCollide();
            tempChar.GetComponent<TempChar>().CharAddToBirth();
        }
        else if (other.tag == "Blocking")
        {
            //for stop charobj word combine
            isCharObjWordCombHit = false;
            Debug.Log("none");
            GameManager.instance.player.PlaySoundHitBlocking();
            AfterCollide();
            tempChar.GetComponent<TempChar>().CharAddToBirth();
        }
    }

    //after collide without damage
    private void AfterCollide()
    {
        //hide charObj
        charSR.enabled = false;
        // Debug.Log("trigger");
        //stop force
        charRb.velocity = Vector2.zero;
        //make to original posistion
        transform.position = originalPosition;
        charAnim.SetTrigger("hide");
        //enable keyboard interaction
        keyboard.GetComponent<CanvasGroup>().interactable = true;
        keyboard.GetComponent<CanvasGroup>().alpha = 1;
        if (isWordCombine)
            isWordCombine = false;
        //for stop charobj word combine
        isCharObjWordCombHit = true;
    }
    //after collide with damage
    private void AfterCollide(Collider2D other, int damage)
    {
        // Debug.Log(damage);
        //Create new dmg obj b4 send to enemy
        Damage dmg = new Damage
        {
            damageAmount = damage,
            origin = transform.position,
            pushForce = pushForce
        };
        //normal
        //send message to other to make call ReceiveDamage function
        other.SendMessage("ReceiveDamage", dmg);
        //pass special power in damage
        if (specialPower != "")
            SendSpecialDamage(other);
        //hide charObj
        charSR.enabled = false;
        // Debug.Log("trigger");
        //stop force
        charRb.velocity = Vector2.zero;
        //make to original posistion
        transform.position = originalPosition;
        charAnim.SetTrigger("hide");
        //enable keyboard interaction
        keyboard.GetComponent<CanvasGroup>().interactable = true;
        keyboard.GetComponent<CanvasGroup>().alpha = 1;
        if (isWordCombine)
            isWordCombine = false;
        //for stop charobj word combine
        isCharObjWordCombHit = true;
    }

    //set special power - when attack
    public void SetSprecialPower(string text)
    {
        specialPower = text;
        switch (text)
        {
            case "thunder":
                charSpecialSR.sprite = charSprites[1];
                //do special power image - cange charSprites image
                break;
            case "ice":
                charSpecialSR.sprite = charSprites[2];
                //do special power image
                break;
            case "fire":
                charSpecialSR.sprite = charSprites[3];
                //do special power image
                break;
            case "wind":
                charSpecialSR.sprite = charSprites[4];
                //do special power image
                break;
            default:
                charSpecialSR.sprite = charSprites[0];
                break;
        }
    }
    //send special damage to player
    private void SendSpecialDamage(Collider2D coll)
    {
        //send damage value to player
        if (coll.GetComponent<Enemy>())
            coll.GetComponent<Enemy>().ChangeEnemyDamage(damage);
        else if (coll.GetComponent<Boss>())
            coll.GetComponent<Boss>().ChangeEnemyDamage(damage);
        coll.SendMessage("ReceiveSpecialPowerDamage", specialPower);
    }
}
