using System;
using _Scripts.Units;
using TMPro;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private char stringCondition;
    [SerializeField] public State from;
    [SerializeField] public State to;
    [SerializeField] private GameObject transitionLineObject;
    [SerializeField] private GameObject transitionArrowObject;
    [SerializeField] private Canvas canvas;
    private LineRenderer lineRenderer;
    public TextMeshProUGUI textLabel;

    private void Awake()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera=Camera.main;
        textLabel = canvas.GetComponentInChildren<TextMeshProUGUI>();
        textLabel.text=""+stringCondition;
    }

    void Start()
    {
        lineRenderer = transitionLineObject.GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            return;
        }
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0,lineRenderer.transform.position);
        lineRenderer.SetPosition(1,transitionArrowObject.transform.position);
        if (!from || !to || !transitionLineObject || !transitionArrowObject)
        {
            return;
        }

        // Get LineRenderer component
        

        
        from.transitions.Add(stringCondition,this);
        UpdateTransition();
    }

    void Update()
    {
        if(from && to)
            UpdateTransition();
        else
        {
            lineRenderer.SetPosition(0,lineRenderer.transform.position);
            lineRenderer.SetPosition(1,transitionArrowObject.transform.position);
        }
    }

    void UpdateTransition()
    {
        if (from == null || to == null) return;

        // Set the line renderer positions
        Vector3 fromPos = from.transform.position;
        Vector3 toPos = to.transform.position;
        
        
        
        Vector2 direction = (toPos - fromPos).normalized;
        float arrowOffset = to.radius; 
        Vector3 adjustedFromPos = fromPos + (Vector3)(direction * from.radius);
        Vector3 arrowPos = toPos - (Vector3)(direction * arrowOffset);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, adjustedFromPos);
        lineRenderer.SetPosition(1, arrowPos);


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Position the arrow at the `to` state and rotate it
        transitionArrowObject.transform.position = arrowPos;
        transitionArrowObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        if (lineRenderer == null || textLabel == null)
            return;

        // Find the midpoint of the LineRenderer
        Vector3 midPoint = (adjustedFromPos + arrowPos) / 2;
        textLabel.transform.position = midPoint + Vector3.up * 0.5f; 

        // Rotate text to face the camera
        var dir = arrowPos - adjustedFromPos;
        if (dir.x < 0)
        {
            dir = adjustedFromPos - arrowPos;
        }
        textLabel.transform.right = (dir).normalized;

    }
    
    void UpdateLabelPosition()
    {
        
    }
}
