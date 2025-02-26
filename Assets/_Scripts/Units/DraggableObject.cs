using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DraggableObject : MonoBehaviour
{
    private Camera _cam;
    private Coroutine _dragCoroutine;

    public bool isDragging;
    private Vector3 _offset;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void StartDrag()
    {
        isDragging = true;
        _offset = transform.position - GetMouseWorldPosition();
        
    }

    private void OnMouseDown()
    {
        StartDrag();
        print("mouseDown");
    }

    private void OnMouseUp()
    {
        isDragging = false;
        
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position=Vector2.MoveTowards(transform.position,GetMouseWorldPosition() + _offset,1f);
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0; // Keep object in 2D space
        return mousePoint;
    }
}
