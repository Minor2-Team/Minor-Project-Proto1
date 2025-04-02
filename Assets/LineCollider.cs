using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class LineCollider : MonoBehaviour
{
    [SerializeField]private float edgeColliderRadiusMargin;
    [SerializeField]private float edgeColliderEndPointMargin;
    private EdgeCollider2D _edgeCollider2D;
    private LineRenderer _lineRenderer;
    private Vector3[] _previousLinePositions;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        
        _edgeCollider2D.edgeRadius = _lineRenderer.startWidth / 2f + edgeColliderRadiusMargin;
    }

    private void OnValidate()
    {
        if (_edgeCollider2D != null)
        {
            _edgeCollider2D.edgeRadius = _lineRenderer.startWidth / 2f + edgeColliderRadiusMargin;
        }
    }

    private void Start()
    {
        UpdateCollider();
    }

    private void Update()
    {
        if (HasLineChanged())
        {
            UpdateCollider();
        }
    }

    private bool HasLineChanged()
    {
        if (_previousLinePositions == null || _previousLinePositions.Length != _lineRenderer.positionCount)
            return true;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            if (_previousLinePositions[i] != _lineRenderer.GetPosition(i))
                return true;
        }
        return false;
    }

    private void UpdateCollider()
    {
        int pointCount = _lineRenderer.positionCount;
        if (pointCount < 2) return; // EdgeCollider2D needs at least 2 points

        List<Vector2> colliderPoints = new List<Vector2>(pointCount);
        for (int i = 0; i < pointCount; i++)
        {
            // Convert world position to local position
            Vector3 worldPos = _lineRenderer.GetPosition(i);
            Vector2 localPos = transform.InverseTransformPoint(worldPos);
            colliderPoints.Add(localPos);
        }
        Vector2 direction = (colliderPoints[^1] - colliderPoints[^2]).normalized;
        colliderPoints[^1] -= direction * edgeColliderEndPointMargin;
        _edgeCollider2D.SetPoints(colliderPoints);
        // Cache new positions for future comparisons
        _previousLinePositions = new Vector3[pointCount];
        _lineRenderer.GetPositions(_previousLinePositions);
    }
}
