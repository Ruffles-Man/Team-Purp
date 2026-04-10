using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField, HideInInspector] GameObject body;
    private CharacterController controller;
    private Animator animator;
    private InputSystem_Actions inputActions;

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
        HandleMove(inputActions);
    }

    private void HandleMove(InputSystem_Actions actions)
    {
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new(moveInput.x, 0, moveInput.y);
        if (moveDirection != Vector3.zero)
        {
            animator.SetFloat("Speed", moveDirection.magnitude); // Update the animator with the movement speed
            body.transform.rotation = Quaternion.LookRotation(moveDirection.normalized); // Rotate the body to face the movement direction
        }
        else
        {
            animator.SetFloat("Speed", 0); // Set speed to 0 when not moving
        }
        moveDirection = transform.TransformDirection(moveDirection); // Convert local movement to world space
        controller.Move(moveSpeed * Time.deltaTime * moveDirection);
    }
}
