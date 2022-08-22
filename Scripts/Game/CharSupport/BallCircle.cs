using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCircle : MonoBehaviour
{
    public Transform[] balls;
    public float distance = 0.25f;
    public float[] ballSpeeds = { 2.5f, -2.5f };
    public Boss bossScript;

    // Update is called once per frame
    void Update()
    {
        //set paused game  or set stun
        if (GameManager.instance.isPaused || bossScript.isStun)
            return;
        //make ball circle
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * ballSpeeds[i]) * distance, Mathf.Sin(Time.time * ballSpeeds[i]) * distance, 0);
        }
    }
}
