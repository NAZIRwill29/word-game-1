using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform follow;
    public RectTransform healtBar;
    public RectTransform health;
    private Camera mainCamera;
    [SerializeField]
    private float offsetX, offsetY, lengthBar = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        healtBar.localScale = new Vector3(lengthBar, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //check if gameObj exist
        if (!follow)
            return;
        var screenPos = mainCamera.WorldToScreenPoint(follow.position);
        //make it follow gameObj
        transform.position = new Vector3(screenPos.x + offsetX, screenPos.y + offsetY, screenPos.z);
    }
}
