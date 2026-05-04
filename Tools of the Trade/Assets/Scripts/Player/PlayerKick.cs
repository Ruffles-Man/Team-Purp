using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerKick : LockableMonoBehavior
{
    CharacterController controller;
    Animator animator;

    private readonly int kickTwoHash = Animator.StringToHash("Inside Crescent Kick");
    private readonly int kickOneHash = Animator.StringToHash("Mma Kick");
    private readonly int kickParamHash = Animator.StringToHash("kick");
    private readonly int comboStepHash = Animator.StringToHash("comboStep"); // The Parameter
    public int comboStep;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Performs an initial kick if first entering Attack substate machine and initializes comboStep to 0. If already in Attack it triggers the next animation in the string by incrementing comboStep by one
    /// </summary>
    public void PerformKick() {
        // 1. Get the current state
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        // 2. Check if we are ALREADY in an attack state
        if (stateInfo.IsTag("Attack")) { 
            // We are already attacking, so this click is a COMBO request
            comboStep++; 
            animator.SetInteger(comboStepHash, comboStep);
        } else {
            // We are NOT attacking yet, so this is the INITIAL strike
            animator.SetTrigger(kickParamHash);
            comboStep = 0;
            animator.SetInteger(comboStepHash, 0);
        }
    }
}
