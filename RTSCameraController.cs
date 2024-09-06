using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float zoomSpeed = 25f;
    public float rotateSpeed = 100f;

    public float minZoom = 10f;
    public float maxZoom = 100f;

    public float borderThickness = 10f;

    private Vector3 targetPosition;
    private float targetZoom;
    private float targetRotationY;

    void Start()
    {
        targetPosition = transform.position;
        targetZoom = transform.position.y;
        targetRotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
        UpdateCameraTransform();
    }

    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // Keyboard input
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - borderThickness)
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= borderThickness)
            moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= borderThickness)
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - borderThickness)
            moveDirection += transform.right;

        targetPosition += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom - zoomDelta, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button
        {
            float rotationDelta = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            targetRotationY += rotationDelta;
        }
    }

    void UpdateCameraTransform()
    {
        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetZoom, targetPosition.z), Time.deltaTime * 5f);

        // Smoothly rotate the camera
        Quaternion targetRotation = Quaternion.Euler(30f, targetRotationY, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}