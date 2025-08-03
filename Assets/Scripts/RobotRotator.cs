using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRotator : MonoBehaviour
{
    public Transform robotTransform;     // Objeto que se rota (el robot)
    public float rotationSpeed = 3f;     // Sensibilidad del mouse

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private float currentY = 0f;

    void Start()
    {
        currentY = robotTransform.eulerAngles.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            currentY += mouseDelta.x * rotationSpeed * Time.deltaTime;

            robotTransform.rotation = Quaternion.Euler(-90f, currentY, 0f);
            lastMousePosition = Input.mousePosition;
        }
    }
}

