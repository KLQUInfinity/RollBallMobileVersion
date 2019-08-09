using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public RectTransform virtualJoystickSpace;

    private Vector3 desiredPostion;
    private Vector3 offset;

    private Vector2 touchPosition;
    private float swipeResistance = 200.0f;

    private float smoothSpeed = 7.5f;
    private float distance = 5.0f;
    private float yOffset = 3.5f;

    private float startTime = 0;
    private bool isInsideVirtualJoystickSpace = false;

    private void Start()
    {
        offset = new Vector3(0, yOffset, -1f * distance);
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - startTime < 2.5f)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SlideCamera(true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SlideCamera(false);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(virtualJoystickSpace, Input.mousePosition))
            {
                isInsideVirtualJoystickSpace = true;
            }
            else
            {
                touchPosition = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            if (isInsideVirtualJoystickSpace)
            {
                isInsideVirtualJoystickSpace = false;
                return;
            }

            float swipeForce = touchPosition.x - Input.mousePosition.x;
            if (Mathf.Abs(swipeForce) > swipeResistance)
            {
                if (swipeForce < 0)
                {
                    SlideCamera(true);
                }
                else if (swipeForce > 0)
                {
                    SlideCamera(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - startTime < 2.5f)
        {
            return;
        }

        desiredPostion = lookAt.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed * Time.deltaTime);
        transform.LookAt(lookAt.position + Vector3.up);
    }

    public void SlideCamera(bool left)
    {
        if (left)
        {
            offset = Quaternion.Euler(0, 90, 0) * offset;
        }
        else
        {
            offset = Quaternion.Euler(0, -90, 0) * offset;
        }
    }
}
