using UnityEngine;
using UnityEngine.InputSystem;

public class stringAttackScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    public InputActionReference punch;
    public InputActionReference kick;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        punch.action.started += Punch;
        punch.action.Enable();
        kick.action.started += Kick;
        kick.action.Enable();
    }

    private void OnDisable()
    {
        punch.action.started -= Punch;
        punch.action.Disable();
        kick.action.started -= Kick;
        kick.action.Enable();
    }

    private void Punch(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("punch");
        Debug.Log("Punched!");
    }

    private void Kick(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("kick");
        Debug.Log("Kicked!");
    }
}
