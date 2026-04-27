using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : LockableMonoBehavior
{
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField, HideInInspector] GameObject body;

    protected Vector2 SmoothMoveInput = Vector2.zero;

    private CharacterController controller;
    private Animator animator;
    private Vector2 velocity = Vector2.zero;

    // hashed animation references
    private readonly int crouchHash = Animator.StringToHash("Crouch");
    private readonly int crouchSpeedHash = Animator.StringToHash("CrouchSpeed");

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void BeginCrouch()
    {
        animator.SetBool(crouchHash, true);
        animator.SetFloat(crouchSpeedHash, 0f); 
    }

    public void PerformCrouch(InputSystem_Actions actions)
    {
        // input processing
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        SmoothMoveInput = Vector2.SmoothDamp(SmoothMoveInput, moveInput, ref velocity, smoothTime); 
       
        // movement
        Vector3 moveDirection = new(SmoothMoveInput.x, 0f, SmoothMoveInput.y);
        moveDirection = transform.TransformDirection(moveDirection); // Convert local movement to world space
        Vector3 moveVector = Time.deltaTime * moveDirection;
        controller.Move(moveVector);

        // animation & visuals
        animator.SetFloat(crouchSpeedHash, moveDirection.magnitude * crouchSpeed); // Update the animator with the crouch movement speed
        if (moveDirection != Vector3.zero)
        {
            body.transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }
    }

    public void EndCrouch()
    {
        animator.SetBool(crouchHash, false);
    }
}
