using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(PlayerSFX))]
[RequireComponent(typeof(PlayerVFX))]
public class PlayerDash : LockableMonoBehavior
{
    [Tooltip("Duration of the dash in seconds")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashDistance = 4f;
    private CharacterController controller;
    private Animator animator;
    PlayerSFX playerSFX;
    PlayerVFX playerVFX;

    private readonly int dashHash = Animator.StringToHash("Dash");

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerSFX = GetComponent<PlayerSFX>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    public void LockDash()
    {
        enabled = false;
    }

    public void UnlockDash()
    {
        enabled = true;
    }

    public IEnumerator Dash(Vector3 dashDirection)
    {
        float elapsedTime = 0f;

        // Play dash animation
        animator.SetBool(dashHash, true);
        playerSFX.PlayDashSFX();

        while (elapsedTime < dashDuration)
        {
            controller.Move(dashDistance * Time.deltaTime * dashDirection / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool(dashHash, false); // Stop dash animation
    }
}
