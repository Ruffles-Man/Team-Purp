using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : LockableMonoBehavior
{
    /// <summary>
    /// Max speed is the maximum speed the player can reach when sprinting at full speed.
    /// </summary>
    protected float MaxSpeed => moveSpeed * sprintMultiplier;

    /// <summary>
    /// Current speed is the player's current speed based on their base move speed and any sprinting bonus.
    /// </summary>
    protected float CurrentSpeed => moveSpeed + BonusSpeed;

    /// <summary>
    /// Bonus speed is any additional speed gained from sprinting.
    /// </summary>
    protected float BonusSpeed => moveSpeed * SprintProgress * (sprintMultiplier - 1f);

    /// <summary>
    /// Speed progress is a value between 0 and 1 representing how fast the player is moving for animation purposes.
    /// </summary>
    protected float SpeedProgress => SmoothMoveInput.magnitude * CurrentSpeed / MaxSpeed; // Calculate speed as a percentage of max speed

    /// <summary>
    /// Sprint progress is a value between 0 and 1 representing how much of the sprint speed bonus is currently applied based on how long the player has been sprinting.
    /// </summary>
    protected float SprintProgress => Mathf.Clamp01(elapsedSprintTime / timeToMaxSprint);

    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float timeToMaxSprint = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField, HideInInspector] GameObject body;
    protected Vector2 SmoothMoveInput = Vector2.zero;


    private CharacterController controller;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;
    private bool sprinting = false;
    private float elapsedSprintTime = 0f;

    // hashed animation references
    private readonly int speedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    public void PerformSprint(InputSystem_Actions actions)
    {
        // set sprint time based on input
        if (actions.Player.Sprint.WasPressedThisFrame())
        {
            elapsedSprintTime = 0f;
        }
        if (actions.Player.Sprint.WasReleasedThisFrame())
        {
            // capture the sprint time at the moment of release for smooth deceleration
            elapsedSprintTime = Mathf.Clamp(elapsedSprintTime, 0f, timeToMaxSprint);
        }

        sprinting = actions.Player.Sprint.IsPressed();
        if (sprinting)
        {
            // works up to full sprint speed over timeToMaxSprint seconds
            elapsedSprintTime += Time.deltaTime;
        }
        else
        {
            // works down to no sprint speed over timeToMaxSprint seconds
            elapsedSprintTime -= Time.deltaTime;
        }
    }

    public void PerformMove(InputSystem_Actions actions)
    {
        // input processing
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        SmoothMoveInput = Vector2.SmoothDamp(SmoothMoveInput, moveInput, ref velocity, smoothTime); 
       
        // movement
        Vector3 moveDirection = new(SmoothMoveInput.x, 0f, SmoothMoveInput.y);
        moveDirection = transform.TransformDirection(moveDirection); // Convert local movement to world space
        Vector3 moveVector = CurrentSpeed * Time.deltaTime * moveDirection;
        controller.Move(moveVector);

        // animation & visuals
        animator.SetFloat(speedHash, SpeedProgress); // Update the animator with the movement speed
        if (moveDirection != Vector3.zero)
        {
            body.transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }
    }
}
