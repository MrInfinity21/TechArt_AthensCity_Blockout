using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Vector2 lookInput;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 120f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    private float xRotation;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += _ => lookInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void LateUpdate()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
