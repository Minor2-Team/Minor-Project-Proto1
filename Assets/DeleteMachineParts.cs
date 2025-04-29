using UnityEngine;

public class DeleteMachineParts : MonoBehaviour
{
    private DraggableObject selectedPart;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectMachinePart();
        }

        if (Input.GetKeyDown(KeyCode.Delete) && selectedPart)
        {
            DeleteSelectedPart();
        }
    }

    private void TrySelectMachinePart()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider && hit.collider.TryGetComponent(out DraggableObject obj))
        {
            selectedPart = obj;
        }
        else
        {
            selectedPart = null;
        }
    }

    private void DeleteSelectedPart()
    {
        Destroy(selectedPart.gameObject);
        selectedPart = null;
    }
}
