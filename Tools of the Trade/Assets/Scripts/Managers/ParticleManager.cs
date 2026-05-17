using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    public void SpawnHitEffect(HitInfo hitInfo)
    {
        var hitVFX = Instantiate(hitEffectPrefab, hitInfo.position, Quaternion.identity);
        hitVFX.transform.SetParent(this.transform, true);
    }
}
