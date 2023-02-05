using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float mouseEdgeThreshold = 10.0f;
    public float dragSpeed = 2.0f;

    public float zoomSpeed = 10.0f;
    public float zoomMin = 5.0f;
    public float zoomMax = 15.0f;

    float movementTime = 0;


    private Vector3 dragStart;
    private Vector3 dragEnd;
    private Vector3 dragDelta;
    private bool dragging = false;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = Camera.main.orthographicSize - scroll * zoomSpeed * Time.deltaTime;
        newZoom = Mathf.Clamp(newZoom, zoomMin, zoomMax);
        Camera.main.orthographicSize = newZoom;

        float zoomAdjust = newZoom / 5;


        Vector3 movement = Vector3.zero;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement += new Vector3(horizontal, vertical);

        if(Time.time>1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStart = Input.mousePosition;
                dragging = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }

            if (dragging)
            {
                dragEnd = Input.mousePosition;
                dragDelta = dragEnd - dragStart;
                transform.position -= new Vector3(dragDelta.x, dragDelta.y, 0) *zoomAdjust* Time.deltaTime * dragSpeed;
                dragStart = dragEnd;
                return;
            }
        }

        float ramp = Mathf.Lerp(0.25f,1, Mathf.Clamp(movementTime / 2f, 0, 1f));
        movement = movement.normalized * movementSpeed*ramp*zoomAdjust * Time.deltaTime;
        transform.position += movement;

        if (movement.magnitude != 0)
            movementTime += Time.deltaTime;
        else 
            movementTime = 0;

        Vector4 mapExtends = HexMap.main.mapExtends;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapExtends.x - 3, mapExtends.y + 3), Mathf.Clamp(transform.position.y, mapExtends.z - 3, mapExtends.w + 3), -10);
    }
}