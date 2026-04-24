using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerJump))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, HideInInspector] private GameObject body;

    PlayerMovement playerMovement;
    PlayerDash playerDash;
    PlayerJump playerJump;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();

        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Dash.performed += HandleDash;
        inputActions.Player.Jump.performed += HandleJump;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Dash.performed -= HandleDash;
        inputActions.Player.Jump.performed -= HandleJump;
    }

    void Update()
    {
        playerJump.PerformVerticalMovement();
        playerMovement.PerformSprint(inputActions);
        playerMovement.PerformMove(inputActions);
    }

    protected void HandleJump(InputAction.CallbackContext context)
    {
        playerJump.PerformJump();
    }

    protected void HandleDash(InputAction.CallbackContext context)
    {
        StartCoroutine(DashCoroutine(context));
    }

    IEnumerator DashCoroutine(InputAction.CallbackContext context)
    {
        playerMovement.LockMovement();

        yield return StartCoroutine(playerDash.Dash(body.transform.forward)); // Dash in the direction the player is facing

        playerMovement.UnlockMovement();
    }

}
