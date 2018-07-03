using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform camTransform;
    public float distance = 10.0f;

    private float currentX = 0.0f;
    private float currentY = 45.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;

    bool one_click = false;
    bool timer_running;
    float timer_for_double_click;
    float delay = .25f;

    private void Start()
    {
        camTransform = transform;
    }

    private void Update()
    {
        lookAt = GameManager.instance.selectedEntity.transform;
        if(Input.GetMouseButtonDown(0))
        {
            if(!one_click) // first click no previous clicks
            {
                one_click = true;

                timer_for_double_click = Time.time; // save the current time
                // do one click things;
            } 
            else
            {
                one_click = false; // found a double click, now reset
                Debug.Log("Double click");
                // TODO double click things
            }
        }
        if (one_click)
        {
            if ((Time.time - timer_for_double_click) > delay)
            {
                one_click = false;
            }
        }


        if (Input.GetMouseButton(1)) 
        {
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
