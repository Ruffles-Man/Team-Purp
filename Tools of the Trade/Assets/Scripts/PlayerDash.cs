using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerDash : LockableMonoBehavior
{
    [Tooltip("Duration of the dash in seconds")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashDistance = 4f;
    private CharacterController controller;
    private Animator animator;

    private readonly int dashHash = Animator.StringToHash("Dash");

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
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

        while (elapsedTime < dashDuration)
        {
            controller.Move(dashDistance * Time.deltaTime * dashDirection / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool(dashHash, false); // Stop dash animation
    }
}
