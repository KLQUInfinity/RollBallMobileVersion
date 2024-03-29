﻿using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour
{
    public VirtualJoystick cameraJoystic;

    public Transform lookAt;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensivityX = 3.0f;
    private float sensivityY = 1.0f;

    private void Update()
    {
        currentX += cameraJoystic.InputDirection.x * sensivityX;
        currentY += cameraJoystic.InputDirection.z * sensivityY;
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * dir;
        transform.LookAt(lookAt);
    }
}
