using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    //for filter contact
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    //for what hit it
    private Collider2D[] hits = new Collider2D[10];
    public AudioClip triggerSound;
    protected AudioSource collidableAudio;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        collidableAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //look for overlap collider
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            //continue code
            if (hits[i] == null)
                continue;
            OnCollide(hits[i]);
            //clean array
            hits[i] = null;
        }
    }
    //function when collide
    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in " + this.name);
    }

}
