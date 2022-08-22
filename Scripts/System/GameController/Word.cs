using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour
{
    private Char[] charWords;
    private char[] letters;
    private string letterCombine;
    [SerializeField]
    private TextAsset wordList;
    private List<string> words;
    public int totalCharInWord;
    public Keyboard keyboard;
    private GameObject targetChar;
    public Player player;
    public CharObj charObj;
    private int damage;
    void Awake()
    {
        //convert word in .txt to string word
        words = new List<string>(wordList.text.Split(new char[]{
            ',', ' ', '\n', '\r'},
            System.StringSplitOptions.RemoveEmptyEntries
        ));
    }

    // Update is called once per frame
    void Update()
    {

    }

    //increase damage after level up
    public void SetDamage(int dmg)
    {
        damage = dmg;
        charObj.SetDamage(dmg);
    }

    //button B
    //convert char to word for damage enemy
    public void WordAttack()
    {
        //refresh data
        letters = new char[9];
        letterCombine = "";
        //get CHAR script from child
        charWords = GetComponentsInChildren<Char>();
        //Debug.Log(charWords.Length);
        for (int i = 0; i < charWords.Length; i++)
        {
            //get letter from char
            letters[i] = charWords[i].letter;
            //combine letter to be word
            letterCombine += letters[i].ToString();
            charWords[i].AddToBirthchar(true);
        }
        Debug.Log("Word = " + letterCombine);
        Debug.Log(CheckWordExist(letterCombine));
        if (CheckWordExist(letterCombine))
        {
            //calculate damage based on letter in word
            damage *= charWords.Length;
            //get target enemy from keyboard
            targetChar = keyboard.targetChar;
            //shoot word
            charObj.charAnim.SetTrigger("show");
            charObj.ShootWord(targetChar, damage, letterCombine);
            player.PlaySoundWord();
            //increase mana and hp by refer on letter in word
            player.Heal(damage);
        }
        else
        {
            player.PlaySoundFailed();
        }
        //stop word blink
    }

    //check if word exist
    private bool CheckWordExist(string word)
    {
        return words.Contains(word);
    }

    //change number of char in word - for limit
    public void ChangeTotalCharInWord(int number)
    {
        totalCharInWord += number;
        // Debug.Log(totalCharInWord);
    }

    //reset word - send char into birthchar
    public void ResetWord()
    {
        //check exist
        if (!GetComponentInChildren<Char>())
            return;
        charWords = GetComponentsInChildren<Char>();
        foreach (var item in charWords)
        {
            item.AddToBirthchar(true);
        }
        ResetTotalCharInWord();
    }

    //reset number of char in word
    public void ResetTotalCharInWord()
    {
        totalCharInWord = 0;
    }

    //refresh number of char in word - for limit
    // public void RefreshTotalCharInWord()
    // {
    //     totalCharInWord = 0;
    //     // Debug.Log(totalCharInWord);
    // }
}
