using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [Tooltip("for player")]
    public Transform lookAt;
    public float boundX = 0.3f;
    public float boundY = 0.15f;

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
    }
    void LateUpdate()
    {
        Vector3 delta = Vector3.zero;
        //for x - center of camera
        float deltaX = lookAt.position.x - transform.position.x;
        //check if bound - prevent out of bound
        if (deltaX > boundX || deltaX < -boundX)
        {
            //Debug.Log("delta" + deltaX + " bound" + boundX);
            //if camera low than player position
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            }
            else
            {
                delta.x = deltaX + boundX;
            }
        }
        //for y - center of camera
        float deltaY = lookAt.position.y - transform.position.y;
        //check if bound
        if (deltaY > boundY || deltaY < -boundY)
        {
            //if camera low than player position
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        //move camera
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
