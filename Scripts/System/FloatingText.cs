using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;
    //show text
    public void Show()
    {
        active = true;
        //time at the present
        lastShown = Time.time;
        go.SetActive(active);
    }

    //hide text
    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;
        //if show is more than duration
        if (Time.time - lastShown > duration)
            Hide();
        go.transform.position += motion * Time.deltaTime;
    }
}
