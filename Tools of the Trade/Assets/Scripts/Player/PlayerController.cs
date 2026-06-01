using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerCrouch))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerLockOn))]
[RequireComponent(typeof(PauseSystem))]
public class PlayerController : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerDash playerDash;
    PlayerJump playerJump;
    PlayerCrouch playerCrouch;
    PlayerAttack playerAttack;
    PlayerLockOn playerLockOn;
    PauseSystem pauseSystem;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();
        playerCrouch = GetComponent<PlayerCrouch>();
        playerLockOn = GetComponent<PlayerLockOn>();

        playerAttack = GetComponent<PlayerAttack>();

        inputActions = new InputSystem_Actions();

        if (pauseSystem == null)
        {
            GameObject managerObj = GameObject.Find("PauseSystem");
            if (managerObj != null)
            {
                pauseSystem = managerObj.GetComponent<PauseSystem>();
            }
            else
            {
                Debug.LogWarning($"[Enemy] Could not find a ParticleManager in the scene context of {gameObject.name}!");
            }
        }
    }

    void OnEnable()
    {
        GetComponentInChildren<Animator>().enabled = true;

        inputActions.Enable();
        inputActions.Player.Dash.performed += HandleDash;
        inputActions.Player.Jump.performed += HandleJump;
        inputActions.Player.Crouch.performed += HandleCrouch;
        inputActions.Player.Crouch.canceled += HandleCrouch;
        inputActions.Player.AttackOne.performed += HandleAttackOne;
        inputActions.Player.AttackTwo.performed += HandleAttackTwo;
        inputActions.Player.LockOn.performed += HandleLockOn;

        inputActions.Player.Pause.performed += pauseSystem.TogglePauseGame;
    }

    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Dash.performed -= HandleDash;
        inputActions.Player.Jump.performed -= HandleJump;
        inputActions.Player.Crouch.performed -= HandleCrouch;
        inputActions.Player.Crouch.canceled -= HandleCrouch;
        inputActions.Player.AttackOne.performed -= HandleAttackOne;
        inputActions.Player.AttackTwo.performed -= HandleAttackTwo;
        inputActions.Player.LockOn.performed -= HandleLockOn;

        inputActions.Player.Pause.performed -= pauseSystem.TogglePauseGame;
    }

    void Update()
    {
        if (pauseSystem.GetIsPaused()) { return; }
        playerJump.PerformVerticalMovement();
        if (!playerMovement._Locked) playerMovement.PerformSprint(inputActions);
        if (!playerMovement._Locked) playerMovement.PerformMove(inputActions);
        if (!playerCrouch._Locked) playerCrouch.PerformCrouch(inputActions);
    }

    protected void HandleLockOn(InputAction.CallbackContext context)
    {
        playerLockOn.PerformLockOn(context);
    }

    protected void HandleJump(InputAction.CallbackContext context)
    {
        if (playerJump._Locked) return;

        playerJump.PerformJump();
    }

    protected void HandleDash(InputAction.CallbackContext context)
    {
        if (playerDash._Locked) return;

        StartCoroutine(DashCoroutine(context));
    }

    protected void HandleCrouch(InputAction.CallbackContext context)
    {
        if (playerCrouch._Locked) return;

        if (context.performed)
        {
            playerCrouch.BeginCrouch();
            playerMovement.Lock(); // Lock movement while crouching
            playerDash.Lock(); // Lock dash while crouching
        }
        else if (context.canceled)
        {
            playerCrouch.EndCrouch();
            playerMovement.Unlock(); // Unlock movement when crouch is released
            playerDash.Unlock(); // Unlock dash when crouch is released
        }
    }

    IEnumerator DashCoroutine(InputAction.CallbackContext context)
    {
        playerMovement.Lock();

        yield return StartCoroutine(playerDash.Dash(transform.forward)); // Dash in the direction the player is facing

        playerMovement.Unlock();
    }

    protected void HandleAttackOne(InputAction.CallbackContext context)
    {
        playerMovement.Lock();
        playerCrouch.Lock();

        playerAttack.PerformAttackOne();

        playerAttack.OnAttackComplete += UnfreezePlayer;
    }

    protected void HandleAttackTwo(InputAction.CallbackContext context)
    {
        playerMovement.Lock();
        playerCrouch.Lock();
        playerAttack.PerformAttackTwo();

        playerAttack.OnAttackComplete += UnfreezePlayer;
    }

    // The clean callback method
    private void UnfreezePlayer()
    {
        playerCrouch.Unlock();
        playerMovement.Unlock();

        // TODO: Safely unsubscribe from the event completely
        playerAttack.OnAttackComplete -= UnfreezePlayer;
    }
}
