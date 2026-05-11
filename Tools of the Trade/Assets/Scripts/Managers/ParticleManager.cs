using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    public void SpawnHitEffect(HitInfo hitInfo)
    {
        Instantiate(hitEffectPrefab, hitInfo.position, Quaternion.identity);
    }
}
