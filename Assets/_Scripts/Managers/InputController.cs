using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public float zoomSpeed = 2f;  // Speed of zooming
    public float minZoom = 5f;    // Minimum zoom level
    public float maxZoom = 20f;   // Maximum zoom level

    private Camera cam;
    private InputSystem_Actions controls;

    void Awake()
    {
        cam = Camera.main;
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Zoom.performed += Zoom;
    }

    void OnDisable()
    {
        controls.Player.Zoom.performed -= Zoom;
        controls.Disable();
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        float scrollInput = context.ReadValue<float>();
        print(scrollInput);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollInput * zoomSpeed, minZoom, maxZoom);
    }
}
