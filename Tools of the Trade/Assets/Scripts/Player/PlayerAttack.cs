using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAttack : LockableMonoBehavior
{
    CharacterController controller;
    Animator animator;

    private readonly int jabHash = Animator.StringToHash("UAL_Armature_Punch_Jab");
    private readonly int crossHash = Animator.StringToHash("UAL_Armature_Punch_Cross");
    private readonly int uppercutHash = Animator.StringToHash("Uppercut Jab Rotation Fixed");
    private readonly int kickTwoHash = Animator.StringToHash("Inside Crescent Kick");
    private readonly int kickOneHash = Animator.StringToHash("Mma Kick");
    private readonly int kickParamHash = Animator.StringToHash("kick");
    private readonly int punchParamHash = Animator.StringToHash("punch");
    private readonly int comboStepHash = Animator.StringToHash("ComboStep"); // The Parameter
    public int comboStep;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Performs an initial jab if first entering Attack substate machine and initializes comboStep to 0. If already in Attack it triggers the next animation in the string by incrementing comboStep by one
    /// </summary>
    public void PerformAttackOne() {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var nextStateInfo = animator.GetNextAnimatorStateInfo(0); // Look at where we are going

        // Check if current state OR the state we are transitioning into has the tag
        bool isAttacking = stateInfo.IsTag("Attack") || nextStateInfo.IsTag("Attack");

        Debug.Log($"Attack Pressed! Tag: {(isAttacking ? "Attack" : "None")}, Step: {comboStep}");

        animator.ResetTrigger(punchParamHash); 
        animator.ResetTrigger(kickParamHash);

        
        if (isAttacking) { 
            if (comboStep < 3) {
                comboStep++;
                animator.SetInteger(comboStepHash, comboStep);
                animator.SetTrigger(punchParamHash);
            }
        } else {
            comboStep = 0;
            animator.SetInteger(comboStepHash, 0);
            animator.SetTrigger(punchParamHash);
        }
    }

    /// <summary>
    /// Performs an initial kick if first entering Attack substate machine and initializes comboStep to 0. If already in Attack it triggers the next animation in the string by incrementing comboStep by one
    /// </summary>
    public void PerformAttackTwo() {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var nextStateInfo = animator.GetNextAnimatorStateInfo(0); // Look at where we are going

        // Check if current state OR the state we are transitioning into has the tag
        bool isAttacking = stateInfo.IsTag("Attack") || nextStateInfo.IsTag("Attack");

        Debug.Log($"Attack Pressed! Tag: {(isAttacking ? "Attack" : "None")}, Step: {comboStep}");

        animator.ResetTrigger(punchParamHash); 
        animator.ResetTrigger(kickParamHash);
        
        if (isAttacking) { 
            if (comboStep < 3) {
                comboStep++;
                animator.SetInteger(comboStepHash, comboStep);
                animator.SetTrigger(kickParamHash);
            }
            
        } else {
            comboStep = 0;
            animator.SetInteger(comboStepHash, 0);
            animator.SetTrigger(kickParamHash);
        }
    }
}
