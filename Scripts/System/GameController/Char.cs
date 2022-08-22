using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char : MonoBehaviour
{

    private bool isClick;
    public char letter;
    public GameObject keyboard;
    [SerializeField]
    public GameObject image;
    private Image imageImg;
    private Keyboard keyboardScript;
    private Word wordScript;
    [SerializeField]
    private string special;
    public int idChar;
    // Start is called before the first frame update
    void Start()
    {
        keyboardScript = keyboard.GetComponent<Keyboard>();
        wordScript = keyboardScript.wordObject.GetComponent<Word>();
        imageImg = image.GetComponent<Image>();
    }

    //action after button char click
    public void CharButtonClick()
    {
        if (!isClick)
        {
            //function button in keyboard
            if (wordScript.totalCharInWord < 8)
            {
                //success shoot
                keyboardScript.ButtonClick();
                //shoot charObj
                keyboardScript.CharToShoot(letter, special, idChar);
                keyboardScript.charObj.GetComponent<CharObj>().charAnim.SetTrigger("show");
                AddToTemp();
            }
            else
            {
                //failed shoot
                GameManager.instance.player.PlaySoundFailed();
                //make word blink
                wordScript.gameObject.GetComponent<Animator>().SetInteger("Number", 1);
                StartCoroutine(WordIdle());
            }
        }
        else
        {
            //function button in wordbox
            GameManager.instance.player.PlaySoundReject();
            AddToBirthchar(true);
        }
    }

    //add to keyboard
    public void AddToKeyboard()
    {
        WriteLetter();
        transform.SetParent(keyboard.transform, true);
    }

    //add to temp
    private void AddToTemp()
    {
        transform.SetParent(keyboardScript.tempChar.transform, true);
    }

    //add to word
    public void AddToWord(char letter)
    {
        isClick = true;
        WriteLetter(letter);
        transform.SetParent(keyboardScript.wordObject.transform, true);
        //turn off special char image
        image.SetActive(false);
        //for limit char in word
        wordScript.ChangeTotalCharInWord(1);
    }

    //add to birthChar
    public void AddToBirthchar(bool isParentWord)
    {
        //check if exist
        if (keyboardScript)
            transform.SetParent(keyboardScript.birthChar.transform, true);
        //turn on special char image
        image.SetActive(true);
        //refresh isClick 
        isClick = false;
        if (isParentWord)
            wordScript.ChangeTotalCharInWord(-1);
    }

    //when reset
    public void AddToSpecialChar()
    {
        //turn on special char image
        image.SetActive(true);
        //refresh isClick 
        isClick = false;
        try
        {
            transform.SetParent(keyboardScript.specialCharContainer.transform, true);
        }
        catch (System.Exception e)
        {
            //handle error
            Debug.Log(e.Message);
        }
    }

    //write random letter
    public void WriteLetter()
    {
        letter = keyboardScript.RandomLetter();
        // Debug.Log("LetterChar = " + letter);
        gameObject.GetComponentInChildren<Text>().text = letter.ToString();
    }
    //write letter
    public void WriteLetter(char letter)
    {
        // Debug.Log("LetterCharWord = " + letter);
        gameObject.GetComponentInChildren<Text>().text = letter.ToString();
    }

    //set special char
    // public void SetSpecialChar(string text)
    // {
    //     special = text;
    //     ChangeSpecialCharImage(text);
    // }

    //change source image for specialChar - only when level up
    // public void ChangeSpecialCharImage(string text)
    // {
    //     switch (text)
    //     {
    //         case "thunder":
    //             imageImg.sprite = keyboardScript.specialCharSprites[1];
    //             break;
    //         case "ice":
    //             imageImg.sprite = keyboardScript.specialCharSprites[2];
    //             break;
    //         case "fire":
    //             imageImg.sprite = keyboardScript.specialCharSprites[3];
    //             break;
    //         case "wind":
    //             imageImg.sprite = keyboardScript.specialCharSprites[4];
    //             break;
    //         default:
    //             imageImg.sprite = keyboardScript.specialCharSprites[0];
    //             break;
    //     }
    // }

    //coroutine word idle
    private IEnumerator WordIdle()
    {
        yield return new WaitForSeconds(1);
        wordScript.gameObject.GetComponent<Animator>().SetInteger("Number", 0);

    }
}
