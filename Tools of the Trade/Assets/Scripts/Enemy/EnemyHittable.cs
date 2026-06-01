using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyHittable : MonoBehaviour, IHittable
{
    public UnityEvent<HitInfo> onHit;
    private readonly int highHash = Animator.StringToHash("HighHit");
    private readonly int midHash = Animator.StringToHash("MidHit");
    private readonly int backHash = Animator.StringToHash("BackLaunch");
    private readonly int upHash = Animator.StringToHash("UpLaunch");
    private readonly int dieHash = Animator.StringToHash("Die");

    [SerializeField] Animator animator;

    [SerializeField] private ParticleManager particleManager;

    private void Awake()
    {
        // If the inspector slot is empty (which it will be on a Prefab), find it dynamically!
        if (particleManager == null)
        {
            GameObject managerObj = GameObject.Find("Particle Manager");
            if (managerObj != null)
            {
                particleManager = managerObj.GetComponent<ParticleManager>();
            }
            else
            {
                Debug.LogWarning($"[Enemy] Could not find a ParticleManager in the scene context of {gameObject.name}!");
            }
        }
    }

    public void TakeHit(HitInfo info)
    {
        onHit.Invoke(info);
        HitType attackType = info.attackType;

        switch (attackType)
        {
            case HitType.High:
                //Play High Reaction
                Debug.Log("attackType: high");
                animator.SetTrigger(highHash);
                break;
            case HitType.Mid:
                //Play Mid Reaction
                Debug.Log("attackType: mid");
                animator.SetTrigger(midHash);
                break;
            case HitType.UpLaunch:
                Debug.Log("attackType: up");
                animator.SetTrigger(upHash);
                //Play Uppercut Launcher Reaction
                break;
            case HitType.BackLaunch:
                Debug.Log("attackType: back");
                animator.SetTrigger(backHash);
                //Play Flyback Reaction
                break;
            
        }
    }
}
