using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float minZoom = 20f;
    public float maxZoom = 60f;
    public float rotationSpeed = 10f;
    public Animator animator;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleClick();
        HandleZoom();
        HandleRotation();
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Input.mousePosition;
            ProcessClick(clickPosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = touch.position;
                ProcessClick(touchPosition);
            }
        }
    }

    void ProcessClick(Vector3 position)
    {
        if (position.x < Screen.width / 2)
        {
            //animator.SetTrigger("LeftClick");
            Debug.Log("left click");
        }
        else
        {
            //animator.SetTrigger("RightClick");
            Debug.Log("right click");
        }
    }

    void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Zoom(-scroll * 100);
        }
    }

    void Zoom(float increment)
    {
        mainCamera.fieldOfView += increment * zoomSpeed;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;
            Rotate(rotationX, rotationY);
        }

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch = Input.GetTouch(0);
            float rotationX = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
            float rotationY = touch.deltaPosition.y * rotationSpeed * Time.deltaTime;
            Rotate(rotationX, rotationY);
        }
    }

    void Rotate(float rotationX, float rotationY)
    {
        transform.Rotate(Vector3.up, -rotationX, Space.World);
        transform.Rotate(Vector3.right, rotationY, Space.World);
    }
}

