using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static InputSystem_Actions Input;

    private CharacterController controller;

    private Vector2 moveInput;
    private float verticalVelocity;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        Input = new InputSystem_Actions();
        Input.Enable();

        Input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += _ => moveInput = Vector2.zero;

        Input.Player.Jump.performed += _ => Jump();
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
       
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        float forwardInput = Mathf.Clamp(moveInput.y, 0f, 1f);

        Vector3 moveDirection =
            camForward.normalized * forwardInput +
            camRight.normalized * moveInput.x;

      
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
