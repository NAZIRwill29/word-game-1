using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    //close - open
    public Sprite[] ObjSprites;
    private SpriteRenderer ObjSR;
    public Animator ObjAnim;
    private AudioSource ObjAudio;
    public AudioClip[] objMusic;
    // Start is called before the first frame update
    void Start()
    {
        ObjSR = GetComponent<SpriteRenderer>();
        ObjAudio = GetComponent<AudioSource>();
    }

    public void ChangeImage(int num)
    {
        ObjSR.sprite = ObjSprites[num];
        ObjAudio.PlayOneShot(objMusic[num], 1.0f);
    }
    public void SetAnimation(string trigger, int num)
    {
        ObjAnim.SetTrigger(trigger);
        ObjAudio.PlayOneShot(objMusic[num], 1.0f);
    }
}
