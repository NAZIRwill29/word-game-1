using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempChar : MonoBehaviour
{
    //add char to word
    public void CharAddToWord(char letter)
    {
        try
        {
            if (gameObject.GetComponentInChildren<Char>())
            {
                Char charScript = gameObject.GetComponentInChildren<Char>();
                charScript.AddToWord(letter);
            }
        }
        catch (System.Exception e)
        {
            //handle error
            Debug.Log(e.Message);
        }
    }

    //delete word if not collide with enemy
    public void CharAddToBirth()
    {
        try
        {
            if (gameObject.GetComponentInChildren<Char>())
            {
                Char charScript = gameObject.GetComponentInChildren<Char>();
                charScript.AddToBirthchar(false);
            }
        }
        catch (System.Exception e)
        {
            //handle error
            Debug.Log(e.Message);
        }
    }
}
