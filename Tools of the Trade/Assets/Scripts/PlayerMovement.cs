using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed => moveSpeed * sprintMultiplier;
    public float CurrentSpeed => moveSpeed * (sprinting ? sprintMultiplier : 1f);
    float SpeedProgress => SmoothMoveInput.magnitude * CurrentSpeed / MaxSpeed; // Calculate speed as a percentage of max speed

    public Vector2 SmoothMoveInput = Vector2.zero;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField, HideInInspector] GameObject body;

    private CharacterController controller;
    private Animator animator;
    private InputSystem_Actions inputActions;
    private Vector2 velocity = Vector2.zero;
    private bool sprinting = false;

    // hashed animation references
    private readonly int speedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        HandleSprint(inputActions);
        HandleMove(inputActions);
    }

    private void HandleSprint(InputSystem_Actions actions)
    {
        sprinting = actions.Player.Sprint.IsPressed();
    }

    private void HandleMove(InputSystem_Actions actions)
    {
        // input processing
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        SmoothMoveInput = Vector2.SmoothDamp(SmoothMoveInput, moveInput, ref velocity, smoothTime); 
       
        // movement
        Vector3 moveDirection = new(SmoothMoveInput.x, 0, SmoothMoveInput.y);
        moveDirection = transform.TransformDirection(moveDirection); // Convert local movement to world space
        Vector3 moveVector = CurrentSpeed * Time.deltaTime * moveDirection;
        controller.Move(moveVector);

        // animation
        if (moveDirection != Vector3.zero)
        {
            animator.SetFloat(speedHash, SpeedProgress); // Update the animator with the movement speed
            body.transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }
        else
        {
            animator.SetFloat(speedHash, 0); // Set speed to 0 when not moving
        }
    }
}
