using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement2D : MonoBehaviour
{
    public float dragSpeed = 2f;
    public Vector2 panLimit;

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI() || IsPointerOverWorldObject()) return;

            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = lastMousePosition - currentMousePosition;
            transform.position += difference * dragSpeed;
            lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x),
                Mathf.Clamp(transform.position.y, -panLimit.y, panLimit.y),
                transform.position.z
            );
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current && EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsPointerOverWorldObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        return hit.collider;
    }
}
