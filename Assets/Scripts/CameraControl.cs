using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 89.9f;

    public Transform lookAt;
    public Transform camTransform;
    public float distance = 10.0f;

    public float currentX = 0.0f;
    public float currentY = 45.0f;
    public float sensitivityX = 3.0f;
    public float sensitivityY = 1.0f;
    public float sensitivityZ = 2.0f;

    public static CameraControl instance;

    private void Awake()
    {
        camTransform = transform;
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityZ;
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
