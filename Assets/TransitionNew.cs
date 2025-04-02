using System;
using _Scripts.Units;
using UnityEngine;

public class TransitionNew : MonoBehaviour
{
    [SerializeField]public State from;
    [SerializeField]public State to;
    [SerializeField]public State From { 
        get => from;
        set
        {
            from = value;
            if (!from)
            {
               // Vector2 direction = (toPos - from.transform.position).normalized;
                lineRenderer.SetPosition(0,from.transform.position);
            }
            
        } 
    }
    [SerializeField] LineRenderer lineRenderer;

    private void Awake()
    {
    }
}
