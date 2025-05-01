using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Scripts.Units;

public class TransitionNew : MonoBehaviour
{
    [SerializeField] private List<char> transitionSymbols = new();
    [SerializeField] public State fromState;
    [SerializeField] public State toState;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform arrowTransform;
    [SerializeField] public Transform fromTransform;
    [SerializeField] public Transform toTransform;
    [SerializeField] private Transform labelTransform;
    [SerializeField] private SingleCharacterInputField singleCharacterInputField;
    [SerializeField] public bool isSelected;

    private const float Radius = 1.5f;

    private void Start()
    {
        if (singleCharacterInputField)
        {
            singleCharacterInputField.OnCharacterChanged += HandleCharacterChanged;
        }
        
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

            /*if (!fromState.transitions.ContainsKey(transitionSymbol))
                fromState.transitions.Add(transitionSymbol, this);*/

            foreach (var transitionSymbol in transitionSymbols)
            {
                if (!fromState.transitions.ContainsKey(transitionSymbol))
                {
                    
                    fromState.transitions.Add(transitionSymbol, this);
                }
            }
        }

        if (toState)
        {
            toTransform.position = toState.transform.position;
            toTransform.position += (fromTransform.position - toTransform.position).normalized * Radius;

            /*if (!toState.transitionsto.ContainsKey(transitionSymbol))
                toState.transitionsto.Add(transitionSymbol, this);*/
            foreach (var transitionSymbol in transitionSymbols)
            {
                if (!toState.transitionsto.Contains(this))
                {
                    
                    /*toState.transitionsto.Add(transitionSymbol, this);*/
                    toState.transitionsto.Add(this);
                }
            }
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (!fromState && toState && fromTransform.GetComponent<DraggableObject>().isDragging)
        {
            toTransform.position = toState.transform.position;
            toTransform.position += (fromTransform.position - toTransform.position).normalized * Radius;
        }

        if (!toState && fromState && toTransform.GetComponent<DraggableObject>().isDragging)
        {
            fromTransform.position = fromState.transform.position;
            fromTransform.position += (toTransform.position - fromTransform.position).normalized * Radius;
        }

        if (fromState && toState && fromState == toState)//self loop
        {
            int loopPoints = 20;
            lineRenderer.positionCount = loopPoints + 1;

            float loopRadius = 1f; 
            Vector3 center = fromTransform.position + Vector3.up * loopRadius; 

            for (int i = 0; i <= loopPoints; i++)
            {
                float angle = 2 * Mathf.PI * i / loopPoints;
                float x = Mathf.Cos(angle) * loopRadius;
                float y = Mathf.Sin(angle) * loopRadius;
                lineRenderer.SetPosition(i, center + new Vector3(x, y, 0));
            }

            arrowTransform.position = lineRenderer.GetPosition(loopPoints / 2);
            Vector3 dir = lineRenderer.GetPosition(loopPoints / 2 + 1) - lineRenderer.GetPosition(loopPoints / 2 - 1);
            arrowTransform.right = dir.normalized;

            labelTransform.position = center ;
            labelTransform.right = Vector3.right;
        }
        else
        {
            lineRenderer.positionCount = 2;
            Vector3 start = fromTransform.position;
            Vector3 end = toTransform.position;
            Vector3 direction = (end - start).normalized;

            lineRenderer.SetPosition(0, start);
            arrowTransform.position = new(end.x,end.y,arrowTransform.position.z);
            arrowTransform.right = direction;
            lineRenderer.SetPosition(1, arrowTransform.position);

            Vector3 midPoint = (start + end) * 0.5f;
            labelTransform.position = midPoint ;

            Vector3 labelDir = direction;
            if (labelDir.x < 0)
                labelDir = -labelDir;

            labelTransform.right = labelDir.normalized;
            labelTransform.position += labelTransform.up * 0.5f;
        }
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
            /*fromState.transitions.Remove(transitionSymbol);*/
            foreach (var transitionSymbol in transitionSymbols)
            {
                fromState.transitions.Remove(transitionSymbol);
            }
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
            /*toState.transitionsto.Remove(transitionSymbol);*/
            foreach (var transitionSymbol in transitionSymbols)
            {
                toState.transitionsto.Remove(this);
            }
            toState = null;
            toTransform.GetComponent<DraggableObject>().OnMouseDragged += UpdateVisuals;
        }

        UpdateTransition();
        UpdateVisuals();
        UpdateTransition();
        UpdateVisuals();
    }
    private void HandleCharacterChanged(bool added,char character)
    {
        if(added)
        {
            transitionSymbols.Add(character);
            if(fromState)
                fromState.transitions.Add(character, this);
        }
        else
        {
            transitionSymbols.Remove(character);
            if(fromState)
                fromState.transitions.Remove(character);
        }
        
        
        
        /*if (transitionSymbol != character)
        {
            if (fromState)
            {
                fromState.transitions.Remove(transitionSymbol);
                
            }
        }
        transitionSymbol = character;
        if (fromState && !fromState.transitions.ContainsKey(transitionSymbol))
        {
            fromState.transitions.Add(transitionSymbol, this);
        }*/
            
    }

    private void OnDestroy()
    {
        
        if (fromState)
        {
            /*fromState.transitions.Remove(transitionSymbol);*/
            foreach (var transitionSymbol in transitionSymbols)
            {
                fromState.transitions.Remove(transitionSymbol);
            }
        }
        if (toState)
        {
            /*toState.transitionsto.Remove(transitionSymbol);*/
            foreach (var transitionSymbol in transitionSymbols)
            {
                toState.transitionsto.Remove(this);
            }
        }
        if (singleCharacterInputField)
        {
            singleCharacterInputField.OnCharacterChanged -= HandleCharacterChanged;
        }
    }
}
