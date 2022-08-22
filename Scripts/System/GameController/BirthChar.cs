using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthChar : MonoBehaviour
{
    public Player player;
    public Animator KeyboardAnimator;
    private Char[] chars;
    private int totalCharInKeyboard = 0;
    private float cooldown = 0.3f;
    private float lastSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // transfer char to keyboard - button A
    public void CharToKeyboard()
    {
        //if none mana
        if (player.manapoint <= 0)
            return;
        //check cooldown
        if (Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;
            if (totalCharInKeyboard < 16)
            {
                //success add char
                chars = GetComponentsInChildren<Char>();
                //spawn object under parent
                //Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
                //Instantiate(charPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                chars[Random.Range(0, chars.Length)].AddToKeyboard();
                totalCharInKeyboard += 1;
                player.PlaySoundReload();
                // Debug.Log(totalCharInKeyboard);
                //decrease mana
                player.ChangeMana(-1);
                GameManager.instance.OnManapointChange();
            }
            else
            {
                //failed add char
                player.PlaySoundFailed();
                //make keyboard blink
                KeyboardAnimator.SetInteger("Number", 1);
                StartCoroutine(KeyboardIdle());
            }
        }

    }

    public void MinusCharNoInKeyboard()
    {
        totalCharInKeyboard -= 1;
        // Debug.Log(totalCharInKeyboard);
    }

    public void ResetCharNoInKeyboard()
    {
        totalCharInKeyboard = 0;
    }

    //coroutine keyboard idle
    private IEnumerator KeyboardIdle()
    {
        yield return new WaitForSeconds(1);
        KeyboardAnimator.SetInteger("Number", 0);

    }
}
