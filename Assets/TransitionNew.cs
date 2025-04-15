using System;
using _Scripts.Units;
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
    [SerializeField] public bool isSelected;

    private void Start()
    {
        
        fromPos.GetComponent<TransitionChange>().InitCollision(ChangeFrom);
        toPos.GetComponent<TransitionChange>().InitCollision(ChangeTo);
        fromPos.GetComponent<TransitionChange>().InitMouse(ChangeMouse);
        toPos.GetComponent<TransitionChange>().InitMouse(ChangeMouse);
        UpdateVisuals();
    }

    

    void UpdateTransition()
    {
        if (from)
        {
            fromPos.position= from.transform.position;
            float radius = 1.5f;
            if(from)radius=from.radius;
            
            if (to)
            {
                fromPos.position += ((to.transform.position - fromPos.position).normalized * radius);
            }
            else
            {
                fromPos.position += ((toPos.position - fromPos.position).normalized * radius);
            }

            if (from.transitions.ContainsKey(stringCondition))
            {
                
            }
            else
            {
                
                from.transitions.Add(stringCondition,this);
            }
        }
        

        if (to)
        {
            toPos.position = to.transform.position;
            float radius = 1.5f;
            if(to)radius=to.radius;
            
            if (from)
            {
                toPos.position += (from.transform.position - toPos.position).normalized * radius;
            }
            else
            {
                toPos.position += (fromPos.position - toPos.position).normalized * radius;
            }
        }
    }

    void UpdateVisuals()
    {
        lineRenderer.SetPosition(0,fromPos.position);
        arrow.position = toPos.position;
        arrow.right = (toPos.position - fromPos.position).normalized;
        lineRenderer.SetPosition(1,arrow.position);
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
