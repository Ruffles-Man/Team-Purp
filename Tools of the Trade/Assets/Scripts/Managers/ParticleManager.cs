using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject hitEffectPrefab;

    public void SpawnHitEffect(HitInfo hitInfo)
    {
        var hitVFX = Instantiate(hitEffectPrefab, hitInfo.position, Quaternion.identity);
        FindAnyObjectByType<PlayerSFX>().PlayAttackSFX(1);
        hitVFX.transform.SetParent(this.transform, true);
    }
}
