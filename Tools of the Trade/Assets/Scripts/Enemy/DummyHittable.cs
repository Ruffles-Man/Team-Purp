using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DummyHittable : MonoBehaviour, IHittable
{
    [SerializeField] UnityEvent<HitInfo> onHit;

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
        particleManager.SpawnHitEffect(info);
    }
}
