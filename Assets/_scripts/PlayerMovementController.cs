using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    Camera mainCam;

    [Header("Movement Variables")]
    [SerializeField] float MoveSpeed = 4f;
    [SerializeField] float SprintSpeed = 8f;
    [SerializeField] float JumpHeight = 1f;
    [SerializeField] float RotationSpeed = .5f;

    Vector3 velocity;

    PlayerInput playerInput;
    InputManager inputManager;

    float pitch = 0f;
    
    [Header("Ground Check Variables")]
    public bool isGrounded = true;
    public LayerMask GroundLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        GroundCheck();
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void OnDrawGizmos()
    {
        // GravityCheck gizmo
        Vector3 spherePosition = new(transform.position.x, transform.position.y - 8f, transform.position.z);
        
        if (isGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawSphere(spherePosition, controller.radius);
    }

    void Jump()
    {
        if (isGrounded)
        {
            if (velocity.y < 0f)
            {
                velocity.y = -1f;
            }

            if (inputManager.isJumping)
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;

            inputManager.isJumping = false;
        }
    }

    void GroundCheck()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - 0.8f, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    void Move()
    { 
        Vector3 inputDirection = new Vector3(inputManager.moveInput.x, 0f, inputManager.moveInput.y).normalized;    

        Vector3 moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.z;

        float speed = inputManager.isSprinting ? SprintSpeed : MoveSpeed;
        
        controller.Move((speed * Time.deltaTime) * moveDirection + (velocity * Time.deltaTime));
    }

    private void Look()
    {
        pitch -= inputManager.lookInput.y * RotationSpeed * 1f;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        mainCam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * (inputManager.lookInput.x * RotationSpeed * 1f));
    }
}
