using System;
using UnityEngine;
using _Scripts.Units;

public class TransitionNew : MonoBehaviour
{
    [SerializeField] private char transitionSymbol;
    [SerializeField] public State fromState;
    [SerializeField] public State toState;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform arrowTransform;
    [SerializeField] public Transform fromTransform;
    [SerializeField] public Transform toTransform;
    [SerializeField] private Transform labelTransform;
    [SerializeField] public bool isSelected;

    private const float Radius = 1.5f;

    private void Start()
    {
        InitialSetup();
        
        SubscribeDragEvents(fromTransform, OnFromDragChanged);
        SubscribeDragEvents(toTransform, OnToDragChanged);
        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }
    

    private void InitialSetup()
    {
        if (!fromState)
        {
            fromTransform.GetComponent<DraggableObject>().OnMouseDragged += UpdateVisuals;
        }
        if(!toState)
        {
            toTransform.GetComponent<DraggableObject>().OnMouseDragged += UpdateVisuals;
        }
    }

    private void SubscribeDragEvents(Transform target, Action<bool> dragCallback)
    {
        target.GetComponent<DraggableObject>().OnDragChange += dragCallback;
    }

    private void OnFromDragChanged(bool isDragging)
    {
        var transitionChange = fromTransform.GetComponent<TransitionChange>();

        if (isDragging)
            transitionChange.OnAnyCollision += HandleFromStateChange;
        else
            transitionChange.OnAnyCollision -= HandleFromStateChange;

        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }

    private void OnToDragChanged(bool isDragging)
    {
        var transitionChange = toTransform.GetComponent<TransitionChange>();

        if (isDragging)
            transitionChange.OnAnyCollision += HandleToStateChange;
        else
            transitionChange.OnAnyCollision -= HandleToStateChange;

        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }

    public void UpdateTransition()
    {
        if (fromState)
        {
            fromTransform.position = fromState.transform.position;
            fromTransform.position += (toTransform.position - fromTransform.position).normalized * Radius;

            if (!fromState.transitions.ContainsKey(transitionSymbol))
                fromState.transitions.Add(transitionSymbol, this);
        }

        if (toState)
        {
            toTransform.position = toState.transform.position;
            toTransform.position += (fromTransform.position - toTransform.position).normalized * Radius;

            if (!toState.transitionsto.ContainsKey(transitionSymbol))
                toState.transitionsto.Add(transitionSymbol, this);
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (fromState == null && toState != null && fromTransform.GetComponent<DraggableObject>().isDragging)
        {
            toTransform.position = toState.transform.position;
            toTransform.position += (fromTransform.position - toTransform.position).normalized * Radius;
        }

        if (toState == null && fromState != null && toTransform.GetComponent<DraggableObject>().isDragging)
        {
            fromTransform.position = fromState.transform.position;
            fromTransform.position += (toTransform.position - fromTransform.position).normalized * Radius;
        }

        Vector3 start = fromTransform.position;
        Vector3 end = toTransform.position;
        Vector3 direction = (end - start).normalized;

        lineRenderer.SetPosition(0, start);
        arrowTransform.position = end;
        arrowTransform.right = direction;
        lineRenderer.SetPosition(1, arrowTransform.position);

        Vector3 midPoint = (start + end) * 0.5f;
        labelTransform.position = midPoint + Vector3.up * 0.5f;

        Vector3 labelDir = arrowTransform.position - direction * Radius - start;
        if (labelDir.x < 0)
            labelDir = -labelDir;

        labelTransform.right = labelDir.normalized;
    }

    private void HandleFromStateChange(State state, bool isEntering)
    {
        if (isEntering)
        {
            fromState = state;
            fromTransform.GetComponent<DraggableObject>().OnMouseDragged -= UpdateVisuals;
        }
        else
        {
            fromState.transitions.Remove(transitionSymbol);
            fromState = null;
            fromTransform.GetComponent<DraggableObject>().OnMouseDragged += UpdateVisuals;
        }

        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }

    private void HandleToStateChange(State state, bool isEntering)
    {
        if (isEntering)
        {
            toState = state;
            toTransform.GetComponent<DraggableObject>().OnMouseDragged -= UpdateVisuals;
        }
        else
        {
            toState = null;
            toTransform.GetComponent<DraggableObject>().OnMouseDragged += UpdateVisuals;
        }

        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }
}
