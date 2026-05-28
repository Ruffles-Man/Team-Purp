using UnityEngine;
using UnityEngine.TextCore.Text;
[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(PlayerSFX))]
[RequireComponent(typeof(PlayerVFX))]
[RequireComponent(typeof(PlayerLockOn))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerCrouch : LockableMonoBehavior
{
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float smoothTime = 0.25f;

    protected Vector2 SmoothMoveInput = Vector2.zero;

    private CharacterController controller;
    private Animator animator;
    PlayerSFX playerSFX;
    PlayerVFX playerVFX;
    PlayerLockOn playerLockOn;
    private Vector2 velocity = Vector2.zero;

    // hashed animation references
    private readonly int crouchHash = Animator.StringToHash("Crouch");
    private readonly int crouchSpeedHash = Animator.StringToHash("CrouchSpeed");

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerSFX = GetComponent<PlayerSFX>();
        playerVFX = GetComponent<PlayerVFX>();
        playerLockOn = GetComponent<PlayerLockOn>();
    }

    public void BeginCrouch()
    {
        animator.SetBool(crouchHash, true);
        animator.SetFloat(crouchSpeedHash, 0f);
        playerSFX.PlayCrouchSFX();
    }

    public void PerformCrouch(InputSystem_Actions actions)
    {
        if (!GetComponent<PlayerMovement>()._Locked) return; // prevent crouch if moving normally

        // input processing
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        SmoothMoveInput = Vector2.SmoothDamp(SmoothMoveInput, moveInput, ref velocity, smoothTime);
        Vector3 moveDirection = new(SmoothMoveInput.x, 0f, SmoothMoveInput.y);

        // movement
        Vector3 moveVector = crouchSpeed * Time.deltaTime * moveDirection;
        controller.Move(moveVector);

        // animation & visuals
        animator.SetFloat(crouchSpeedHash, moveDirection.magnitude * crouchSpeed); // Update the animator with the crouch movement speed
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }

        // rotation (locked vs unlocked)
        if (playerLockOn.lockedOnTarget != null)
        {
            transform.LookAt(playerLockOn.lockedOnTarget.transform.position); // Ensure the player is always facing the locked-on target
        }
        else if (moveDirection.sqrMagnitude > 0.01f) // only rotate if there's significant movement input
        {
            transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }
    }

    public void EndCrouch()
    {
        animator.SetBool(crouchHash, false);
        playerSFX.PlayCrouchSFX();
    }
}
