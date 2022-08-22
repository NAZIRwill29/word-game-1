using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update()
    {
        //for each txt will call method
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        //change text in txt
        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        //WorldToScreenPoint - transfer world space to screen space so can use in UI
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;
        //call show text method
        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        //find t that not active in floatingtexts
        FloatingText txt = floatingTexts.Find(t => !t.active);
        //check if t is null
        if (txt == null)
        {
            //set txt
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();
            floatingTexts.Add(txt);
        }
        return txt;
    }
}
