using UnityEngine;

public class CameraMovement2D : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w") )
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") )
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") )
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") )
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
