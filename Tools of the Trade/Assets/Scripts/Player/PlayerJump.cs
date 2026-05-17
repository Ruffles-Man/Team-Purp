using System;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(PlayerSFX))]
[RequireComponent(typeof(PlayerVFX))]
public class PlayerJump : LockableMonoBehavior
{
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] int numJumps = 2;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask groundMask;

    CharacterController controller;
    Animator animator;
    PlayerSFX playerSFX;
    PlayerVFX playerVFX;

    /// <summary>
    /// Current vertical velocity of the player. This is modified by jumping and gravity and applied to the character controller each frame in PerformVerticalMovement.
    /// </summary>
    private float verticalVelocity = 0f;

    /// <summary>
    /// Whether to check for grounding. This is disabled for a short time after jumping to prevent immediately re-grounding.
    /// </summary>
    private bool GroundCheckEnabled => jumpElapsedTime >= jumpGroundCheckDelay;

    /// <summary>
    /// Delay after jumping to NOT check for grounding to prevent immediately re-grounding after a jump.
    /// </summary>
    private readonly float jumpGroundCheckDelay = 0.2f;

    /// <summary>
    /// Time elapsed since the last jump was performed. Used to determine when to start checking for grounding again.
    /// </summary>
    private float jumpElapsedTime = 0f;

    /// <summary>
    /// Whether the player was grounded in the last frame. Used to trigger landing animations and reset jumps when landing.
    /// </summary>
    private bool landedLastFrame = false;

    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int landHash = Animator.StringToHash("Land");

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerSFX = GetComponent<PlayerSFX>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    /// <summary>
    /// Checks if the player is currently grounded by casting a sphere at the bottom of the character controller. The check is only performed if GroundCheckEnabled is true, which is false for a short time after jumping to prevent immediately re-grounding.
    /// </summary>
    /// <returns></returns>
    bool IsGrounded() 
    {
        // Cast a sphere from the bottom of the controller
        Vector3 spherePosition = new(transform.position.x, transform.position.y - (controller.height / 2), transform.position.z);
        return GroundCheckEnabled && Physics.CheckSphere(spherePosition, groundDistance, groundMask);
    }

    /// <summary>
    /// Performs vertical movement based on the current vertical velocity, applies gravity, and checks for landing to reset jumps and trigger animations.
    /// </summary>
    public void PerformVerticalMovement()
    {
        // Update jump elapsed time
        jumpElapsedTime += Time.deltaTime;

        // Apply vertical movement
        controller.Move(Time.deltaTime * verticalVelocity * Vector3.up);

        // Check for first landing frame
        if (!landedLastFrame && IsGrounded())
        {
            animator.SetTrigger(landHash);
            playerSFX.PlayLandSFX();
            numJumps = 2;
        }
        landedLastFrame = IsGrounded(); 

        // Ground check and gravity logic
        if (IsGrounded())
        {
            verticalVelocity = -2; // grounding force
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        
    }

    /// <summary>
    /// Performs a jump if the player has jumps remaining. Resets the jump timer, decrements jumps, triggers the jump animation, and sets the initial vertical velocity for the jump.
    /// </summary>
    public void PerformJump()
    {
        if (numJumps <= 0) return;

        jumpElapsedTime = 0f;
        numJumps--;
        landedLastFrame = false;
        animator.SetTrigger(jumpHash);
        playerSFX.PlayJumpSFX();
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
    }
}
