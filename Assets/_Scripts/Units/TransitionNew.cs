using System;
using _Scripts.Units;
using TMPro;
using UnityEngine;

public class TransitionNew : MonoBehaviour
{
    [SerializeField] private char stringCondition;
    [SerializeField]public State from;
    [SerializeField]public State to;
    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField] private Transform arrow;
    [SerializeField] public Transform fromPos;
    [SerializeField] public Transform toPos;
    [SerializeField] private Transform textLabel;
    [SerializeField] public bool isSelected;



    private void Start()
    {
        fromPos.GetComponent<DraggableObject>().OnDragChange += FromTrackStateChange;
        toPos.GetComponent<DraggableObject>().OnDragChange += ToTrackStateChange;
        UpdateTransition();
        UpdateVisuals();
    }

    private void FromTrackStateChange(bool flag)
    {
        if (flag)
        {
            fromPos.GetComponent<TransitionChange>().OnAnyCollision += ChangeFrom;
        }
        else
        {
            fromPos.GetComponent<TransitionChange>().OnAnyCollision -= ChangeFrom;
        }
        ChangeMouse(flag);
        UpdateTransition();
        UpdateVisuals();
    }

    private void ToTrackStateChange(bool flag)
    {
        if (flag)
        {
            toPos.GetComponent<TransitionChange>().OnAnyCollision += ChangeTo;
        }
        else
        {
            toPos.GetComponent<TransitionChange>().OnAnyCollision -= ChangeTo;
        }
        ChangeMouse(flag);
        UpdateTransition();
            UpdateVisuals();
    }

    

    public void UpdateTransition()
    {
        float radius = 1.5f;
        
        if(from)
            fromPos.position= from.transform.position;
        if(to)
            toPos.position= to.transform.position;

        if (from)
        {
            fromPos.position += ((toPos.position - fromPos.position).normalized * radius);
            if (!from.transitions.ContainsKey(stringCondition))
            {
                from.transitions.Add(stringCondition,this);
            }
        }

        if (to)
        {
            toPos.position += (fromPos.position - toPos.position).normalized * radius;
            if (!to.transitionsto.ContainsKey(stringCondition))
            {
                to.transitionsto.Add(stringCondition,this);
            }
        }
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (from && !to && toPos.GetComponent<DraggableObject>().isDragging)
        {
            fromPos.position= from.transform.position;
            fromPos.position += ((toPos.position - fromPos.position).normalized * 1.5f);
        }
        if (to && !from && fromPos.GetComponent<DraggableObject>().isDragging)
        {
            toPos.position= to.transform.position;
            toPos.position += (fromPos.position - toPos.position).normalized * 1.5f;
        }
        
        lineRenderer.SetPosition(0,fromPos.position);
        arrow.position = toPos.position;
        arrow.right = (toPos.position - fromPos.position).normalized;
        lineRenderer.SetPosition(1,arrow.position);
        Vector3 arrowPos =toPos.position - (Vector3)(arrow.right * 1.5f);
        Vector3 midPoint = (fromPos.position + toPos.position) / 2;
        textLabel.position = midPoint + Vector3.up * 0.5f; 

        var dir = arrowPos - fromPos.position;
        if (dir.x < 0)
        {
            dir = fromPos.position - arrowPos;
        }
        textLabel.right = (dir).normalized;
    }
    private void Update()
    {
        if (isSelected)
        {
            UpdateVisuals();
        }
        
    }

    void ChangeFrom(State state,bool isEnter)
    {
        if (isEnter)
        {
            from = state;
            isSelected = false;
        }
        else
        {
            from.transitions.Remove(stringCondition);
            from = null;
            
            isSelected = true;
        }

        UpdateTransition();
        UpdateVisuals();
    }
    void ChangeTo(State state,bool isEnter)
    {
        if (isEnter)
        {
            to = state;
            
            isSelected = false;
        }
        else
        {
            to = null;
            isSelected = true;
        }

        UpdateTransition();
        UpdateVisuals();
    }
    void ChangeMouse(bool isEnter)
    {
        if (isEnter)
        {
            isSelected = true;
        }
        else
        {
            isSelected = false;
        }

        fromPos.position = lineRenderer.GetPosition(0);
        toPos.position = arrow.position;
    }
}
