using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float sensitivity = 150f;

    private float yaw;
    private float pitch;

    private Vector2 lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Use the SAME input system from PlayerMovement
        PlayerMovement.Input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        PlayerMovement.Input.Player.Look.canceled += _ => lookInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        RotateCamera();
        FollowPlayer();
    }

    private void RotateCamera()
    {
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -35f, 60f);
    }

    private void FollowPlayer()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredPosition =
            target.position
            - (rotation * Vector3.forward * distance)
            + Vector3.up * height;

        transform.position = desiredPosition;
        transform.rotation = rotation;
    }
}
