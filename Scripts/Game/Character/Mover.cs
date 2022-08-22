using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    //movement speed
    public float ySpeed = 0.7f;
    public float xSpeed = 1.0f;
    private Vector3 originalSize;
    protected float lastImmuneMover;
    //for special power immune
    protected float lastSpecialImmuneMover;
    protected float specialImmuneTime = 10;
    private float rotationZ = 0;
    private float prevRotationZ;
    //for stun
    public bool isStun = false;
    protected int continuousDamage;
    protected CharObj charObjScript;
    //thunder, ice, fire, wind
    public GameObject[] specialEffect;
    // public AudioClip[] specialEffectSound;
    protected int hitDamageByOther;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = transform.localScale;
        fighterAudio = GetComponent<AudioSource>();
        charObjScript = GameObject.Find("CharObj").GetComponent<CharObj>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //set paused game  or set stun
        if (GameManager.instance.isPaused || isStun)
            return;
        //for reference move player
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //make player face direction it walk
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        //add push vector, if any
        moveDelta += pushDirection;
        //Reduce pushForce evert frame, based off receovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //prevent player overlap with NPC and Wall - block player
        //for make hit y
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //check if hit any object
        if (hit.collider == null)
        {
            //move player in y
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //for make hit x
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //check if hit any object
        if (hit.collider == null)
        {
            //move player in x
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
    //update motor with target
    protected virtual void UpdateMotor(Vector3 input, GameObject targetObj)
    {
        //set paused game  or set stun
        if (GameManager.instance.isPaused || isStun)
            return;
        float xSpeedTemp = 0;
        float ySpeedTemp = 0;
        float restrictMove = .2f;
        //restrict move
        if (input.x >= restrictMove)
            xSpeedTemp = xSpeed;
        else if (input.x <= -restrictMove)
            xSpeedTemp = xSpeed;
        else if (input.x < restrictMove && input.x > -restrictMove)
            xSpeedTemp = 0;

        if (input.y >= restrictMove)
            ySpeedTemp = ySpeed;
        else if (input.y <= -restrictMove)
            ySpeedTemp = ySpeed;
        else if (input.y < restrictMove && input.y > -restrictMove)
            ySpeedTemp = 0;
        //for reference move player
        moveDelta = new Vector3(input.x * xSpeedTemp, input.y * ySpeedTemp, 0);


        //make player face direction it walk
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        //add push vector, if any
        moveDelta += pushDirection;
        //Reduce pushForce evert frame, based off receovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //prevent player overlap with NPC and Wall - block player
        //for make hit y
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //check if hit any object
        if (hit.collider == null)
        {
            //move player in y
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //for make hit x
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //check if hit any object
        if (hit.collider == null)
        {
            //move player in x
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }

        //rotate target
        //Quaternion target = Quaternion.Euler(0, 0, GetAngleRotation(input.x, input.y));
        // Debug.Log(GetAngleRotation(transform.position.x, transform.position.y));
        prevRotationZ = rotationZ;
        rotationZ = GetAngleRotation(input.x, input.y);
        //Quaternion target = Quaternion.Euler(0, 0, rotationZ);
        //move target object
        // targetObj.transform.rotation = Quaternion.Slerp(targetObj.transform.rotation, target, 1);
        targetObj.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    //change dist x,y to angle rotation
    private float GetAngleRotation(float x, float y)
    {
        float angle;
        angle = Mathf.Atan(x / y) * (180 / Mathf.PI);
        //Debug.Log("angle =" + angle);
        if (x > 0 && y > 0)
            return 360 - angle;
        else if (x == 1 && y == 0)
            return 270;
        else if (x > 0 && y < 0)
            return 360 - (180 + angle);
        else if (x == 0 && y == -1)
            return 180;
        else if (x < 0 && y < 0)
            return 360 - (180 + angle);
        else if (x == -1 && y == 0)
            return 90;
        else if (x < 0 && y > 0)
            return 360 - (360 + angle);
        else if (x == 0 && y == 1)
            return 0;
        return prevRotationZ;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmuneMover > immuneTime)
        {
            lastImmuneMover = Time.time;
            //make sound effect when collide
            fighterAudio.PlayOneShot(effectSound[1], 0.75f);
            //Debug.Log("play sound");
            base.ReceiveDamage(dmg);
        }

    }

    //make special effect damage
    protected virtual void ReceiveSpecialPowerDamage(string specialPower)
    {
        //check exist
        if (!gameObject)
            return;
        if (Time.time - lastSpecialImmuneMover > specialImmuneTime)
        {
            lastSpecialImmuneMover = Time.time;
            //do something
            //Debug.Log(specialPower);
            switch (specialPower)
            {
                case "thunder":
                    //do special power effect
                    ThunderEffect();
                    break;
                case "ice":
                    //do special power effect
                    IceEffect();
                    break;
                case "fire":
                    //do special power effect
                    continuousDamage = hitDamageByOther / 5;
                    //make sure has damage
                    if (continuousDamage < 1)
                        continuousDamage = 1;
                    FireEffect();
                    break;
                case "wind":
                    //do special power effect
                    WindEffect();
                    break;
                default:
                    break;
            }
        }
    }

    public void ChangeEnemyDamage(int damage)
    {
        hitDamageByOther = damage;
    }

    //thunder
    protected void ThunderEffect()
    {
        specialEffect[0].SetActive(true);
        fighterAudio.PlayOneShot(GameManager.instance.specialEffectSound[0], 1.0f);
        //stun for 5 sec
        StartCoroutine(SetStun(5, specialEffect[0]));
    }
    //ice
    protected void IceEffect()
    {
        specialEffect[1].SetActive(true);
        fighterAudio.PlayOneShot(GameManager.instance.specialEffectSound[1], 1.0f);
        //slow move - lower the speed for 10 sec - speed - (speed/3)
        StartCoroutine(SetSpeed(10, -3, specialEffect[1]));
    }
    //fire
    protected void FireEffect()
    {
        specialEffect[2].SetActive(true);
        fighterAudio.PlayOneShot(GameManager.instance.specialEffectSound[2], 1.0f);
        //continuous hit for 10 sec - damage/5 per 1 sec
        StartCoroutine(SetContinuosHit(10, specialEffect[2]));
    }
    //wind
    protected void WindEffect()
    {
        specialEffect[3].SetActive(true);
        fighterAudio.PlayOneShot(GameManager.instance.specialEffectSound[3], 1.0f);
        //increase speed for 10 sec - speed + (speed/3)
        StartCoroutine(SetSpeed(10, 3, specialEffect[3]));
        StartCoroutine(SetContinuosHit(10, 2));
    }

    //coroutine stun
    private IEnumerator SetStun(float time, GameObject effect)
    {
        isStun = true;
        yield return new WaitForSeconds(time);
        isStun = false;
        effect.SetActive(false);
    }
    //coruotine set Speed move
    private IEnumerator SetSpeed(float time, float num, GameObject effect)
    {
        float prevXSpeed, prevYSpeed;
        prevXSpeed = xSpeed;
        prevYSpeed = ySpeed;
        xSpeed = xSpeed + xSpeed / num;
        ySpeed = ySpeed + ySpeed / num;
        yield return new WaitForSeconds(time);
        xSpeed = prevXSpeed;
        ySpeed = prevYSpeed;
        effect.SetActive(false);
    }
    //coroutine continuous damage
    private IEnumerator SetContinuosHit(float time, int multDamage)
    {
        continuousDamage *= multDamage;
        //receiveDamage
        InvokeRepeating("CallReceiveDamage", 1, 1);
        yield return new WaitForSeconds(time);
        CancelInvoke("CallReceiveDamage");
    }
    private IEnumerator SetContinuosHit(float time, GameObject effect)
    {
        //receiveDamage
        InvokeRepeating("CallReceiveDamage", 1, 1);
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        CancelInvoke("CallReceiveDamage");
    }
    //simple call receivedamage - for used by invoke
    private void CallReceiveDamage()
    {
        //Create new dmg obj b4 send to enemy
        Damage dmg = new Damage
        {
            damageAmount = continuousDamage,
            origin = transform.position,
            pushForce = 0
        };
        ReceiveDamage(dmg);
    }

    //change chase speed
    public void ChangeChaseSpeed(float xNum, float yNum)
    {
        xSpeed = xNum;
        ySpeed = yNum;
    }
}
