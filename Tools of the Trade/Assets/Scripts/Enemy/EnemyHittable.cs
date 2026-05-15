using UnityEngine;
using UnityEngine.Events;

public class EnemyHittable : MonoBehaviour, IHittable
{
    [SerializeField] UnityEvent<HitInfo> onHit;

    public void TakeHit(HitInfo info)
    {
        onHit.Invoke(info);
    }
}
