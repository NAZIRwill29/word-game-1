using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource backgroundAudio;
    //     0      1       2
    //  normal - boss - clear stage 
    public AudioClip[] backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        backgroundAudio = GetComponent<AudioSource>();
    }
    public void BossMusic()
    {
        ChangeBackgroundMusic(1);
    }
    public void ClearStageMusic()
    {
        ChangeBackgroundMusic(2);
    }
    //change music
    public void ChangeBackgroundMusic(int num)
    {
        backgroundAudio.clip = backgroundMusic[num];
        backgroundAudio.Play();
    }
}
