using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DraggableObject : MonoBehaviour
{
    private Camera _cam;

    public bool isDragging;
    public bool withoutCollider;
    private Vector3 _offset;
    public Action<bool> OnDragChange;
    public Action OnMouseDragged;
    public GameObject parent;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void StartDrag()
    {
        isDragging = true;
        OnDragChange?.Invoke(isDragging);
        _offset = transform.position - GetMouseWorldPosition();
        if (withoutCollider)
        {
            StartCoroutine(DragRoutine());
        }
        
    }

    IEnumerator DragRoutine()
    {
        while (isDragging)
        {
            transform.position=Vector2.MoveTowards(transform.position,GetMouseWorldPosition() + _offset,1f);
            if (TryGetComponent(out TransitionNew tar))
            {
                tar.UpdateTransition();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        StartDrag();
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

    private void OnDestroy()
    {
        if (parent)
        {
            
        Destroy(parent);
        }
    }
}
