using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    //spawn item at fighter location
    public void SpawnItem(Transform FighterTm)
    {
        gameObject.SetActive(true);
        transform.position = FighterTm.position;
    }
}
