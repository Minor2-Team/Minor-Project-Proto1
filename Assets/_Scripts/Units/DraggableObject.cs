using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DraggableObject : MonoBehaviour
{
    private Camera _cam;

    public bool isDragging;
    private Vector3 _offset;

    public Action<bool> OnDragChange;
    public Action OnMouseDragged;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void StartDrag()
    {
        isDragging = true;
        OnDragChange?.Invoke(isDragging);
        _offset = transform.position - GetMouseWorldPosition();
        
    }

    private void OnMouseDown()
    {
        StartDrag();;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        OnDragChange?.Invoke(isDragging);
    }

    /*
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
    }*/

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _offset;
        OnMouseDragged?.Invoke();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0; // Keep object in 2D space
        return mousePoint;
    }
}
