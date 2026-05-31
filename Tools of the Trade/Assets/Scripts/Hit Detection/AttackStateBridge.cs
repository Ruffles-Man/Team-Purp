using UnityEngine;

public class AttackStateBridge : StateMachineBehaviour
{
    private PlayerAttack playerAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerAttack == null)
        {
            playerAttack = animator.GetComponentInParent<PlayerAttack>();
        }

        if (playerAttack != null)
        {
            playerAttack.OnAttackStarted();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerAttack != null)
        {
            playerAttack.OnAttackAnimationComplete();
        }
    }
}