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

        

        
        from.transitions.Add(stringCondition,this);
        UpdateTransition();
    }

    void Update()
    {
        if(from || to)
            UpdateTransition();
        else
        {
            lineRenderer.SetPosition(0,lineRenderer.transform.position);
            lineRenderer.SetPosition(1,transitionArrowObject.transform.position);
        }
    }

    void UpdateTransition()
    {

        Vector3 fromPos=transform.position, toPos=transform.position;
        if (from)
        {
             fromPos = from.transform.position;
        }
        if (to)
        {
             toPos = to.transform.position;
        }
        
        if (from == to)
        {
            CreateLoopTransition(fromPos, from.radius);
            return;
        }
        
        Vector2 direction = (toPos - fromPos).normalized;
        float arrowOffset=1.5f;
        if (to)
        {
             arrowOffset = to.radius; 
            
        }
        float radius = 1.5f;
        if(from)radius=from.radius;
        Vector3 adjustedFromPos = fromPos + (Vector3)(direction * radius);
        Vector3 arrowPos = toPos - (Vector3)(direction * arrowOffset);
        lineRenderer.positionCount = 2;
        lineRenderer.transform.position = adjustedFromPos;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, arrowPos);


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transitionArrowObject.transform.position = arrowPos;
        transitionArrowObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        if (lineRenderer == null || textLabel == null)
            return;

        Vector3 midPoint = (adjustedFromPos + arrowPos) / 2;
        textLabel.transform.position = midPoint + Vector3.up * 0.5f; 

        var dir = arrowPos - adjustedFromPos;
        if (dir.x < 0)
        {
            dir = adjustedFromPos - arrowPos;
        }
        textLabel.transform.right = (dir).normalized;

    }

    
        void CreateLoopTransition(Vector3 center, float radius)
        {
            int segmentCount = 20; 
            float loopRadius = radius * 1.2f; 
            lineRenderer.positionCount = segmentCount + 1;

            for (int i = 0; i <= segmentCount; i++)
            {
                float angle = i / (float)segmentCount * Mathf.PI * 2; 
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * loopRadius;
                lineRenderer.SetPosition(i, center + offset);
            }

            Vector3 arrowPos = center + new Vector3(0, loopRadius, 0);
            transitionArrowObject.transform.position = arrowPos;
            transitionArrowObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            textLabel.transform.position = center + new Vector3(0, loopRadius * 1.3f, 0);
            textLabel.transform.rotation = Quaternion.identity;
        }
    


    void UpdateLabelPosition()
    {
        
    }
}
