using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerDash : MonoBehaviour
{
    [Tooltip("Duration of the dash in seconds")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashDistance = 4f;
    private PlayerMovement movement;
    private CharacterController controller;
    private Animator animator;

    private readonly int dashHash = Animator.StringToHash("Dash");

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    public IEnumerator Dash(Vector3 dashDirection)
    {
        float elapsedTime = 0f;
        float dashDuration = 0.2f; // Duration of the dash in seconds

        // Play dash animation
        animator.SetBool(dashHash, true);

        while (elapsedTime < dashDuration)
        {
            controller.Move(dashDistance * Time.deltaTime * dashDirection / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool(dashHash, false); // Stop dash animation
        movement.UnlockMovement(); // Unlock movement after dash
    }
}
