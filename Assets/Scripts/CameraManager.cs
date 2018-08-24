using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour {
    
    const float Y_ANGLE_MIN = 0f;
    const float Y_ANGLE_MAX = 89.9f;
    const float DISTANCE_MIN = 1.5f;
    const float DISTANCE_MAX = 20f;

    public Transform lookAt;
    public float currentX = 0.0f;
    public float currentY = 45.0f;
    public float distance = 10.0f;

    public float sensitivityX = 3.0f;
    public float sensitivityY = 1.0f;
    public float sensitivityZ = 2.0f;

    Transform camTransform;

    public static CameraManager instance;

    void Awake()
    {
        camTransform = transform;
        instance = this;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
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
                distance = Mathf.Clamp(distance, DISTANCE_MIN, DISTANCE_MAX);
            }
        }
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        if (lookAt != null)
        {
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
        }
    }
}
