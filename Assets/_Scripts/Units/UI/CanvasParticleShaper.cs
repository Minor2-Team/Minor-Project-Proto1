using System;
using UnityEngine;

public class CanvasParticleShaper : MonoBehaviour
{
    private ParticleSystem ps;
    ParticleSystem.ShapeModule shapeModule;
    private RectTransform canvasRect;
    private float initialCanvasScale;
    private Vector2 initialStartSize;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        shapeModule = ps.shape;
        initialCanvasScale = canvasRect.localScale.x;
        initialStartSize = ps.shape.scale;
        
        UpdateParticleScale();
    }

    void LateUpdate()
    {
        if (Mathf.Abs(canvasRect.localScale.x - initialCanvasScale) > 0.001f)
        {
            UpdateParticleScale();
        }
    }

    void UpdateParticleScale()
    {
        var main = ps.main;
        float scaleRatio = canvasRect.localScale.x / initialCanvasScale;
        
        // Scale all size-related parameters
        
        shapeModule.scale = new(initialStartSize.x * scaleRatio, initialStartSize.y * scaleRatio, ps.shape.scale.z);
      

    }
}
