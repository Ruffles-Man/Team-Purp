using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour
{
    private readonly int highHash = Animator.StringToHash("HighHit");
    private readonly int midHash = Animator.StringToHash("MidHit");
    private readonly int backHash = Animator.StringToHash("BackLaunch");
    private readonly int upHash = Animator.StringToHash("UpLaunch");
    Animator animator;
    public void HandleHitType(HitInfo hitInfo)
    {
        HitType attackType = hitInfo.attackType;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        switch (attackType)
        {
            case HitType.High:
                //Play High Reaction
                animator.SetTrigger(highHash);
                break;
            case HitType.Mid:
                //Play Mid Reaction
                animator.SetTrigger(highHash);
                break;
            case HitType.UpLaunch:
                animator.SetTrigger(highHash);
                //Play Uppercut Launcher Reaction
                break;
            case HitType.BackLaunch:
                animator.SetTrigger(highHash);
                //Play Flyback Reaction
                break;
            
        }
    }
}
